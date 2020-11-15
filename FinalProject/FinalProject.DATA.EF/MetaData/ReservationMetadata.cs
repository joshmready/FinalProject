using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DATA.EF
{
    class ReservationMetadata
    {
        //[Required]
        //[Display(Name = "Reservation ID")]
        //public int ReservationID { get; set; }

        [Required]
        [Display(Name = "Customer Asset ID")]
        public int CustomerAssetID { get; set; }

        [Required]
        [Display(Name = "Location ID")]
        public int LocationID { get; set; }

        [Required]
        [Display(Name = "Reservation Date")]
        public System.DateTime ReservationDate { get; set; }
    }
    [MetadataType(typeof(ReservationMetadata))]
    public partial class Reservation
    { }
}
