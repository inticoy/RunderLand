using System;
using System.Net;
using System.Net.Http;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            string url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // URL
            url += "?ServiceKey=" + "서비스키"; // Service Key
            url += "&pageNo=1";
            url += "&numOfRows=1000";
            url += "&dataType=XML";
            url += "&base_date=20210628";
            url += "&base_time=0600";
            url += "&nx=55";
            url += "&ny=127";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = string.Empty;
            HttpWebResponse response;
            using (response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();
            }

            Console.WriteLine(results);
        }
    }
}