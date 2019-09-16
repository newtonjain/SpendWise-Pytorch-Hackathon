using System;
using System.ComponentModel.DataAnnotations;

namespace ResourceModel
{
    public class PredictiveItemModel
    {
        [Required]
        [Display(Name = "ID")]
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Name")]
        public string ItemName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Category")]
        public string ItemCategory { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Amount")]
        public decimal ItemAmount { get; set; }
    }
}
