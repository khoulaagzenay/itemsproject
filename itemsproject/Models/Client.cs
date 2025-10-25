using System.ComponentModel.DataAnnotations;        
using System.ComponentModel.DataAnnotations.Schema;

namespace itemsproject.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string ? Name { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
