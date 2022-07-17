using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeagueActivityBot.Calendar.Integration.Models;
using LeagueActivityBot.Exceptions;
using Newtonsoft.Json;

namespace LeagueActivityBot.Calendar.Integration
{
    public class CalendarHttpClient
    {
        private readonly HttpClient _httpClient;

        public CalendarHttpClient(HttpClient httpClient, CalendarClientOptions setting)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-token", setting.ApiKey);
        }

        public async Task AddEvent(IEnumerable<AddEventRequestModel> events)
        {
            var response = await _httpClient.PostAsync($"/events", GetContent(events));
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return;

            throw new ClientException(
                $"Добавление события. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
        
        private HttpContent GetContent(object obj)
        {
            var content = JsonConvert.SerializeObject(obj);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}