using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using CarServ.service.Services.Configuration;
using CarServ.Service.Services.Interfaces;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using System.ClientModel;
using Microsoft.AspNetCore.Mvc;
using CarServ.Repository.Repositories.Interfaces;

namespace CarServ.Service.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly AzureOpenAiSetting _settings;
        private readonly IAppointmentRepository _appointmentRepository;

        public ChatbotService(HttpClient httpClient, IOptions<AzureOpenAiSetting> options, IAppointmentRepository appointmentRepository)
        {
            _settings = options.Value;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<string> GetChatbotResponseAsync(string userInput)
        {
            if (userInput.Contains("appointment details for appointment ID", StringComparison.OrdinalIgnoreCase) || userInput.Contains("cho tôi thông tin về cuộc hẹn ID", StringComparison.OrdinalIgnoreCase))
            {
                int appointmentId = ExtractAppointmentId(userInput);
                if (appointmentId != -1)
                {
                    var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
                    if (appointment != null)
                    {
                        return $"Appointment ID: {appointment.AppointmentId}, Date: {appointment.AppointmentDate}, Status: {appointment.Status}";
                    }
                    else
                    {
                        return "I'm sorry, I couldn't find any details for that appointment ID.";
                    }
                }
                else
                {
                    return "Please provide a valid appointment ID.";
                }
            }

            AzureOpenAIClient client = new AzureOpenAIClient(new Uri(_settings.Endpoint), new AzureKeyCredential(_settings.ApiKey));
            ChatClient chatClient = client.GetChatClient(_settings.DeploymentName);

            ChatCompletion completion = chatClient.CompleteChat(
                [
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(userInput),
                ]);

            return completion.Content[0].Text;
        }

        private int ExtractAppointmentId(string input)
        {
            return int.TryParse(input.Split(' ').Last(), out var id) ? id : 0;
        }
    }
}
