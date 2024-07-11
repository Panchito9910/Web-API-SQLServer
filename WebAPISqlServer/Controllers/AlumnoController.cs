using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using WebAPISqlServer.Models;
using WebAPISqlServer.Models.DTO;

namespace WebAPISqlServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        private readonly ModelDualContext _context;
        private readonly IMapper _mapper;
        
        public AlumnoController(ModelDualContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var alumno = await _context.Alumnos
               .Select(al => new
               {
                   al.IdAlumno,
                   al.Matricula,
                   al.NombreAlumno,
                   al.ApellidoAlumno,
                   al.SemestreActual,
                   al.CorreoAlumno,
                   Proyecto = al.Proyectos
                               .Select(p => new { Id = p.IdProyecto, p.CodigoProyecto, p.NombreProyecto, p.CodigoEmpresa }).ToList()
               })
               .ToListAsync();
                if (alumno is null) return NotFound();
                return Ok(new Reply
                {
                    Respuesta="Ok",
                    Mensaje="Éxito al realizar la petición",
                    Datos = alumno
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Reply
                {
                    Respuesta="Error",Mensaje="Ocurrió un error al intentar acceder  a los datos"
                });
            }
           
        }
        [HttpGet("matricula")]
        public async Task<IActionResult> Get(string matricula)
        {
            var alumno = await _context.Alumnos
                .Select(al => new
                {
                    al.IdAlumno,
                    al.Matricula,
                    al.NombreAlumno,
                    al.ApellidoAlumno,
                    al.SemestreActual,
                    al.CorreoAlumno,
                    Proyecto = al.Proyectos
                               .Select(p => new { Id = p.IdProyecto, p.CodigoProyecto, p.NombreProyecto, p.CodigoEmpresa }).ToList()
                })
                .FirstOrDefaultAsync(a => a.Matricula == matricula);
            if (alumno is null) return NotFound();
            return Ok(alumno);
        }
        [HttpPost]
        public async Task<IActionResult> Post(AlumnoDTO alumnoDto)
        {
            var alumno = _mapper.Map<Alumno>(alumnoDto);
            _context.Add(alumno);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("varios")]
        public async Task<IActionResult> Post(AlumnoDTO[] alumnoDTOs)
        {
            var alumno = _mapper.Map<Alumno[]>(alumnoDTOs);
            _context.AddRange(alumno);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}/")]
        public async Task<IActionResult> Put(int id, AlumnoDTO alumnoDTO)
        {
            var alumno = _mapper.Map<Alumno>(alumnoDTO);
            alumno.IdAlumno = id;
            _context.Update(alumno);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}/")]
        public async Task<IActionResult> Delete(int id)
        {
            var del=await _context.Alumnos.Where(a => a.IdAlumno == id).ExecuteDeleteAsync();
            if (del == 0) return NotFound();
            return NoContent();
        }
    }
}
