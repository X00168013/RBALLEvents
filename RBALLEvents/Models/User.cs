using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RBALLEvents.Models
{
        public enum Gender
        {
            Male, Female, NA
        }

        public enum AccountType
        {
            Member, EventCoordinator
        }
        public class User
        {  
        public int ID { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(50)]
            public string LastName { get; set; }

        [Required]
       [Column("FirstName")]
       [Display(Name = "First Name")]
       [StringLength(50)]
      public string FirstName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]

            public DateTime DOB { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Required]
            public int PhoneNumber { get; set; }

            [Required]
            public string Address { get; set; }

            public Gender Gender { get; set; }

            public AccountType AccountType { get; set; }

            [Display(Name = "Full Name")]
            public string FullName
            {
                get
                {
                    return LastName + ", " + FirstName;
                }
            }


    }
    }