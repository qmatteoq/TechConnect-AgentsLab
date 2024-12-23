using System.Text.Json.Serialization;

namespace TravelAgency.Shared.Requests
{
    public class DestinationRequest
    {
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
    }
}
