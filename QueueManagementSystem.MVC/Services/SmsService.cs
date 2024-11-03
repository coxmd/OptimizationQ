
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models;
using System.Text;

namespace QueueManagementSystem.MVC.Services
{
    public class SmsService : ISmsService
    {
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public SmsService(IDbContextFactory<QueueManagementSystemContext> dbFactory, HttpClient httpClient, IConfiguration configuration)
        {

            _httpClient = httpClient;
            _configuration= configuration;
            _dbFactory = dbFactory;
        }

        public async Task<bool> SendSmsAsync(string to, string message)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var smsSettings = await context.SmsSettings.FirstOrDefaultAsync();

            // Prepare request headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", smsSettings!.Url);

             var smsPayload = new
            {
                senderID = smsSettings!.Name,
                message = message,
                phones = to 
            };

            Sms smscontent = new Sms();
            smscontent.MobileNo = to;
            smscontent.Message = message;


            // Convert the payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(smsPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await _httpClient.PostAsync(smsSettings.Url, content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                // You can parse the responseBody if needed, or return a success message
                //return Ok(responseBody);
                smscontent.SentStatus = true;
                context.SmsMessages.Add(smscontent);
                context.SaveChanges();
                return true;
            }
            else
            {
                smscontent.SentStatus = false;
                context.SmsMessages.Add(smscontent);
                context.SaveChanges();
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"SMS sending failed: {error}");
                return false;
            }
        }

 
    }
    
}
