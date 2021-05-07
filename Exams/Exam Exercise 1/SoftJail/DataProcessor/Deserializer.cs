namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            List<ImportDepartmentCellDto> departmentsCellsDto = JsonConvert.DeserializeObject<List<ImportDepartmentCellDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Department> departments = new List<Department>();

            foreach (var dsDto in departmentsCellsDto)
            {
                if (!IsValid(dsDto) ||
                    !dsDto.Cells.Any() ||
                     dsDto.Cells.Any(c => !IsValid(c)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department
                {
                    Name = dsDto.Name,
                    Cells = dsDto.Cells.Select(x => new Cell
                    {
                        CellNumber = x.CellNumber,
                        HasWindow = x.HasWindow
                    }).ToList()
                };

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            List<ImportPrisonerMailDto> prisonMailsDto = JsonConvert.DeserializeObject<List<ImportPrisonerMailDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Prisoner> prisoners = new List<Prisoner>();

            foreach (var prisonDto in prisonMailsDto)
            {
                if (!IsValid(prisonDto) ||
                    prisonDto.Mails.Any(m => !IsValid(m)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                bool isValidDate = DateTime.TryParse(prisonDto.ReleaseDate, out DateTime resultDate);
                Prisoner prisoner = new Prisoner
                {
                    FullName = prisonDto.FullName,
                    Nickname = prisonDto.Nickname,
                    Age = prisonDto.Age,
                    IncarcerationDate = DateTime.ParseExact(prisonDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    ReleaseDate = isValidDate ? (DateTime?)resultDate : null,
                    Bail = prisonDto.Bail,
                    CellId = prisonDto.CellId,
                    Mails = prisonDto.Mails.Select(m => new Mail
                    {
                        Description = m.Description,
                        Sender = m.Sender,
                        Address = m.Address
                    }).ToList()
                };

                prisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }
            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer xmlSerelializer = new XmlSerializer(typeof(List<ImportOfficerPrisonerDto>), new XmlRootAttribute("Officers"));

            StringBuilder sb = new StringBuilder();
            List<Officer> officers = new List<Officer>();

            using (StringReader reader = new StringReader(xmlString))
            {
                List<ImportOfficerPrisonerDto> officerPrisonersDto = (List<ImportOfficerPrisonerDto>)xmlSerelializer.Deserialize(reader);

                foreach (var officerDto in officerPrisonersDto)
                {
                    bool isValidPosition = Enum.TryParse<Position>(officerDto.Position, out Position resultPosition);
                    bool isValidWeapon = Enum.TryParse<Weapon>(officerDto.Weapon, out Weapon resultWeapon); 

                    if (!IsValid(officerDto) ||
                        !isValidPosition ||
                        !isValidWeapon)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    
                    Officer officer = new Officer
                    {
                        FullName = officerDto.Name,
                        Salary = officerDto.Money,
                        Position = resultPosition,
                        Weapon = resultWeapon,
                        DepartmentId = officerDto.DepartmentId,
                        OfficersPrisoners = officerDto.Prisoners.Select(x => new OfficerPrisoner
                        {
                            PrisonerId = x.Id
                        }).ToArray()
                    };

                    sb.AppendLine($"Imported {officer.FullName} ({officer.OfficersPrisoners.Count} prisoners)");

                    officers.Add(officer);
                }
            }
            context.Officers.AddRange(officers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}