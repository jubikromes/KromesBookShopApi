using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookApp.Core.Models.Map
{
    public class AuthorMap : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData
                (
                new Author { CreatedBy = "", FirstName = "Gareth", LastName = "Bale" , Id = Guid.NewGuid()},
                new Author { CreatedBy = "", FirstName = "Cristiano", LastName = "Ronaldo", Id = Guid.NewGuid() },
                new Author { CreatedBy = "", FirstName = "Stephen", LastName = "King", Id = Guid.NewGuid() }
                );
            //throw new NotImplementedException();
        }
    }
}
