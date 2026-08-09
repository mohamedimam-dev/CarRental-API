using CarRental.API.Common;
using CarRental.API.Data;
using CarRental.API.DTOs.Customers;
using CarRental.API.Entities;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CarRentalDbContext _context;
        public CustomerService(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<CustomerDTO>> AddAsync(AddCustomerDTO dto)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == dto.CreatedByUserId);

            if (!userExists)
                return ServiceResult<CustomerDTO>
                    .NotFound("User not found.");

            bool driverLicenseExists = await _context.Customers
                .AnyAsync(c =>
                    c.DriverLicenseNumber == dto.DriverLicenseNumber);

            if (driverLicenseExists)
                return ServiceResult<CustomerDTO>
                    .Conflict("Driver license number already exists.");

            Customer customer = new Customer
            {
                Name = dto.Name,
                ContactInformation = dto.ContactInformation,
                DriverLicenseNumber = dto.DriverLicenseNumber,
                CreatedByUserId = dto.CreatedByUserId
            };

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            CustomerDTO customerDto = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                ContactInformation = customer.ContactInformation,
                DriverLicenseNumber = customer.DriverLicenseNumber,
                CreatedByUserId = customer.CreatedByUserId,
                CreatedDate = customer.CreatedDate
            };

            return ServiceResult<CustomerDTO>.Success(customerDto);
        }

        public async Task<ServiceResult<CustomerDTO>> GetByIdAsync(int customerId)
        {
            CustomerDTO? customer = await _context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerDTO
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    ContactInformation = c.ContactInformation,
                    DriverLicenseNumber = c.DriverLicenseNumber,
                    CreatedByUserId = c.CreatedByUserId,
                    CreatedDate = c.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (customer == null)
                return ServiceResult<CustomerDTO>
                    .NotFound("Customer not found.");

            return ServiceResult<CustomerDTO>.Success(customer);
        }
      
        public async Task<List<CustomerListDTO>> GetAllAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .OrderByDescending(c => c.CustomerId)
                .Select(c => new CustomerListDTO
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    ContactInformation = c.ContactInformation,
                    DriverLicenseNumber = c.DriverLicenseNumber
                })
                .ToListAsync();
        }

        public async Task<ServiceResult<CustomerDTO>> UpdateAsync(
            int customerId,
            UpdateCustomerDTO dto)
        {
            Customer? customer = await _context.Customers
                .FindAsync(customerId);

            if (customer == null)
                return ServiceResult<CustomerDTO>
                    .NotFound("Customer not found.");

            bool isDriverLicenseExists = await _context.Customers
                .AnyAsync(c =>
                    c.DriverLicenseNumber == dto.DriverLicenseNumber &&
                    c.CustomerId != customerId);

            if (isDriverLicenseExists)
                return ServiceResult<CustomerDTO>
                    .Conflict("Driver license number already exists.");

            customer.Name = dto.Name;
            customer.ContactInformation = dto.ContactInformation;
            customer.DriverLicenseNumber = dto.DriverLicenseNumber;

            await _context.SaveChangesAsync();

            CustomerDTO customerDto = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                ContactInformation = customer.ContactInformation,
                DriverLicenseNumber = customer.DriverLicenseNumber,
                CreatedByUserId = customer.CreatedByUserId,
                CreatedDate = customer.CreatedDate
            };

            return ServiceResult<CustomerDTO>.Success(customerDto);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int customerId)
        {
            Customer? customer = await _context.Customers
                .FindAsync(customerId);

            if (customer == null)
                return ServiceResult<bool>
                    .NotFound("Customer not found.");

            bool hasBookings = await _context.RentalBookings
                .AnyAsync(b => b.CustomerId == customerId);

            if (hasBookings)
                return ServiceResult<bool>
                    .Conflict("Customer cannot be deleted because rental bookings exist.");

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
   
    }
}
