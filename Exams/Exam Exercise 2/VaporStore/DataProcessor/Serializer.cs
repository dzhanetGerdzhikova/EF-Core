namespace VaporStore.DataProcessor
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
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static object XmlSerialize { get; private set; }

        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            List<ExportGenreDto> genres = context.Genres.ToList()
                .Where(g => genreNames.Contains(g.Name.ToString()))
               .Select(g => new ExportGenreDto
               {
                   Id = g.Id,
                   Genre = g.Name,
                   Games = g.Games.Where(gm => gm.Purchases.Any()).Select(gm => new ExportGameDto
                   {
                       Id = gm.Id,
                       Title = gm.Name,
                       Developer = gm.Developer.Name,
                       Tags = string.Join(", ", gm.GameTags.Select(gt => gt.Tag.Name)),
                       Players = gm.Purchases.Count()
                   })
                   .OrderByDescending(gm => gm.Players)
                   .ThenBy(gm => gm.Id),
                   TotalPlayers = g.Games.Sum(gm => gm.Purchases.Count)
               }).ToList()
               .OrderByDescending(g => g.TotalPlayers)
               .ThenBy(g => g.Id).ToList();

            return JsonConvert.SerializeObject(genres, Formatting.Indented);

        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var users = context.Users.ToList().Where(u => u.Cards.Any(e => e.Purchases.Any())).Select(u => new ExportUserDto
            {
                Username = u.Username,
                Purchases = context.Purchases.ToList().Where(p => p.Type.ToString() == storeType && p.Card.User.Username == u.Username).Select(p => new ExportPurchaseDto
                {
                    Card = p.Card.Number,
                    Cvc = p.Card.Cvc,
                    Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    Game = new ExportPurchGameDto
                    {
                        Genre = p.Game.Genre.Name,
                        Price = p.Game.Price,
                        Title = p.Game.Name
                    }
                }).OrderBy(p => p.Date)
                .ToArray()
                ,
            }).OrderByDescending(x => x.TotalSpent)
            .ThenBy(u => u.Username)
            .ToList();

            XmlSerializer xmlSerialize = new XmlSerializer(typeof(List<ExportUserDto>), new XmlRootAttribute("Users"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using (StringWriter writer= new StringWriter(sb))
            {
                xmlSerialize.Serialize(writer,users,namespaces);
            }
            return sb.ToString().TrimEnd();
        }
    }
}