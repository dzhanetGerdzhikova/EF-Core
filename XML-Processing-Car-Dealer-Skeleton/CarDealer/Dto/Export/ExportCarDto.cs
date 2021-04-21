using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dto.Export
{
    [XmlType("Car")]
   public class ExportCarDto
    {
        [XmlElement("make")]
        public string Make { get; set; }
         [XmlElement("model")]
        public string Model { get; set; }
        [XmlElement("travelled-distance")]
        public long TravelledDistance { get; set; }

    }
}
