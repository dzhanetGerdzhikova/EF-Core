﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("User")]
   public  class ExportUserDto
    {
        [XmlAttribute("username")]
        public string  Username { get; set; }

        [XmlArray("Purchases")]
        public ExportPurchaseDto[] Purchases { get; set; }

        [XmlElement]
        public decimal TotalSpent => Purchases.Sum(x => x.Game.Price);
    }
}
