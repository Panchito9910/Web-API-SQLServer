using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPISqlServer.Models;
using WebAPISqlServer.Models.DTO;

namespace WebAPISqlServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly ModelDualContext _context;
        private readonly IMapper _mapper;
        public ProyectoController(ModelDualContext context,IMapper mapper)
        {
            _context = context;   
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var proyecto = await _context.Proyectos
                .Select(pro => new
                {
                    pro.IdProyecto,
                    pro.CodigoProyecto,
                    pro.NombreProyecto,
                    pro.CodigoEmpresa,
                    pro.Matricula
                }).ToListAsync();
                if (proyecto is null) return NotFound();
                return Ok(new Reply
                {
                    Respuesta="Ok",
                    Mensaje="Exito al realizar la petición",
                    Datos = proyecto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Reply
                {
                    Respuesta = "Error",
                    Mensaje = "Ocurrió un error al intentar acceder  a los datos"
                });
            }
        }
        [HttpGet("codigoProyecto")]
        public async Task<IActionResult> Get(string codigoProyecto)
        {
            var proyecto = await _context.Proyectos
                .Select(pro => new
                {
                    pro.IdProyecto,
                    pro.CodigoProyecto,
                    pro.NombreProyecto,
                    pro.CodigoEmpresa,
                    pro.Matricula
                }).FirstOrDefaultAsync(p=>p.CodigoProyecto==codigoProyecto);
            if (proyecto is null) return NotFound();
            return Ok(proyecto);
        }
        [HttpPost]
        public async Task<IActionResult> Post(ProyectoDTO proyectoDTO)
        {
            var proyecto = _mapper.Map<Proyecto>(proyectoDTO);
            _context.Add(proyecto);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("varios")]
        public async Task<IActionResult> Post(ProyectoDTO[] proyectoDTOs)
        {
            var proyecto = _mapper.Map<Proyecto[]>(proyectoDTOs);
            _context.Add(proyecto);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}/")]
        public async Task<IActionResult> Put(int id,ProyectoDTO proyectoDTO)
        {
            var proyecto = _mapper.Map<Proyecto>(proyectoDTO);
            proyecto.IdProyecto = id;
            _context.Update(proyecto);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}/")]
        public async Task<IActionResult> Delete(int id)
        {
            var del = await _context.Proyectos.Where(p => p.IdProyecto == id).ExecuteDeleteAsync();
            if (del == 0) return NotFound();
            return NoContent();
        }
    }
}
