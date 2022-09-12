using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace RealEstate.Models
{

    public class ImportExcel
    {
        [Required(ErrorMessage = "Please select file")]
        [FileExt(Allow = ".xls,.xlsx", ErrorMessage = "Only excel file")]
        public HttpPostedFileBase file { get; set; }
    }

    public class Tb_Comment
    {

        public Notification thongBao { get; set; }
        public List<Comment> lstComments { get; set; }
    }
   
    public class JsonModelReturnViewSanPham
    {
        public Estate Estate { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class FileExt : ValidationAttribute
    {
        public string Allow;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string extension = ((System.Web.HttpPostedFileBase)value).FileName.Split('.')[1];
                if (Allow.Contains(extension))
                    return ValidationResult.Success;
                else
                    return new ValidationResult(ErrorMessage);
            }
            else
                return ValidationResult.Success;
        }
    }


    public class EstateArguments
    {
        public string EstateCode { get; set; } // Ký hiệu
        public long? TownId { get; set; }
        public long? Estate_TypeId { get; set; }
        public RangeAreaDto AreaRange { get; set; }
        public int? Estate_GroupId { get; set; }
        public RangePriceDto PriceRange { get; set; }
        public int? AccountId { get; set; }
        public bool IsFromFilters { get; set; }
        public bool IsHot { get; set; }
        public bool isOutSide { get; set; }
        public string houseNumber { get; set; }
        public int Number_Lot { get; set; }
        public int Number_Paper { get; set; }
        public int SortId { get; set; }
        // role for load estate
        public Role role { get; set; }

        public long? Estate_StatusId { get; set; }
        public long? Estate_ProjectId { get; set; }
        public long? WardId { get; set; }


        public void ResetFilters()
        {
            EstateCode = string.Empty;
            TownId = default(int?);
            Estate_TypeId = default(int?);
            AreaRange = null;
            PriceRange = null;
            Estate_GroupId = null;
            PriceRange = null;
            IsHot = false;
        }
    }
    public class RangePriceDto
    {
        public double From { get; set; }
        public double To { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is RangePriceDto item))
            {
                return false;
            }
            return this.From.Equals(item.From) && this.To.Equals(item.To);
        }
    }
    public class RangeAreaDto
    {
        public double From { get; set; }
        public double To { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is RangePriceDto item))
            {
                return false;
            }
            return this.From.Equals(item.From) && this.To.Equals(item.To);
        }
    }

   
}
