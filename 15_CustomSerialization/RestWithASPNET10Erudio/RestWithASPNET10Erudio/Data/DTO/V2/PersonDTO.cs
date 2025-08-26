using RestWithASPNET10Erudio.JsonSerializers;
using System.Text.Json.Serialization;

namespace RestWithASPNET10Erudio.Data.DTO.V2
{
    public class PersonDTO
    {

        //[JsonPropertyOrder(3)]
        //[JsonPropertyName("code")]
        public long Id { get; set; }

        //[JsonPropertyOrder(4)]
        //[JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        //[JsonPropertyOrder(5)]
        //[JsonPropertyName("last_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LastName { get; set; }

        //[JsonPropertyOrder(1)]
        public string Address { get; set; }


        //[JsonPropertyOrder(6)]
        //[JsonConverter(typeof(GenderSerializer))]
        public string Gender { get; set; }


        //[JsonPropertyOrder(2)]
        //[JsonConverter(typeof(DateSerializer))]
        //[JsonIgnore]
        public DateTime? BirthDay { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Age { get; set; }

        [JsonIgnore]
        public bool IsAdult => Age >= 18;

        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
