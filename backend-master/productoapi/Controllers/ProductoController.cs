using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using productoapi.Models;
using productoapi.Request;
using productoapi.Services;

namespace productoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        private readonly ProductoService _ProductoService;

        public ProductoController(ProductoService productoService)
        {
            _ProductoService = productoService;
        }


        [HttpGet]
        public async Task<IEnumerable<Producto>> Get()
        {
            return await _ProductoService.Get();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromForm] ProductoRequest request)
        {
            var result = await _ProductoService.Post(request);

            if (!result.success)
            {
                return BadRequest(new { response = new { error = result.error } });
            }

            return Ok(new { response = "Producto agregado exitosamente" });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, [FromForm] ProductoRequest request)
        {
            var result = await _ProductoService.Put(id, request);

            if (!result.success)
            {
                return BadRequest(new { response = new { error = result.error } });
            }

            return Ok(new { response = "Producto actualizado exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Remove(int id)
        {
            var result = await _ProductoService.Remove(id);

            if (!result.success)
            {
                return BadRequest(new { response = new { error = result.error } });
            }

            return Ok(new { response = "Producto eliminado exitosamente" });
        }
    }
}
