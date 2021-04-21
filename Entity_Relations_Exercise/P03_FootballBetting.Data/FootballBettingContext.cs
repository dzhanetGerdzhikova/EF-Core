using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class P01_StudentSystem : DbContext
    {
        public P01_StudentSystem()
        {
        }

        public P01_StudentSystem(DbContextOptions<P01_StudentSystem> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(team =>
           {
               team.HasKey(t => t.TeamId);
               team.Property(t => t.Name).HasMaxLength(50).IsRequired(true).IsUnicode(true);
               team.Property(t => t.LogoUrl).IsRequired(true).IsUnicode(false);
               team.Property(t => t.Initials).HasMaxLength(3).IsRequired(true).IsUnicode(true);

               team.HasOne(t => t.PrimaryKitColor)
               .WithMany(c => c.PrimaryKitTeams)
               .HasForeignKey(t => t.PrimaryKitColorId)
               .OnDelete(DeleteBehavior.Restrict);

               team.HasOne(t => t.SecondaryKitColor)
               .WithMany(c => c.SecondaryKitTeams)
               .HasForeignKey(t => t.SecondaryKitColorId)
               .OnDelete(DeleteBehavior.Restrict);

               team.HasOne(t => t.Town).WithMany(tw => tw.Teams).HasForeignKey(t => t.TownId);
           });

            modelBuilder.Entity<Color>(color =>
            {
                color.HasKey(c => c.ColorId);
                color.Property(c => c.Name).HasMaxLength(30).IsRequired(true).IsUnicode(false);
            });

            modelBuilder.Entity<Town>(town =>
            {
                town.HasKey(to => to.TownId);
                town.Property(to => to.Name).HasMaxLength(50).IsRequired(true).IsUnicode(true);
                town.HasOne(to => to.Country)
                .WithMany(co => co.Towns)
                .HasForeignKey(to => to.CountryId);
            });

            modelBuilder.Entity<Country>(country =>
            {
                country.HasKey(co => co.CountryId);
                country.Property(co => co.Name).HasMaxLength(50).IsRequired(true).IsUnicode(false);
            });

            modelBuilder.Entity<Player>(player =>
                {
                    player.HasKey(p => p.PlayerId);
                    player.Property(p => p.Name).HasMaxLength(80).IsRequired(true).IsUnicode(true);

                    player.HasOne(p => p.Team)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.TeamId);

                    player.HasOne(p => p.Position)
                    .WithMany(pos => pos.Players)
                    .HasForeignKey(p => p.PositionId);
                });

            modelBuilder.Entity<Position>(position =>
            {
                position.HasKey(pos => pos.PositionId);
                position.Property(pos => pos.Name).HasMaxLength(30).IsRequired(true).IsUnicode(false);
            });

            modelBuilder.Entity<PlayerStatistic>(playerStatistic =>
            {
                playerStatistic.HasKey(ps => new { ps.GameId, ps.PlayerId });

                playerStatistic
                .HasOne(ps => ps.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(ps => ps.PlayerId);

                playerStatistic
                .HasOne(ps => ps.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(ps => ps.GameId);
            });

            modelBuilder.Entity<Game>(game =>
            {
                game.HasKey(g => g.GameId);

                game.HasKey(g => g.GameId);
                game
                .HasOne(g => g.HomeTeam)
                .WithMany(ht => ht.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                game
                .HasOne(g => g.AwayTeam)
                .WithMany(at => at.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                game.Property(g => g.Result).HasMaxLength(7).IsRequired(false).IsUnicode(false);
            });

            modelBuilder.Entity<Bet>(bet =>
            {
                bet.HasKey(b => b.BetId);

                bet
                .HasOne(b => b.Game)
                .WithMany(g => g.Bets)
                .HasForeignKey(b => b.GameId);

                bet
                .HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId);
            });

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.UserId);
                user.Property(u => u.Username).HasMaxLength(50).IsRequired(true).IsUnicode(false);
                user.Property(u => u.Password).HasMaxLength(256).IsRequired(true).IsUnicode(false);
                user.Property(u => u.Email).HasMaxLength(50).IsRequired(true).IsUnicode(false);
                user.Property(u => u.Name).HasMaxLength(80).IsRequired(false).IsUnicode(true);
            });
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}