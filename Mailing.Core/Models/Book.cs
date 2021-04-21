using BookApp.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kromes.Core.Models
{
    public class Book  : BaseEntity
    {
        
        [MaxLength(300)]

        public string Title { get; set; }
        public DateTime Published { get; set; }

        public string IsbnCode { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }


    }
}
