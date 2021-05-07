using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProjectWithTheirTasksDto
    {
        [XmlAttribute]
        public int TasksCount { get; set; }

        [XmlElement]
        public string ProjectName { get; set; }

        [XmlElement]
        public string HasEndDate { get; set; }

        [XmlArray]
        public ExportTaskDto[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class ExportTaskDto
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Label { get; set; }

    }
}
