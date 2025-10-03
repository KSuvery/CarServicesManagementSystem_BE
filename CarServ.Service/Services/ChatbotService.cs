using Azure;
using Azure.AI.OpenAI;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.service.Services.Configuration;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.RegularExpressions;

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
            try
            {
                if (IsAppointmentQuery(userInput))
                {
                    int appointmentId = ExtractAppointmentId(userInput);
                    if (appointmentId <= 0)
                    {
                        throw new ArgumentException("ID cuộc hẹn không khả dụng.");
                    }

                    var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
                    if (appointment != null)
                    {
                        return $"ID: {appointment.AppointmentId}, Thời gian: {appointment.AppointmentDate}, Trạng thái: {appointment.Status}";
                    }
                    else
                    {
                        throw new KeyNotFoundException("Không có cuộc hẹn nào tồn tại với ID đó.");
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
            catch (ArgumentException ex)
            {
                return ex.Message;
            }
            catch (KeyNotFoundException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return "An unexpected error occurred. Please try again later.";
            }
        }

        private int ExtractAppointmentId(string input)
        {
            return int.TryParse(input.Split(' ').Last(), out var id) ? id : 0;
        }

        private bool IsAppointmentQuery(string userInput)
        {
            string[] appointmentKeywords = [
                "appointment details", "details for appointment", "show appointment",
        "get appointment info", "cuộc hẹn", "thông tin cuộc hẹn", "lịch hẹn",
        "appointment information", "info about appointment", "appointment ID"
            ];
            return appointmentKeywords.Any(k => userInput.Contains(k, StringComparison.OrdinalIgnoreCase))
                || Regex.IsMatch(userInput, @"(appointment|cuộc hẹn|lịch hẹn).*(details|info|information|thông tin)", RegexOptions.IgnoreCase);
        }
    }
}
