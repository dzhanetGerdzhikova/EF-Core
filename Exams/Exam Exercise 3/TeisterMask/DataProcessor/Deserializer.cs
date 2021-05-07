namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ImportProjectDto>), new XmlRootAttribute("Projects"));

            StringBuilder sb = new StringBuilder();

            using (StringReader reader = new StringReader(xmlString))
            {
                List<ImportProjectDto> projectsDto = (List<ImportProjectDto>)xmlSerializer.Deserialize(reader);

                List<Project> projectsInDb = new List<Project>();

                foreach (var projectDto in projectsDto)
                {

                    if (!IsValid(projectDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime parsedProjOpendate = DateTime.ParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    bool isParsedProjDueDate = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedProjDueDate);

                    Project project = new Project
                    {
                        Name = projectDto.Name,
                        OpenDate = parsedProjOpendate,
                        DueDate = isParsedProjDueDate ? (DateTime?)parsedProjDueDate : null
                    };

                    foreach (var tastDto in projectDto.Tasks)
                    {
                        if (!IsValid(tastDto))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        DateTime parsedTaskOpendate = DateTime.ParseExact(tastDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        DateTime parsedTaskDueDate = DateTime.ParseExact(tastDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        if (parsedTaskOpendate < parsedProjOpendate
                            || (project.DueDate.HasValue && parsedTaskDueDate > parsedProjDueDate))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        bool IsParsedExecutionType = Enum.TryParse<ExecutionType>(tastDto.ExecutionType, out ExecutionType parsedExecutionType);

                        bool IsParsedLabelType = Enum.TryParse<LabelType>(tastDto.LabelType, out LabelType parsedLabelType);

                        if (!IsParsedExecutionType || !IsParsedLabelType)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        Task task = new Task
                        {
                            Name = tastDto.Name,
                            OpenDate = parsedTaskOpendate,
                            DueDate = parsedTaskDueDate,
                            ExecutionType = parsedExecutionType,
                            LabelType = parsedLabelType
                        };

                        project.Tasks.Add(task);
                    };

                    sb.AppendLine($"Successfully imported project - {project.Name} with {project.Tasks.Count} tasks.");

                    projectsInDb.Add(project);
                }

                context.Projects.AddRange(projectsInDb);
                context.SaveChanges();

                return sb.ToString().TrimEnd();
            }
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            List<ImportEmployeeDto> employeeDto = JsonConvert.DeserializeObject<List<ImportEmployeeDto>>(jsonString);

            List<Employee> employeeInDb = new List<Employee>();

            StringBuilder sb = new StringBuilder();

            foreach (var emplDto in employeeDto)
            {
                if (!IsValid(emplDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee
                {
                    Username = emplDto.Username,
                    Email = emplDto.Email,
                    Phone = emplDto.Phone,
                };

                foreach (var task in emplDto.Tasks.Distinct())
                {
                    if (!context.Tasks.Any(t => t.Id == task))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task taskInDb = context.Tasks.FirstOrDefault(x => x.Id == task);

                    employee.EmployeesTasks.Add(new EmployeeTask
                    {
                        Employee = employee,
                        Task = taskInDb
                    });

                }

                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));

                employeeInDb.Add(employee);
            }

            context.Employees.AddRange(employeeInDb);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}