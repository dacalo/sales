using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Common.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        [Display(Name ="Image")]
        public string ImagePath { get; set; }
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode =false)]
        public decimal Price { get; set; }
        [Display(Name ="Is Available")]
        public bool IsAvailable { get; set; }
        [Display(Name = "Publish On")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                }
                return $"http://10.1.39.5:84{this.ImagePath.Substring(1)}";
            }
        }

        //public override string ToString()
        //{
        //    return this.Description;
        //}

    }
}
