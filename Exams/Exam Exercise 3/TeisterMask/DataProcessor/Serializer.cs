namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projectsInDb = context.Projects.ToArray()
                .OrderByDescending(p => p.Tasks.Count())
                 .ThenBy(p => p.Name)
                .Where(p => p.Tasks.Any())
                 .Select(p => new ExportProjectWithTheirTasksDto
                 {
                     TasksCount=p.Tasks.Count,
                     ProjectName = p.Name,
                     HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                     Tasks = p.Tasks.ToList()
                     .OrderBy(t => t.Name)
                     .Select(t => new ExportTaskDto
                     {
                         Name = t.Name,
                         Label = t.LabelType.ToString()
                     })
                     .ToArray()
                 })
                 .ToList();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ExportProjectWithTheirTasksDto>), new XmlRootAttribute("Projects"));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using(StringWriter writer= new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, projectsInDb, ns);
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var users = context.Employees.ToList().Where(e => e.EmployeesTasks.Any(s => s.Task.OpenDate >= date))
                  .Select(e => new
                  {
                      Username = e.Username,
                      Tasks = e.EmployeesTasks.Where(et => et.Task.OpenDate >= date)
                     .OrderByDescending(x => x.Task.DueDate)
                     .ThenBy(x => x.Task.Name)
                      .Select(et => new
                      {
                          TaskName = et.Task.Name,
                          OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                          DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                          LabelType = et.Task.LabelType.ToString(),
                          ExecutionType = et.Task.ExecutionType.ToString()
                      })
                     .ToList()
                  }).OrderByDescending(x => x.Tasks.Count())
                  .ThenBy(x => x.Username)
                  .Take(10)
                  .ToList();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }
    }
}