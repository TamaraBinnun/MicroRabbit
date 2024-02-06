using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MicroRabbit.Domain.Core.Models
{
    public class UpdateBaseRequest
    {
        [JsonIgnore]
        public DateTime LastUpdatedDate { get; set; }
    }
}