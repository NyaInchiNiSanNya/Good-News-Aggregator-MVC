﻿using HtmlAgilityPack;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Article.WebParsers
{
    internal class OnlinerParser: AbstractParser
    {
        private readonly HtmlDocument? _htmlDoc;

        public OnlinerParser(String html)
        {
            HtmlWeb web = new HtmlWeb();
            
            _htmlDoc = web.Load(html);
        }
        internal override String GetArticleSourceReference(String id)
        {
            return id;

        }

        internal override List<string> GetArticleTagsFromRss(List<string> categories)
        {
            List<String> tags = new List<string>();
            foreach (string category in categories)
            {
                switch (category)
                {
                    case "Технологии":
                        tags.Add("позновательное");
                        break;
                    case "Авто":
                        tags.Add("авто");
                        break;
                    case "Лайфстайл":
                        tags.Add("лайфстайл");
                        break;
                    case "Кошелек":
                        tags.Add("лайфстайл");
                        break;
                }
            }
            return tags;
        }

        internal override String GetPictureReference()
        {
            String pattern = @"background-image:\s*url\(['""]?(?<url>.*?)['""]?\);";

            Match match = Regex.Match(_htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'news-header__image']").Attributes["style"].Value, pattern);

            return match.Success ? match.Groups["url"].Value : @"https://mobimg.b-cdn.net/v3/fetch/e4/e47497aa7aadc5a81cd0694b1e65bdfb.jpeg";

        }

        internal override String GetShortDescription()
        {
            String shortDescription = _htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'news-text']/p").InnerHtml;

            String plainText = Regex.Replace(shortDescription, "<.*?>", string.Empty);
            
            plainText = Regex.Replace(plainText, @"<.*?>|https?://\S+|www\.\S+", string.Empty);
            
            if (plainText.Length > 300)
            {
                plainText = plainText.Substring(0, 300) + "...";
            }

            return shortDescription;
        }

        internal override String GetFullTextDescription()
        {
            var text = _htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'news-text']");
            var nodesToRemove = text.SelectNodes(@"//div[@class = 'news-incut news-incut_extended news-incut_position_right news-incut_shift_top news-helpers_hide_tablet']|
											 //div[@class = 'news-reference']|
                                             //div[@id = 'news-text-end']|
											 //div[@class = 'news-header news-header_extended ']|
                                             //div[@class = 'news-header news-header_extended']|
											 //p[@style = 'text-align: right;']|
											 //div[@class = 'news-widget']|
											 //div[@class = 'news-header news-header_extended news-helpers_show_tablet']|
											 //div[@class = 'news-banner news-banner_condensed news-helpers_show_tablet']|
                                             //div[@class = 'news-widget news-widget_special']|
											 //div[@class = 'news-media news-media_extended-condensed news-media_3by2 news-media_centering']|
                                             //div[@class = 'news-promo']
											 ");
            
            foreach (var nodes in nodesToRemove)
            {
                nodes.Remove();
            }

            var hrNode = text.SelectSingleNode("//hr");

            if (hrNode != null)
            {
                HtmlNode nextNode = hrNode.NextSibling;
                while (nextNode != null)
                {
                    HtmlNode currentNode = nextNode;
                    nextNode = currentNode.NextSibling;
                    currentNode.Remove();
                }
            }

            var ResultText = Regex
                .Replace(text.InnerHtml, @"<script\b[^>]*>(.*?)</script>", "",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);

                
                return ResultText;
        }
    }
}
