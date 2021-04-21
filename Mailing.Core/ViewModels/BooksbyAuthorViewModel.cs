using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookApp.Core.ViewModels
{
    public class BooksbyAuthorViewModel
    {
        [Required]
        public Guid BookId { get; set; }
        [Required]


        public string BookTitle { get; set; }


        public DateTime Published { get; set; }

        public string IsbnCode { get; set; }

        public string AuthorName { get; set; }

    }
}
