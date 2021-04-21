using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProductShop.DTO
{
   public  class UsersDto
    {
        [JsonProperty("usersCount")]
        public int CountUsers { get; set; }
        [JsonProperty("users")]
        public SellerDto[] Sellers { get; set; }
    }
}
