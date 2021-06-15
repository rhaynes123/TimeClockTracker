using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace TimeClockTracker.Models
{
    public class TimePunch
    {
        [Required, Key, JsonPropertyName("id")]
        public int Id { get; set; }
        [Required,JsonPropertyName("userId")]
        public string UserId { get; set; }
        [Required,JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("clockIn")]
        public DateTime ClockIn { get; set; }
        [JsonPropertyName("clockOut")]
        public DateTime? ClockOut { get; set; }
        [Required,JsonPropertyName("lastPunch")]
        public DateTime LastPunch { get; set; }

    }
}
