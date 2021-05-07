namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors.Select(a => new
            {
                AuthorName = a.FirstName + ' ' + a.LastName,
                Books = a.AuthorsBooks.OrderByDescending(b => b.Book.Price).Select(ab => new
                {
                    BookName = ab.Book.Name,
                    BookPrice = ab.Book.Price.ToString("f2")
                })
            })
                 .OrderByDescending(a => a.Books.Count())
                 .ThenBy(a => a.AuthorName);

            string json = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var books = context.Books
                 .Where(b => b.PublishedOn < date && b.Genre == Data.Models.Enums.Genre.Science)
                 .OrderByDescending(b => b.Pages)
                 .ThenByDescending(b => b.PublishedOn)
                 .Select(b => new ExportBookDto
                 {
                     Pages = b.Pages,
                     Name = b.Name,
                     Date = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture)
                 })
                 .Take(10)
                 .ToList();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ExportBookDto>), new XmlRootAttribute("Books"));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", ""); 

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter,books,ns);
                return textWriter.ToString();
            }
        }
    }
}