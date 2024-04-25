using ApiOAuthPractica.Data;
using ApiOAuthPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOAuthPractica.Repositories
{
    public class RepositoryDoctores
    {

        private DoctoresContext context;

        public RepositoryDoctores(DoctoresContext context)
        { 
            this.context = context;
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            return await this.context.Doctores.ToListAsync();
        }

        public async Task<Doctor> FindDoctorAsync(int idDoctor)
        {
            return await this.context.Doctores.FirstOrDefaultAsync(x => x.IdDoctor == idDoctor);
        }

        public async Task<Doctor> LoginDoctorAsync(string apellido, int idDoctor)
        {
            return await this.context.Doctores.Where(x => x.Apellido == apellido && x.IdDoctor == idDoctor).FirstOrDefaultAsync();
        }


    }
}
