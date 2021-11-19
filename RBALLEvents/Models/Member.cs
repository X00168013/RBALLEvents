using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RBALLEvents.Models
{
    public class Member : User
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }

        public ICollection<Registration> Registrations { get; set; }
    }
}
