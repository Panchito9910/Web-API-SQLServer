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
    public class EmpresaController : ControllerBase
    {
        private readonly ModelDualContext _context;
        private readonly IMapper _mapper;
        public EmpresaController(ModelDualContext context,IMapper mapper)
        {
            _context = new ModelDualContext();
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var empresa = await _context.Empresas
                .Select(em => new
                {
                    em.IdEmpresa,
                    em.CodigoEmpresa,
                    em.NombreEmpresa,
                    em.CorreoEmpresa,
                    Proyectos = em.Proyectos.Select(p => new { p.IdProyecto, p.CodigoProyecto, p.NombreProyecto }).ToList()
                }).ToListAsync();
                if (empresa is null) return NotFound();
                return Ok(new Reply
                {
                    Respuesta="Ok",
                    Mensaje="Exito al realizar la petición",
                    Datos=empresa
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
        [HttpGet("codigoEmpresa")]
        public async Task<IActionResult> Get(string codigoEmpresa)
        {
            var empresa = await _context.Empresas
                .Select(em => new
                {
                    em.IdEmpresa,
                    em.CodigoEmpresa,
                    em.NombreEmpresa,
                    em.CorreoEmpresa,
                    Proyectos = em.Proyectos.Select(p => new { p.IdProyecto, p.CodigoProyecto, p.NombreProyecto }).ToList()
                }).FirstOrDefaultAsync(e=>e.CodigoEmpresa==codigoEmpresa);
            if (empresa is null) return NotFound();
            return Ok(empresa);
        }
        [HttpPost]
        public async Task<IActionResult> Post(EmpresaDTO empresaDto)
        {
            var empresa = _mapper.Map<Empresa>(empresaDto);
            _context.Add(empresa);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("varios")]
        public async Task<IActionResult> Post(EmpresaDTO[] empresaDTOs)
        {
            var empresa = _mapper.Map<Empresa[]>(empresaDTOs);
            _context.Add(empresa);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}/")]
        public async Task<IActionResult> Put(int id,EmpresaDTO empresaDTO)
        {
            var empresa = _mapper.Map<Empresa>(empresaDTO);
            empresa.IdEmpresa = id;
            _context.Update(empresa);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}/")]
        public async Task<IActionResult> Delete(int id)
        {
            var del = await _context.Empresas.Where(e => e.IdEmpresa == id).ExecuteDeleteAsync();
            if (del == 0) return NotFound();
            return NoContent();
        }
    }
}
