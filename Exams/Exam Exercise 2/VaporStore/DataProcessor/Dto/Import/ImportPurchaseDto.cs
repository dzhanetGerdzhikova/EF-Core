using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
   public class ImportPurchaseDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement]
        [Required]
        public string Type { get; set; }

        [XmlElement]
        [RegularExpression(@"[A-Z\d]{4}-[A-Z\d]{4}-[A-Z\d]{4}")]
        public string Key { get; set; }

        [XmlElement]
        [Required]
        public string Card { get; set; }

        [XmlElement]
        [Required]
        public string Date { get; set; }
    }
}
