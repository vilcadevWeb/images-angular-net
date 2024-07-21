using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productoapi.Enums;
using productoapi.Models;
using productoapi.Request;

namespace productoapi.Services
{
    public class ProductoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAzureBlobStorageService AzureBlobStorageService;
        public ProductoService(ApplicationDbContext context, IAzureBlobStorageService azureBlobStorageService)
        {
            _context = context;
            this.AzureBlobStorageService = azureBlobStorageService;
        }

        public async Task<IEnumerable<Producto>> Get()
        {
            var productos = await _context.Productos.ToListAsync();
            return productos;
        }

        public async Task<(bool success, string error)> Post([FromForm] ProductoRequest request)
        {
            var platillo = new Producto
            {
                Nombre = request.Nombre,
            };

            try
            {
                if (request.ImagenUrl != null)
                {
                    platillo.ImagenUrl = await this.AzureBlobStorageService.UploadAsync(request.ImagenUrl, ContainerEnum.productocontenedor);
                }
                _context.Add(platillo);
                await _context.SaveChangesAsync();
                return (true, "Producto agregado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, "Hubo un error al agregar el producto");
            }

        }


        public async Task<(bool success, string error)> Put(int id, [FromForm] ProductoRequest request)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return (false, "No se encontró el producto");

            }
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                producto.Nombre = request.Nombre;
            }
            if (request.ImagenUrl != null)
            {
                producto.ImagenUrl = await this.AzureBlobStorageService.UploadAsync(request.ImagenUrl, ContainerEnum.productocontenedor, request.Url);
            }
            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return (false, "Hubo un error al actualizar");
            }

            return (true, "Producto actualizado exitosamente");
        }

        public async Task<(bool success, string error)> Remove(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return (false, "No se encontró el producto");
            }

            if (!string.IsNullOrEmpty(producto.ImagenUrl))
            {
                await this.AzureBlobStorageService.DeleteAsync(ContainerEnum.productocontenedor, producto.ImagenUrl);
            }
            this._context.Productos.Remove(producto);
            this._context.SaveChanges();
            return (true, "Producto eliminado exitosamente");
        }

    }
}
