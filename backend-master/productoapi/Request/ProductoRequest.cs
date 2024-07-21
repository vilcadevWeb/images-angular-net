namespace productoapi.Request
{
    public class ProductoRequest
    {
        public string Nombre { get; set; }
        public IFormFile ImagenUrl { get; set; }

        public string? Url { get; set; }
    }
}
