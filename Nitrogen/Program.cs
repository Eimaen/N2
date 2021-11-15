using Leaf.xNet;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Nitrogen
{
    public class Program
    {
        private static Webhook webhook;

        private static int found = 0, @checked = 0, hits = 0;

        private static void UpdateTitle() => Console.Title = $"Nitrogen / Hits: {hits} / Checked: {@checked} / Found: {found} / Proxies: {ProxyQueue.Proxies.Count}";

        public static bool Check(string code)
        {
            var request = new HttpRequest();
            request.IgnoreProtocolErrors = true;

            while (true)
            {
                request.UserAgent = Settings.Default.UserAgent;
                request.Proxy = ProxyQueue.NextProxy();
                if (request.Proxy != null)
                    request.Proxy.ConnectTimeout = int.MaxValue;
                hits++;
                try
                {
                    HttpResponse httpResponse = request.Get($"https://discord.com/api/v9/entitlements/gift-codes/{code}?country_code={Settings.Default.CountryCode}&with_application=true&with_subscription_plan=true");
                    string response = httpResponse.ToString();
                    if (response.ToLower().Contains("rate limited")) // There is a rate limit, use another proxy.
                    {
                        Console.WriteLine($"\u001b[91mRate    \u001b[96m{code} \u001b[37musing {(request.Proxy == null ? "no proxy" : $"proxy {request.Proxy} ({request.Proxy.Type})")}.");
                        continue;
                    }
                    if (response.ToLower().Contains("nitro") || response.Contains(code)) // This IS the code.
                    {
                        @checked++; found++;
                        Console.WriteLine($"\u001b[92mFound   \u001b[96m{code} \u001b[37musing {(request.Proxy == null ? "no proxy" : $"proxy {request.Proxy} ({request.Proxy.Type})")}.");
                        if (Settings.Default.EnableWebhook) 
                            webhook.SendMessage($"https://discord.gift/{code}");
                        UpdateTitle();
                        return true;
                    }
                    if (response.ToLower().Contains("unknown gift code") || response.Contains("error code:")) // This IS NOT the code.
                    {
                        @checked++;
                        Console.WriteLine($"\u001b[36mChecked \u001b[96m{code} \u001b[37musing {(request.Proxy == null ? "no proxy" : $"proxy {request.Proxy} ({request.Proxy.Type})")}.");
                        UpdateTitle();
                        return false;
                    }
                    if (httpResponse.StatusCode == HttpStatusCode.OK && !Settings.Default.IgnoreSuccessfulRequestsWithoutNitro) // We don't know whether this code is Nitro, but still want to save it as it returns status code 200.
                    {
                        Console.WriteLine($"\u001b[92mFailed  \u001b[96m{code} \u001b[37musing {(request.Proxy == null ? "no proxy" : $"proxy {request.Proxy} ({request.Proxy.Type})")}.\n{response}");
                        if (Settings.Default.EnableWebhook) 
                            webhook.SendMessage($"https://discord.gift/{code} \nSucceeded, but failed to process the request.\nResponse:\n```{response}```");
                        UpdateTitle();
                        return true;
                    }
                    return false;
                }
                catch (HttpException) { Console.WriteLine($"\u001b[91mFailed  \u001b[37mwith proxy {request.Proxy} ({request.Proxy.Type}), retrying with another one..."); }
                catch (ProxyException) { Console.WriteLine($"\u001b[91mFailed  \u001b[37mwith proxy {request.Proxy} ({request.Proxy.Type}), retrying with another one..."); }
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length) => new string(Enumerable.Repeat("abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789", length).Select(s => s[random.Next(s.Length)]).ToArray());

        public static void Work()
        {
            Thread.Sleep(random.Next(1000));
            while (true)
            {
                string code = RandomString(Settings.Default.CodeLength);
                if (Check(code))
                    File.AppendAllText("results.txt", code + '\n');
            }
        }

        public static void Main(string[] args)
        {
            Settings.Default.Save();
            if (Settings.Default.EnableWebhook)
            {
                webhook = new Webhook(Settings.Default.WebhookUrl) { Username = Settings.Default.WebhookName };
                webhook.SendMessage("Started up successfully!");
            }
            ThreadPool.SetMaxThreads(Settings.Default.ThreadCount * 2, Settings.Default.ThreadCount);
            ProxyQueue.UpdateList(File.ReadAllLines("proxies.txt").ToList());
            for (int i = 0; i < Settings.Default.ThreadCount; i++)
                new Thread(Work).Start();
            while (true)
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                    new Thread(() => Check("6uJH3BRQHCdapZCZsewwKyB5")).Start();
        }
    }
}