using Kromes.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookApp.Core.Models
{
    public class AuthorBook : BaseEntity
    {
        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public Guid AuthorId { get; set; }

        public Author Author { get; set; }

    }
}
