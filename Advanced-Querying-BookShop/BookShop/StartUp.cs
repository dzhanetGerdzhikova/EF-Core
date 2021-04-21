namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            //Problem 2
            //string command = Console.ReadLine();

            //string result = GetBooksByAgeRestriction(db,command);
            //Console.WriteLine(result);

            //Problem 3
            //Console.WriteLine(GetGoldenBooks(db));

            //Problem 4
            //Console.WriteLine(GetBooksByPrice(db));

            //Problem 5
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));

            //Problem 6
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db,input));

            //Problem 7
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, date));

            //Problem 8
            //string letters = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db,letters));

            //Problem 9
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db,input));

            //Problem 10
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db,input));

            //Problem 11
            //int lenght =int.Parse( Console.ReadLine());
            //int count = CountBooks(db, lenght);
            //Console.WriteLine($"There are {count} books with longer title than {lenght} symbols");

            //Problem 12
            //Console.WriteLine(CountCopiesByAuthor(db));

            //Problem 13
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //Problem 14
            //Console.WriteLine(GetMostRecentBooks(db));

            //Problem 15
            //IncreasePrices(db);

            //Problem 16
            //Console.WriteLine(RemoveBooks(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var titles = context.Books
                .AsEnumerable()
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var titles = context.Books
                .AsEnumerable()
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .Select
                (b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var titles = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var splitedInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToLower()).ToArray();

            List<string> bookTitles = new List<string>();

            foreach (var currentInput in splitedInput)
            {
                var booksTitles = context.Books
                            .Where(b => b.BookCategories
                            .Any(c => c.Category.Name.ToLower() == currentInput))
                            .Select(b => b.Title)
                            .ToList();

                foreach (var bookTitle in booksTitles)
                {
                    bookTitles.Add(bookTitle);
                }
            }

            bookTitles = bookTitles.OrderBy(b => b).ToList();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            var inputDate = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b => b.ReleaseDate < inputDate)
                .Select(b => new
                {
                    b.Title,
                    b.ReleaseDate,
                    b.Price
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.ReleaseDate} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new { a.FirstName, a.LastName })
                .OrderBy(a => a.FirstName)
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var titles = context.Books
                .AsEnumerable()
                .Where(b => b.Title.Contains(input, StringComparison.CurrentCultureIgnoreCase))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    Title = b.Title,
                    Author = b.Author.FirstName + " " + b.Author.LastName
                })
                .ToList();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} ({b.Author})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return books;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var authorsBooksCopies = context.Authors
                                      .Select(a => new
                                      {
                                          Name = a.FirstName + ' ' + a.LastName,
                                          BooksCopy = a.Books.Select(b => b.Copies).Sum()
                                      })
                                      .OrderByDescending(b => b.BooksCopy)
                                      .ToList();

            authorsBooksCopies.ForEach(abc => sb.AppendLine($"{abc.Name} - {abc.BooksCopy}"));

            return sb.ToString();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categoryProfits = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks.Select(cb => cb.Book.Copies * cb.Book.Price).Sum()
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.Name)
                .ToList();

            categoryProfits.ForEach(cp => sb.AppendLine($"{cp.Name} ${cp.Profit:f2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var category = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks.Select(cb => new
                    {
                        cb.Book.Title,
                        ReleaseDate = cb.Book.ReleaseDate
                    })
                    .OrderByDescending(cb=>cb.ReleaseDate)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(c=>c.Name)
                .ToList();

            foreach (var cat in category)
            {
                sb.AppendLine($"--{cat.Name}");

                foreach (var book in cat.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200);

            context.RemoveRange(books);

            return books.Count();
        }
    }
}
