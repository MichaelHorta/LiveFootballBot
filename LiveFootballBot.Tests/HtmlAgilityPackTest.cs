using Xunit;
using HtmlAgilityPack;
using System.Text;

namespace LiveFootballBot.Tests
{
    public class HtmlAgilityPackTest
    {
        [Fact]
        private void ExecuteScrapper()
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            //StreamReader reader = new StreamReader(WebRequest.Create("https://www.marca.com/futbol/primera-division/directo/2019/11/03/5dbeed8d268e3e36298b456f.html").GetResponse().GetResponseStream(), Encoding.GetEncoding("ISO-8859-1")); //put your encoding            
            //htmlDocument.Load(reader);

            var htmlWeb = new HtmlWeb
            {
                OverrideEncoding = Encoding.GetEncoding("ISO-8859-1")
            };

            
            htmlDocument = htmlWeb.LoadFromWebAsync("https://www.marca.com/futbol/primera-division/directo/2019/11/03/5dbeed8d268e3e36298b456f.html", Encoding.GetEncoding("ISO-8859-1")).GetAwaiter().GetResult();
            var nodes = htmlDocument.DocumentNode.SelectNodes("//li[@class='ue-c-stream-live__post']");

            foreach (var n in nodes)
            {
                var commentNode = n.SelectSingleNode(".//div[@class='ue-c-stream-live__main']/div[@class='ue-c-stream-live__comment']/p");
                if (null == commentNode)
                    continue;

                var icon = n.SelectSingleNode(".//div[@class='ue-c-stream-live__main']/div[@class='ue-c-stream-live__icon']/i");
                var iconClass = null != icon ? icon.GetAttributeValue("class", string.Empty) : string.Empty;
                var time = n.SelectSingleNode(".//div[@class='ue-c-stream-live__main']/div[@class='ue-c-stream-live__time']/span[@role='heading']").InnerText;
                var comment = commentNode.InnerText;
            }
        }
    }
}
