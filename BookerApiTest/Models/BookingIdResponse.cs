using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookerApiTest.Models
{
    public class BookingIdResponse
    {
        [JsonProperty("bookingid")]
        public int BookingId { get; set; }
    }
}
