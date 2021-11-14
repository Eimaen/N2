using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitrogen
{
    public class Webhook
    {
        public string Username { get; set; }
        public string Picture { get; set; }
        private string Url;

        public Webhook(string url)
        {
            Url = url;
        }

        public void SendMessage(string msgSend)
        {
            using (var request = new HttpRequest())
            {
                var reqParams = new RequestParams();

                reqParams["username"] = Username;
                reqParams["avatar_url"] = Picture;
                reqParams["content"] = msgSend;

                request.Post(Url, reqParams);
            }
        }
    }
}
