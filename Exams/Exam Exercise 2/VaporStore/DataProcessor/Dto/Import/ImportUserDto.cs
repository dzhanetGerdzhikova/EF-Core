using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Dto.Import
{
   public  class ImportUserDto
	{
  //      "FullName": "",
		//"Username": "invalid",
		//"Email": "invalid@invalid.com",
		//"Age": 20,
		//"Cards": [
		//	{
		//		"Number": "1111 1111 1111 1111",
		//		"CVC": "111",
		//		"Type": "Debit"
		//	}
		//]

		[Required]
		[RegularExpression(@"([A-Z]+[a-z]+)\s([A-Z]+[a-z]+)")]
        public string FullName { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(20)]
        public string  Username { get; set; }

		[Required]
        public string Email { get; set; }

		[Required]
		[Range(3,103)]
        public int Age { get; set; }
        public ImportCardDto[] Cards { get; set; }
    }
}
