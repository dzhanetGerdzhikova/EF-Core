namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners.Where(p => ids.Contains(p.Id))
                  .Select(p => new
                  {
                      Id = p.Id,
                      Name = p.FullName,
                      CellNumber = p.Cell.CellNumber,
                      Officers = p.PrisonerOfficers
                      .Select(ofp => new
                      {
                          OfficerName = ofp.Officer.FullName,
                          Department = ofp.Officer.Department.Name
                      })
                      .OrderBy(of => of.OfficerName)
                      .ToList(),
                      TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(x => x.Officer.Salary), 2)
                  })
                  .OrderBy(p => p.Name)
                  .ThenBy(p => p.Id)
                  .ToList();

            return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            List<string> prisoners = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            List<ExportPrisonersInboxDto> prisonersInDb = context.Prisoners.Where(p => prisoners.Contains(p.FullName))
                .Select(p => new ExportPrisonersInboxDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new ExportMessageDto
                    {
                        Description = new string(m.Description.Reverse().ToArray())
                    }).ToList()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToList();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ExportPrisonersInboxDto>), new XmlRootAttribute("Prisoners"));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using (StringWriter writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, prisonersInDb, ns);
            }

            return sb.ToString().TrimEnd();
        }
    }
}