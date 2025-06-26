using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.Json;

namespace ollama_openai.Utilities
{
    public static class HtmlAgilityPackHelper
    {
        public static string GetScrappedWebsiteText(string url)
        {
           
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // Extract all links
            doc.DocumentNode.Descendants()
                        .Where(n => n.Name == "script" || n.Name == "style" || n.Name == "img" || n.Name == "input")
                        .ToList()
                        .ForEach(n => n.Remove());
            StringBuilder builder = new StringBuilder();
            var nodes = doc.DocumentNode.SelectNodes("//h1|//h2|//h3|//p|//div|//span");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var text = node.InnerText.Trim();
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        builder.Append(text);
                    }
                }
                return builder.ToString();
            }
            else
            {
                return "No relevant content found.";
            }
        }


        public static string JsonToMarkdown(JsonElement element, int indent = 0)
        {
            var sb = new StringBuilder();
            string indentStr = new string(' ', indent * 2);

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var prop in element.EnumerateObject())
                    {
                        sb.AppendLine($"{indentStr}- **{prop.Name}**:");
                        sb.Append(JsonToMarkdown(prop.Value, indent + 1));
                    }
                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                    {
                        sb.AppendLine($"{indentStr}-");
                        sb.Append(JsonToMarkdown(item, indent + 1));
                    }
                    break;

                case JsonValueKind.String:
                    sb.AppendLine($"{indentStr}  `{element.GetString()}`");
                    break;

                case JsonValueKind.Number:
                    sb.AppendLine($"{indentStr}  `{element.GetRawText()}`");
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    sb.AppendLine($"{indentStr}  `{element.GetBoolean()}`");
                    break;

                case JsonValueKind.Null:
                    sb.AppendLine($"{indentStr}  `null`");
                    break;
            }

            return sb.ToString();
        }
    }
}