using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class ExportUsersDto
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlArray("users")]
        public ExportUsersWithSoldProductsDto[] Users { get; set; }
    }
    [XmlType("User")]
    public class ExportUsersWithSoldProductsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }
        [XmlElement("lastName")]
        public string LastName { get; set; }
        [XmlElement("age")]
        public int? Age { get; set; }
        [XmlElement("SoldProducts")]
        public ExportSoldProduct SoldProduct { get; set; }

    }

    [XmlType("SoldProducts")]
    public class ExportSoldProduct
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ExportProducts[] SoldProducts { get; set; }
    }

    [XmlType("Product")]
    public class ExportProducts
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }
    }

}
