using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Services.Article.WebParsers
{
    internal class EkoPortalParser : AbstractParser
    {
        private readonly HtmlDocument? htmlDoc;

        public EkoPortalParser(String html)
        {
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            htmlDoc = web.Load("https://ecoportal.su/news/view/"+html+ ".html");
        }
        
        internal override String GetArticleSourceReference(String id)
        {
            return "https://ecoportal.su/news/view/"+ id + ".html";

        }

        internal override List<string> GetArticleTagsFromRss(List<string> categories)
        {
            List<String> tags = new List<string>();
            foreach (string category in categories)
            {
                switch (category)
                {
                    case "Климат":
                        tags.Add("позновательное");
                        break;
                    case "Интересно":
                        tags.Add("позновательное");
                        break;
                    case "Наука":
                        tags.Add("позновательное");
                        break;
                    case "История":
                        tags.Add("позновательное");
                        break;
                    case "Новости законодательства":
                        tags.Add("позновательное");
                        break;
                    case "Зелёные технологии":
                        tags.Add("позновательное");
                        break;
                    case "Продукты":
                        tags.Add("лайфстайл");
                        break;
                    case "Здоровье":
                        tags.Add("лайфстайл");
                        break;
                    case "Природа":
                        tags.Add("животные");
                        break;
                    case "Домашние животные":
                        tags.Add("животные");
                        break;
                    case "Автомобили":
                        tags.Add("авто");
                        break;

                }
            }

            return tags;
        }

        internal override String GetPictureReference()
        {
            var imgNode = htmlDoc.DocumentNode.SelectSingleNode("//newsimage/img");
            var pattern = @"(?<=src="")[^""]+\.jpg(?="")";
            var match = Regex.Match(imgNode.OuterHtml, pattern);
            
            return @"https://ecoportal.su" + match.Value;

        }

        internal override String GetShortDescription()
        {
            var description = htmlDoc.DocumentNode.SelectSingleNode("//description").InnerHtml;
            Console.WriteLine(description);
            return description;
        }

        internal override string GetFullTextDescription()
        {
            var Text = htmlDoc.DocumentNode.SelectSingleNode("//text");
            
            return Text.InnerHtml;
        }
    }
}
