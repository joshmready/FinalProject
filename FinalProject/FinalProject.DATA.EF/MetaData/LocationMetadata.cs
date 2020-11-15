using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DATA.EF
{
    class LocationMetadata
    {

        public int LocationID { get; set; }

        [Required]
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Reservation Limit")]
        public byte ReservationLimit { get; set; }
    }

    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    { }
}
