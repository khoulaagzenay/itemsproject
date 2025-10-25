using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace itemsproject.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public ICollection<Category>? categories { get; set; }
        public ICollection<Item>? Items { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public byte[]? dbImage { get; set; }

        [NotMapped]
        public string? ImageSrc
        {
            get
            {
                if (dbImage != null)
                {
                    var imageBase64Data = Convert.ToBase64String(dbImage, 0, dbImage.Length);
                    return "data:image/jpg;base64, + imageBase64Data ";
                }
                return string.Empty;
            }
        }
    }
    
}
