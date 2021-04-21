using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProductShop.DTO
{
   public class SellerDto
    {
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("age")]
        public int? Age { get; set; }
        [JsonProperty("soldProducts")]
        public SoldProductOfSellerDto SoldProducts { get; set; }
    }
}
