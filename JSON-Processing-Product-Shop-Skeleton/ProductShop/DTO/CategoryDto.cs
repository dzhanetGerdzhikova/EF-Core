using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace ProductShop.DTO
{
    public class CategoryDto
    {
        [JsonProperty("category")]
        public string Name { get; set; }
        [JsonProperty("productCount")]
        public int CountProduct { get; set; }
        [JsonProperty("averagePrice")]
        public string AvrPrice { get; set; }
        [JsonProperty("totalRevenue")]
        public string TotalSum { get; set; }
    }
}
