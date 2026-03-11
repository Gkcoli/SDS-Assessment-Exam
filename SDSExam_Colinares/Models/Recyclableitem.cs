using System.ComponentModel.DataAnnotations;

namespace SDSExam_Colinares.Models
{
    public class RecyclableItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Recyclable Type is required.")]
        public int RecyclableTypeId { get; set; }

        public string RecyclableTypeName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(150)]
        public string ItemDescription { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        public decimal Weight { get; set; }

        public decimal ComputedRate { get; set; }
    }
}