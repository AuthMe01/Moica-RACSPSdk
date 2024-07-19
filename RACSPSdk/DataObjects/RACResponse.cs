using System;
using System.Text.Json.Serialization;

namespace tw.moica.RACSPSdk.DataObjects
{
    // ReSharper disable once InconsistentNaming
    public class RACResponse
    {
        [JsonPropertyName("error_message")]

        public String ErrorMessage { get; set; }
        [JsonPropertyName("error_code")]

        public String ErrorCode { get; set; }
    }
}