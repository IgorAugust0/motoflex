using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Motoflex.Application.Interfaces;

namespace Motoflex.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(
            IAdminRepository repository,
            ILogger<AdminService> logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _logger = logger;
        }


        public async Task<IEnumerable<Admin>> GetAsync()
        {
            try
            {
                var admins = await _repository.GetAsync();
                _logger.LogDebug("Retrieved {Count} administrators", admins.Count());
                return admins;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving administrators");
                throw;
            }
        }

        public async Task<Admin?> CreateAdminAsync()
        {
            try
            {
                var admin = new Admin();
                await _repository.InsertAsync(admin);

                _logger.LogInformation("New administrator created successfully. Id: {AdminId}", admin.Id);
                return admin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating administrator");
                return null;
            }
        }
    }
}
