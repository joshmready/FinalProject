using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Message Title")]
        public string MessageTitle { get; set; }
        [Required]
        [UIHint("MultilineText")]
        [Display(Name = "Message Body")]
        public string Message { get; set; }
    }
}