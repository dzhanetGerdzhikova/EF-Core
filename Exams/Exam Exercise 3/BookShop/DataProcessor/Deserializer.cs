namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ImportBookDto>), new XmlRootAttribute("Books"));

            using (StringReader reader = new StringReader(xmlString))
            {
                List<ImportBookDto> booksDto = (List<ImportBookDto>)xmlSerializer.Deserialize(reader);

                List<Book> validBooks = new List<Book>();

                foreach (var bookDto in booksDto)
                {
                    if (!IsValid(bookDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isValidDate = DateTime.TryParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultFromParse);

                    if (!isValidDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validBooks.Add(new Book
                    {
                        Name = bookDto.Name,
                        Genre = (Genre)bookDto.Genre,
                        Pages = bookDto.Pages,
                        Price = bookDto.Price,
                        PublishedOn = resultFromParse
                    });

                    sb.AppendLine(string.Format(SuccessfullyImportedBook, bookDto.Name, bookDto.Price));
                }
                context.Books.AddRange(validBooks);
                context.SaveChanges();
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            List<ImportAuthorDto> authorBookDtos = JsonConvert.DeserializeObject<List<ImportAuthorDto>>(jsonString);

            foreach (var authorDto in authorBookDtos)
            {
                if (!IsValid(authorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var countAuthors = context.Authors.Where(a => a.Email == authorDto.Email).Count();

                if (countAuthors > 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Author author = new Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Email = authorDto.Email,
                    Phone = authorDto.Phone,
                    AuthorsBooks = new List<AuthorBook>()
                };

                foreach (var bookDto in authorDto.Books) 
                {
                    Book book = context.Books.FirstOrDefault(b => b.Id == bookDto.BookId);
                    if (book != null)
                    {
                        author.AuthorsBooks.Add(new AuthorBook()
                        {
                            Author = author,
                            Book = book
                        });
                    }
                }
                
                if(author.AuthorsBooks.Count()==0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                context.Authors.Add(author);
                context.SaveChanges();
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, author.FirstName + ' ' + author.LastName, author.AuthorsBooks.Count()));
            }

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