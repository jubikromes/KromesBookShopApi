using System;
using System.Collections.Generic;
using System.Text;

namespace Mailing.Core.Models
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

    }
}
