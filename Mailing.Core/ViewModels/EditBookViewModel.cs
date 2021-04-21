using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookApp.Core.ViewModels
{
    public class EditBookViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]

        public string Title { get; set; }

        public DateTime Published { get; set; }

        public string IsbnCode { get; set; }

        public List<Guid> AuthorIds { get; set; }

        public string CategoryId { get; set; }
    }
}
