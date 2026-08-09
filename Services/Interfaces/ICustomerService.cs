using CarRental.API.Common;
using CarRental.API.DTOs.Customers;

namespace CarRental.API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerListDTO>> GetAllAsync();

        Task<ServiceResult<CustomerDTO>> GetByIdAsync(int customerId);

        Task<ServiceResult<CustomerDTO>> AddAsync(
            AddCustomerDTO dto,
            int createdByUserId);

        Task<ServiceResult<CustomerDTO>> UpdateAsync(
            int customerId,
            UpdateCustomerDTO dto);

        Task<ServiceResult<bool>> DeleteAsync(int customerId);
    }
}
