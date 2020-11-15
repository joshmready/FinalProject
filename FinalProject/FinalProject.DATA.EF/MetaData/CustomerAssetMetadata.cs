using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DATA.EF
{
    class CustomerAssetMetadata
    {
        [Required]
        [Display(Name = "Customer Asset ID")]
        public int CustomerAssetID { get; set; }

        [Required]
        [Display(Name = "Vehicle Make/Model")]
        public string AssetName { get; set; }

        //[Required]
        //[Display(Name = "Customer ID")]
        //public string CustomerID { get; set; }

        //[Required]
        [Display(Name = "Vehicle Photo")]
        public string AssetPhoto { get; set; }

        [Display(Name = "Special Notes")]
        [UIHint("MultilineText")]
        public string SpecialNotes { get; set; }

        //[Required]
        //[Display(Name = "Is Active")]
        //public bool IsActive { get; set; }

        //[Required]
        //[Display(Name = "Date Added")]
        //[DisplayFormat(NullDisplayText = "Please enter date added", DataFormatString = "0:d")]
        //public System.DateTime DateAdded { get; set; }
    }

    [MetadataType(typeof(CustomerAssetMetadata))]
    public partial class CustomerAsset
    { }
}
