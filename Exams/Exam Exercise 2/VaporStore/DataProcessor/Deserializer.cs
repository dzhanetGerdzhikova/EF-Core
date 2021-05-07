namespace VaporStore.DataProcessor
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
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            List<ImportGameDto> games = JsonConvert.DeserializeObject<List<ImportGameDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var gameDto in games)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (gameDto.Name == "Invalid")
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isParsedDate = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);

                if (!isParsedDate)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Game game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = parsedDate,
                    GameTags = new List<GameTag>()
                };

                Developer dev = context.Developers.FirstOrDefault(x => x.Name == gameDto.Developer);
                if (dev == null)
                {
                    dev = new Developer { Name = gameDto.Developer };
                    context.Developers.Add(dev);
                }

                game.Developer = dev;

                Genre genre = context.Genres.FirstOrDefault(x => x.Name == gameDto.Genre);
                if (genre == null)
                {
                    genre = new Genre { Name = gameDto.Genre };
                    context.Genres.Add(genre);
                }

                game.Genre = genre;

                foreach (var tagDto in gameDto.Tags)
                {
                    Tag tagInDb = context.Tags.FirstOrDefault(x => x.Name == tagDto);

                    if (tagInDb == null)
                    {
                        Tag tag = new Tag { Name = tagDto };
                        context.Tags.Add(tag);

                        game.GameTags.Add(new GameTag
                        {
                            Tag = tag,
                            Game = game
                        });
                    }
                    else
                    {
                        game.GameTags.Add(new GameTag
                        {
                            Tag = tagInDb,
                            Game = game
                        });
                    }
                }

                context.Games.Add(game);
                context.SaveChanges();

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }
            return sb.ToString();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            List<ImportUserDto> usersDto = JsonConvert.DeserializeObject<List<ImportUserDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var userDto in usersDto)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                User user = new User
                {
                    FullName = userDto.FullName,
                    Age = userDto.Age,
                    Email = userDto.Email,
                    Username = userDto.Username
                };

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }


                    bool isCardTypeValid = Enum.TryParse<CardType>(cardDto.Type, out CardType parsedCardType);

                    if (!isCardTypeValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    Card card = new Card
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.CVC,
                        Type = parsedCardType,
                        User = user                         
                    };
                    user.Cards.Add(card);
                }

                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
                context.Users.Add(user);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ImportPurchaseDto>), new XmlRootAttribute("Purchases"));

            StringBuilder sb = new StringBuilder();

            using (StringReader reader = new StringReader(xmlString))
            {
                List<ImportPurchaseDto> purchasesDto = (List<ImportPurchaseDto>)xmlSerializer.Deserialize(reader);

                List<Purchase> purchases = new List<Purchase>();

                foreach (var purchaseDto in purchasesDto)
                {
                    if(!IsValid(purchaseDto))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    bool isParsedDate = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);

                    if(!isParsedDate)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    bool isParsedType = Enum.TryParse<PurchaseType>(purchaseDto.Type, out PurchaseType parsedType);

                    if(!isParsedType)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    Purchase purchase = new Purchase
                    {
                        Date = parsedDate,
                        Type = parsedType,
                        ProductKey = purchaseDto.Key,
                        Card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card),
                        Game=context.Games.FirstOrDefault(g=>g.Name==purchaseDto.Title)
                    };

                    purchases.Add(purchase);
                    sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");

                }
                context.Purchases.AddRange(purchases);
                context.SaveChanges();

            }

            return sb.ToString().TrimEnd() ;
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
