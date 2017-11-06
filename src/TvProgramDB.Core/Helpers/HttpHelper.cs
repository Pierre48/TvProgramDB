using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TvProgramDB.Core.Helpers
{
    public static class HttpHelper
    {
        public static string GetHtml(string url)
        {
            string result = "";
            var myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            using (var myResponse = myRequest.GetResponse())
            {
                using (var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                myResponse.Close();
            }
            return result;
        }
    }
}
