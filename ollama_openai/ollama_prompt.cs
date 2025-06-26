using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ollama_openai
{
    internal class ollama_prompt
    {
        public List<ChatMessage> GetPrompt(string usercontent)
        {
            List<ChatMessage> messages = new List<ChatMessage>
                    {
                        new ChatMessage { role = "user", content = usercontent }
                    };


            return messages;
        }
    }
    
}
