using Entity_Relations.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity_Relations.EntityConfigurations
{
    public class RecipeConfigurations : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("MyRecipes", "system");

            builder.Property(x => x.Name)
             .HasColumnName("Title")
             .HasColumnType("char(30)");


        }
    }


}
