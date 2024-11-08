﻿using Motoflex.Domain.Entities;

namespace Motoflex.Application.Interfaces
{
    public interface IRentalService
    {
        Task<Rental?> GetByIdAsync(Guid id);
        Task<bool> InsertRentalAsync(Rental rental);
        Task<decimal> ReportReturnAsync(Guid id, DateTime returnDate, Guid renterId);
    }
}
