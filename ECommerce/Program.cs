using System.Text;

using ECommerceOrderManagement.Data;
using ECommerceOrderManagement.DTOs.Common;
using ECommerceOrderManagement.Middleware;
using ECommerceOrderManagement.Models;
using ECommerceOrderManagement.Repositories.Implementations;
using ECommerceOrderManagement.Repositories.Interfaces;
using ECommerceOrderManagement.Services.Implementations;
using ECommerceOrderManagement.Services.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);


// 1. ADD CONTROLLERS


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage)
                .Where(message => !string.IsNullOrWhiteSpace(message))
                .ToList();

            var message = errors.Count > 0
                ? string.Join(" ", errors)
                : "One or more validation errors occurred.";

            var response = new ApiResponse<object>
            {
                Status = false,
                Message = message,
                Data = null
            };

            return new BadRequestObjectResult(response);
        };
    });



// 2. DATABASE CONFIGURATION


var connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection is missing from appsettings.json.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// 3. ASP.NET CORE IDENTITY


builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



// 4. JWT AUTHENTICATION

var jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT Key is missing from appsettings.json.");

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "JWT Issuer is missing from appsettings.json.");

var jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "JWT Audience is missing from appsettings.json.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,

                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
            };
    });


// 5. AUTHORIZATION


builder.Services.AddAuthorization();


// 6. REPOSITORY DEPENDENCY INJECTION


builder.Services.AddScoped<
    IProductRepository,
    ProductRepository>();

builder.Services.AddScoped<
    ICategoryRepository,
    CategoryRepository>();

builder.Services.AddScoped<
    ICustomerRepository,
    CustomerRepository>();

builder.Services.AddScoped<
    IOrderRepository,
    OrderRepository>();

builder.Services.AddScoped<
    IReportRepository,
    ReportRepository>();


// 7. SERVICE DEPENDENCY INJECTION


builder.Services.AddScoped<
    IProductService,
    ProductService>();

builder.Services.AddScoped<
    ICategoryService,
    CategoryService>();

builder.Services.AddScoped<
    ICustomerService,
    CustomerService>();

builder.Services.AddScoped<
    IOrderService,
    OrderService>();

builder.Services.AddScoped<
    IAuthService,
    AuthService>();


// 8. REPORT SERVICE


builder.Services.AddScoped<
    IReportService,
    ReportService>();


// 9. SWAGGER / OPENAPI


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    
    // Swagger document
    

    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "E-Commerce Order Management API",

            Version = "v1",

            Description =
                "ASP.NET Core Web API for E-Commerce Order Management"
        });


    
    // JWT Bearer Security Definition
    

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Enter your JWT token."
        });


    
    // JWT Security Requirement
    

    options.AddSecurityRequirement(
        document =>
            new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecuritySchemeReference(
                        "Bearer",
                        document)
                ] = []
            });
});



// 10. BUILD APPLICATION


var app = builder.Build();


// 11. GLOBAL EXCEPTION HANDLING MIDDLEWARE


app.UseMiddleware<ExceptionHandlingMiddleware>();



// 12. SWAGGER


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}



// 13. HTTPS REDIRECTION

app.UseHttpsRedirection();



// 14. AUTHENTICATION


app.UseAuthentication();



// 15. AUTHORIZATION


app.UseAuthorization();



// 16. MAP CONTROLLERS


app.MapControllers();


// 17. SEED IDENTITY ROLES


using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedRolesAsync(
        scope.ServiceProvider);
}


// 18. RUN APPLICATION


app.Run();