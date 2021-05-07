﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
  public   class ImportTaskDto
    {
        [XmlElement]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [XmlElement]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement]
        [Required]
        public string DueDate { get; set; }

        [XmlElement]
        [Required]
        public string ExecutionType { get; set; }

        [XmlElement]
        [Required]
        public string LabelType { get; set; }
    }
}
