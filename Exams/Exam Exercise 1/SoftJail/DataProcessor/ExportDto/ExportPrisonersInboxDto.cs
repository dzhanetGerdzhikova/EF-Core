using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExportPrisonersInboxDto
    {
        [XmlElement]
        public int Id { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string IncarcerationDate { get; set; }

        [XmlArrayItem("Message")]
        public List<ExportMessageDto> EncryptedMessages { get; set; }
    }
}
