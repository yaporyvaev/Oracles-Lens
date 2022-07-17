using System;
using Newtonsoft.Json;

namespace LeagueActivityBot.Calendar.Integration.Models
{
    public class AddEventRequestModel
    {
        [JsonProperty(PropertyName = "sourceID")]
        public string EventId { get; set; }
        public string Summary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}