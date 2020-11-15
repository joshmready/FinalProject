using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DATA.EF
{
    class UserDetailMetadata
    {
        [Required]
        [Display(Name = "First Name")]
        [DisplayFormat(NullDisplayText = "*First Name is required")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [DisplayFormat(NullDisplayText = "*Last Name is required")]
        public string LastName { get; set; }
    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        [Display(Name = "Customer Name")]
        public string FullName
            {
                get { return FirstName + " " + LastName; }
            }
    }
}
