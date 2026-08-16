using CarRental.API.Authorization;
using CarRental.API.Data;
using CarRental.API.Services;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JWT:Issuer"],

            ValidAudience = builder.Configuration["JWT:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JWT:SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, UserOwnerOrAdminHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOwnerOrAdmin", policy =>
        policy.Requirements.Add(new UserOwnerOrAdminRequirement()));
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Register Swagger generator and customize its behavior.
builder.Services.AddSwaggerGen(options =>
{
    // ===============================
    // 1) Define the JWT Bearer security scheme
    // ===============================
    //
    // This tells Swagger that our API uses JWT Bearer authentication
    // through the HTTP Authorization header.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // The name of the HTTP header where the token will be sent.
        Name = "Authorization",


        // Indicates this is an HTTP authentication scheme.
        Type = SecuritySchemeType.Http,


        // Specifies the authentication scheme name.
        // Must be exactly "Bearer" for JWT Bearer tokens.
        Scheme = "Bearer",


        // Optional metadata to describe the token format.
        BearerFormat = "JWT",


        // Specifies that the token is sent in the request header.
        In = ParameterLocation.Header,


        // Text shown in Swagger UI to guide the user.
        Description = "Enter: Bearer {your JWT token}"
    });


    // ===============================
    // 2) Require the Bearer scheme for secured endpoints
    // ===============================
    //
    // This tells Swagger that endpoints protected by [Authorize]
    // require the Bearer token defined above.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                // Reference the previously defined "Bearer" security scheme.
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },


            // No scopes are required for JWT Bearer authentication.
            // This array is empty because JWT does not use OAuth scopes here.
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<CarRentalDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IRentalBookingService, RentalBookingService>();
builder.Services.AddScoped<IRentalTransactionService, RentalTransactionService>();
builder.Services.AddScoped<IVehicleReturnService, VehicleReturnService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IMaintenanceCompletionService, MaintenanceCompletionService>();
builder.Services.AddScoped<IFuelTypeService, FuelTypeService>();
builder.Services.AddScoped<IVehicleCategoryService, VehicleCategoryService>();
builder.Services.AddScoped<IBookingStatusService, BookingStatusService>();
builder.Services.AddScoped<IMaintenanceStatusService, MaintenanceStatusService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISecurityLogService, SecurityLogService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("CarRentalApiPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7137",
                "http://localhost:5218"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CarRentalApiPolicy");

app.UseRateLimiter();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
    {
        await context.Response.WriteAsync("Too many login attempts. Please try again later.");
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
