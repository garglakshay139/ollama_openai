using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ollama_openai
{
    internal class openai_prompt
    {
        public List<ChatMessage> GetPrompt(string systemcontent, string usercontent)
        {
            List<ChatMessage> messages = new List<ChatMessage>
                    {
                        new ChatMessage { role = "system", content = systemcontent },
                        new ChatMessage { role = "user", content = usercontent }
                    };


            return messages;
        }

       
    }
    public class ChatMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
