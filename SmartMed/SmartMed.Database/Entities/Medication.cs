using System;
using System.Collections.Generic;

namespace SmartMed.Database.Entities
{
    public partial class Medication
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Quantity { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
