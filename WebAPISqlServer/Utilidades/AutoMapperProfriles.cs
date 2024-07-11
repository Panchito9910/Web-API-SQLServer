using AutoMapper;
using WebAPISqlServer.Models;
using WebAPISqlServer.Models.DTO;

namespace WebAPISqlServer.Utilidades
{
    public class AutoMapperProfriles:Profile
    {
        public AutoMapperProfriles() 
        {
            CreateMap<AlumnoDTO, Alumno>();
            CreateMap<EmpresaDTO, Empresa>();
            CreateMap<ProyectoDTO, Proyecto>();
        }
    }
}
