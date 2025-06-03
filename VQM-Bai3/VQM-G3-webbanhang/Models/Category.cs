using System.ComponentModel.DataAnnotations;

namespace VQM_G3_webbanhang.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }

    }
}
