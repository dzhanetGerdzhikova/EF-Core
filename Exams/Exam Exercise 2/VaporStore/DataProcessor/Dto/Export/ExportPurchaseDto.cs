using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class ExportPurchaseDto
    {
        [XmlElement]
        public string Card { get; set; }

        [XmlElement]
        public string Cvc { get; set; }

        [XmlElement]
        public string Date { get; set; }

        [XmlElement]
        public ExportPurchGameDto Game { get; set; }

    }
}
