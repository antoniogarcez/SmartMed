using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SmartMed.Client.Entities
{
    [DataContract]
    public class Medication
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "quantity", IsRequired = true)]
        public decimal Quantity { get; set; }
    }
}
