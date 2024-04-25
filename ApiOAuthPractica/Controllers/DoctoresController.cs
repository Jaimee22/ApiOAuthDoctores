using ApiOAuthPractica.Models;
using ApiOAuthPractica.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiOAuthPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctoresController : ControllerBase
    {

        private RepositoryDoctores repo;

        public DoctoresController(RepositoryDoctores repo) 
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Doctor>>> GetDoctores()
        { 
            return await this.repo.GetDoctorsAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> FindDoctor(int id)
        { 
            Doctor doctor = await this.repo.FindDoctorAsync(id);

            if (doctor == null)
            { 
                return NotFound();
            }
            else 
            {
                return Ok(doctor);
            }

        }



    }
}
