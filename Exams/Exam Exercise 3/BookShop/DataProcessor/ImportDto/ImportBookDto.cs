using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class ImportBookDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [XmlElement]
        public string Name { get; set; }

        [Range(1, 3)]
        [XmlElement]
        public int Genre { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement]
        public decimal Price { get; set; }

        [Range(50, 5000)]
        [XmlElement]
        public int Pages { get; set; }

        [Required]
        [XmlElement]
        public string PublishedOn { get; set; }
    }
}