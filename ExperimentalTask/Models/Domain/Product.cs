using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace ExperimentalTask.Models.Domain
{
    [Index(nameof(ProduceDate), IsUnique = true)]
    [Index(nameof(ManufactureEmail), IsUnique = true)]
    public class Product
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [DisplayFormat(DataFormatString = "MM/dd/yyyy")]
        public DateTime ProduceDate { get; set; }
        [Required]
        [Phone]
        public string ManufacturePhone { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string ManufactureEmail { get; set; } = string.Empty;
        /// <summary>Description (True: is valid _ False: isn't valid) or it can handle with enums</summary>
        [Required]
        public bool IsAvailable { get; set; }

        //Enum
        //public enum IsAvailable { Yes, No, Shortage}
    }
}
