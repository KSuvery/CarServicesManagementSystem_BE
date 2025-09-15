using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using CarServ.service.Services.Configuration;
using CarServ.Service.Services.Interfaces;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using System.ClientModel;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.Service.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly AzureOpenAiSetting _settings;

        public ChatbotService(HttpClient httpClient, IOptions<AzureOpenAiSetting> options)
        {
            _settings = options.Value;
        }

        public async Task<string> GetChatbotResponseAsync(string userInput)
        {
            AzureOpenAIClient client = new AzureOpenAIClient(new Uri(_settings.Endpoint), new AzureKeyCredential(_settings.ApiKey));
            ChatClient chatClient = client.GetChatClient(_settings.DeploymentName);

            ChatCompletion completion = chatClient.CompleteChat(
                [
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(userInput),
                ]);

            return completion.Content[0].Text;
        }
    }
}
