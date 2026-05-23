using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClientManager.Api.Application.DTO
{
    public class ResponseBadRequestDto
    {
        public ResponseBadRequestDto()
        {
            this.Errors = new Dictionary<string, string[]>();
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        [JsonProperty("errors")]
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
