using Mailing.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookApp.Core.ViewModels
{

    public class BookViewModel
    {
        public Guid Id { get; set; }
        public BookViewModel()
        {
            Authors = new List<BookAuthorModel>();
        }
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Published { get; set; }

        public string IsbnCode { get; set; }

        public List<BookAuthorModel> Authors { get; set; }



        public static explicit operator BookViewModel(Book source)
        {
            var response = new BookViewModel
            {
                Id = source.Id,
                IsbnCode = source.IsbnCode,
                Published = source.Published,
                Title = source.Title
            };

            return response;
        }
    }

    public class BookAuthorModel
    {
        public string AuthorName { get;set;}
        public Guid AuthorId { get; set; }

    }
    public class CreateBookViewModel
    {
        public CreateBookViewModel()
        {
            AuthorIds = new List<Guid>();
        }
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Published { get; set; }

        public string IsbnCode { get; set; }

        public List<Guid> AuthorIds { get; set; }

        public string CategoryId { get; set; }

    }

    public class BookResponseViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public DateTime Published { get; set; }

        public string IsbnCode { get; set; }

        public List<Guid> Authors { get; set; }


        public static explicit operator BookResponseViewModel(Book source)
        {
            var response = new BookResponseViewModel
            {
                Id = source.Id,
                IsbnCode = source.IsbnCode,
                Published = source.Published,
                Title = source.Title
            };

            return response;
        }
    }

}
