using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
namespace CarDealer.Dto.Import
{
    [XmlType("Car")]
   public  class CarImportDto
    {
        [XmlElement("make")]
        public string   Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public int TraveledDistance { get; set; }

        [XmlArray("parts")]
        public PartIdImportDto[] PartsId { get; set; }
    }
    [XmlType("partId")]
    public class PartIdImportDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
