using System.ComponentModel.DataAnnotations;

namespace productoapi.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }

        public string ImagenUrl { get; set; }
    }
}
