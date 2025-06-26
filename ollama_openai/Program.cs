using Ollama;
using ollama_openai;
using ollama_openai.Utilities;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Markdig;
using System.Transactions;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;
using System.Net;
using System.Net.Http;


string str = HtmlAgilityPackHelper.GetScrappedWebsiteText("https://www.screener.in/company/TCS/consolidated/");

Console.Write("Enter 1 for OpenAI response and Enter 2 for response from Ollama: ");
string? input = Console.ReadLine();

if (input == "1")
{

    Console.WriteLine("You selected OpenAI.");
    var openai_obj = new openai_prompt();
    var openai_prompt = openai_obj.GetPrompt("You are financial assistant that analyses the content of a financial website and provide the trend of the particular company",
        $"The content of the website is as follows {str}. Give summary of latest aquisitions and mergers in TCS in 150 words");

    string apiKey = "sk-proj-api-key"; // 🔐 Replace with your OpenAI API key
    string endpoint = "https://api.openai.com/v1/chat/completions";

    var requestBody = new
    {
        model = "gpt-4o-mini",
        messages = openai_prompt,
        temperature = 0.0
    };

    string json = JsonSerializer.Serialize(requestBody);

    // Step 3: Create HTTP client and request
    var httpclient = new HttpClient();
    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var responseresult = await httpclient.PostAsync(endpoint, content);

    string responseJson = await responseresult.Content.ReadAsStringAsync();

    using JsonDocument doc = JsonDocument.Parse(responseJson);

    string assistantReply = doc?.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

    Console.WriteLine("Assistant said:\n" + assistantReply);
    // Call OpenAI logic here
}
else if (input == "2")
{
    Console.WriteLine("You selected Ollama.");

    string OllamaAPi = "http://localhost:11434/api/chat";
    var Headers = @"{ Content-Type : application/json}";
    var Model = "llama3.2";
    var ollama_obj = new ollama_prompt();
    var ollama_prompt = ollama_obj.GetPrompt($"The content of the website is as follows {str}. Give summary of latest aquisitions and mergers in TCS in 150 words");
    var payload = new
    {
        model = Model,
        messages = new[]
    {
        new { role = "user", content = "Summarize the text given in double stars giving the latest aquisitions and mergers from the company**" + str.Substring(0,1000) +  "**explain in 150 words" }
    },
        stream = false
    };
    string json = JsonSerializer.Serialize(payload);
    var httpclient = new HttpClient();
    httpclient.Timeout = TimeSpan.FromMinutes(10);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var responseresult = await httpclient.PostAsync(OllamaAPi, content);
    string responseJson = await responseresult.Content.ReadAsStringAsync();

    using JsonDocument doc = JsonDocument.Parse(responseJson);

    string assistantReply = doc.RootElement.GetProperty("message").GetProperty("content").GetString();

    Console.WriteLine("Assistant reply:\n" + assistantReply);

    //string json = JsonSerializer.Serialize(payload);
    // Call Ollama logic here
}
else
{
    Console.WriteLine("Invalid selection.");
}


Console.WriteLine("End");

