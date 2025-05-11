using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookerApiTest.Models
{
    public class BookingDates
    {
        [JsonProperty("checkin")]
        public string CheckIn { get; set; }

        [JsonProperty("checkout")]
        public string CheckOut { get; set; }
    }
}


