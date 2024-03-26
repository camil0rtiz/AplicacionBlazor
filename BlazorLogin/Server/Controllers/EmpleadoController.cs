using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorLogin.Server.Models;
using BlazorLogin.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorLogin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {

        private readonly DbcrudBlazorContext _dbContext;
        public EmpleadoController(DbcrudBlazorContext dbContext)
        {

            _dbContext = dbContext;

        }

        [HttpGet]
        [Route("lista")]

        public async Task<ActionResult> Lista()
        {
            var responseApi = new ResponseAPI<List<EmpleadoDTO>>();
            var listaEmpleadoDTO = new List<EmpleadoDTO>();

            try
            {
                foreach (var item in await _dbContext.Empleados.Include(d => d.IdDepartamentoNavigation).ToListAsync())
                {
                    listaEmpleadoDTO.Add(new EmpleadoDTO
                    {
                        IdEmpleado = item.IdEmpleado,
                        NombreCompleto = item.NombreCompleto,
                        IdDepartamento = item.IdDepartamento,
                        Sueldo = item.Sueldo,
                        FechaContrato = item.FechaContrato,
                        Departamento = new DepartamentoDTO
                        {
                            IdDepartamento = item.IdDepartamentoNavigation.IdDepartamento,
                            Nombre = item.IdDepartamentoNavigation.Nombre,
                        }

                    });
                }

                responseApi.EsCorrecto = true;
                responseApi.Valor = listaEmpleadoDTO;

            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;

            }

            return Ok(responseApi);
        }

        [HttpGet]
        [Route("buscar/{id}")]

        public async Task<ActionResult> Buscar(int id)
        {

            var responseApi = new ResponseAPI<EmpleadoDTO>();
            var empleadoDTO = new EmpleadoDTO();

            try
            {
                var dbEmpleado = await _dbContext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);

                if(dbEmpleado != null)
                {
                    empleadoDTO.IdEmpleado = dbEmpleado.IdEmpleado;
                    empleadoDTO.NombreCompleto = dbEmpleado.NombreCompleto;
                    empleadoDTO.IdDepartamento = dbEmpleado.IdDepartamento;
                    empleadoDTO.Sueldo = dbEmpleado.Sueldo;
                    empleadoDTO.FechaContrato = dbEmpleado.FechaContrato;

                    responseApi.EsCorrecto = true;
                    responseApi.Valor = empleadoDTO;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No encontrado";

                }
            }
            catch(Exception ex) 
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);

        }

        [HttpPost]
        [Route("guardar")]

        public async Task<ActionResult> Guardar(EmpleadoDTO empleado)
        {

            var responseApi = new ResponseAPI<int>();;

            try
            {
                var dbEmpleado = new Empleado
                {
                    NombreCompleto = empleado.NombreCompleto,
                    IdDepartamento = empleado.IdDepartamento,
                    Sueldo = empleado.Sueldo,
                    FechaContrato = empleado.FechaContrato,
                };

                _dbContext.Empleados.Add(dbEmpleado);
                await _dbContext.SaveChangesAsync();

                if (dbEmpleado.IdEmpleado != 0)
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbEmpleado.IdEmpleado;

                }
                else
                {

                    responseApi.EsCorrecto = true;
                    responseApi.Mensaje = "No guardado";

                }

            
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);

        }

        [HttpPut]
        [Route("editar/{id}")]

        public async Task<ActionResult> Editar(EmpleadoDTO empleado, int id)
        {

            var responseApi = new ResponseAPI<int>(); ;

            try
            {
                var dbEmpleado = await _dbContext.Empleados.FirstOrDefaultAsync(e=>e.IdEmpleado == id);
              ;

                if (dbEmpleado != null)
                {

                    dbEmpleado.NombreCompleto = empleado.NombreCompleto;
                    dbEmpleado.IdDepartamento = empleado.IdDepartamento;
                    dbEmpleado.Sueldo = empleado.Sueldo;
                    dbEmpleado.FechaContrato = empleado.FechaContrato;

                    
                    _dbContext.Empleados.Update(dbEmpleado);
                    await _dbContext.SaveChangesAsync();

                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbEmpleado.IdEmpleado;

                }
                else
                {

                    responseApi.EsCorrecto = true;
                    responseApi.Mensaje = "Empleado no encontrado";

                }


            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);

        }

        [HttpDelete]
        [Route("eliminar/{id}")]

        public async Task<ActionResult> Eliminar(int id)
        {

            var responseApi = new ResponseAPI<int>(); ;

            try
            {
                var dbEmpleado = await _dbContext.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == id);
                ;

                if (dbEmpleado != null)
                {

                    _dbContext.Remove(dbEmpleado);
                    await _dbContext.SaveChangesAsync();

                    responseApi.EsCorrecto = true;
                
                }
                else
                {

                    responseApi.EsCorrecto = true;
                    responseApi.Mensaje = "Empleado no encontrado";

                }


            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);

        }
    }
}
