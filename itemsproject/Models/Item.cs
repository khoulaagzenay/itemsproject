using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace itemsproject.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required, ForeignKey(nameof(Category))]
        public int CategoryId { get; set; } 
        public Category? Category { get; set; }

        // Relation many-to-many avec Client
        [Display(Name = "Client")]
        public ICollection<Client>? Clients { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
