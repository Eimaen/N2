using Leaf.xNet;
using System.Collections.Generic;

namespace Nitrogen
{
    public static class ProxyQueue
    {
        public static void UpdateList(List<string> list)
        {
            Proxies = list;
            currentProxy = 0;
        }

        public static List<string> Proxies { get; private set; }
        private static int currentProxy = 0;

        public static ProxyClient NextProxy()
        {
            if (Proxies == null || Proxies.Count == 0)
                return null;
            ProxyClient client = null;
            while (Proxies[currentProxy] == string.Empty)
            {
                currentProxy++;
                currentProxy %= Proxies.Count;
            }
            try
            {
                client = ProxyClient.Parse(Proxies[currentProxy]);
            }
            catch { }
            currentProxy++;
            currentProxy %= Proxies.Count;
            return client;
        }

        public static void Remove(string proxy)
        {
            for (int i = 0; i < Proxies.Count; i++)
                if (Proxies[i] == proxy)
                    Proxies[i] = string.Empty;
        }
    }
}