using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProductShop.DTO
{
   public  class SoldProductOfSellerDto
    {
        [JsonProperty("count")]
        public int CountSoldProducts { get; set;}

        [JsonProperty("products")]
        public ProductOfSellerDto[] Products { get; set; }
    }
}
