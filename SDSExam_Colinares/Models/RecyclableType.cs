using System.ComponentModel.DataAnnotations;

namespace SDSExam_Colinares.Models
{
    public class RecyclableType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [MaxLength(100)]
        public string Type { get; set; }

        [Required(ErrorMessage = "Rate is required.")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "MinKg is required.")]
        public decimal MinKg { get; set; }

        [Required(ErrorMessage = "MaxKg is required.")]
        public decimal MaxKg { get; set; }
    }
}