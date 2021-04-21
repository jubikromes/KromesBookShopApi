using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookApp.Core.Models.Map
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData
              (
              new Category { CreatedBy = "", Id = Guid.NewGuid(), Title = "Fiction" },
              new Category { CreatedBy = "",  Id = Guid.NewGuid(), Title = "Non-Fiction" },
              new Category { CreatedBy = "",  Id = Guid.NewGuid(), Title = "Spiritual" }
              );
        }
    }
}
