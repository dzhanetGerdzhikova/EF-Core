using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerPrisonerDto
    {
        [XmlElement]
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [XmlElement]
        [Required]
        [Range(typeof(decimal),"0", "79228162514264337593543950335")]
        public decimal Money { get; set; }

        [XmlElement]
        [Required]
        public string Position { get; set; }

        [XmlElement]
        [Required]
        public string Weapon { get; set; }

        [XmlElement]
        [Required]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportPrisonerDto[] Prisoners { get; set; }
    }
}
