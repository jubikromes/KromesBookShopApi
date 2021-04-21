using Kromes.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookApp.Core.Models
{
    public class Author : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
