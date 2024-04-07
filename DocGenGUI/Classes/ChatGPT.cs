using DocGen.Models;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocGen.Classes
{
    internal class ChatGPT
    {
        private static OpenAI_API.OpenAIAPI api = new OpenAI_API.OpenAIAPI(
            Environment.GetEnvironmentVariable("OPEN_AI_KEY")
        );
        
        public async static Task<string> getSummary(FileModel fileContents)
        {
            var chat = api.Chat.CreateConversation();
            chat.Model = Model.ChatGPTTurbo;
            chat.AppendSystemMessage(fileContents.FileContents + "\n\nTell me what the code is doing.");

            return await chat.GetResponseFromChatbotAsync();
        }
    }
}
