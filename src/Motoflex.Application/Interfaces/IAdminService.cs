using Motoflex.Domain.Entities;

namespace Motoflex.Application.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<Admin>> GetAsync();
        Task<Admin?> CreateAdminAsync();
    }
}
