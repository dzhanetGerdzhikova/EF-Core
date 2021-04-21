﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dto.Export
{
    [XmlType("suplier ")]
   public class ExportSupplierDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("parts-count")]
        public int PartsCount { get; set; }
    }
}
