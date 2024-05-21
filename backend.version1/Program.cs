using backend.Domain.Interfaces;
using backend.Infrastructure.Repositories;
using backend.version1.Domain.AutoMapper;
using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;
using backend.version1.Domain.Interfaces;
using backend.version1.Infrastructure.Repositories;
using backend.version1.Infrastructure.Seeders;
using backend.version1.Infrastructure.Services;
using backend.version1.Infrastructure.Services.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
string applicationUrl = configuration["ApplicationUrl"];
var urls = applicationUrl.Split(';');
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = 257;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IAutoMapperBase<Product, ProductDto>, AutoMapperBase<Product, ProductDto>>();
builder.Services.AddScoped<IAutoMapperBase<User, UserDto>, AutoMapperBase<User, UserDto>>();
builder.Services.AddScoped<IAutoMapperBase<Post, PostDto>, AutoMapperBase<Post, PostDto>>();
builder.Services.AddScoped(typeof(IAutoMapperBase<,>), typeof(AutoMapperBase<,>));
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IBaseService<Product, ProductDto>, ProductService>();
builder.Services.AddScoped<IBaseService<User, UserDto>, AuthenticationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddTransient<DatabaseSeeder>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(provider =>
{
    var accessor = provider.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/api/v1");
    return new UriService(uri);
});

//for identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wedding Planner API", Version = "v1" });
    c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header. \r\n\r\n
                      Enter your token in the text input below.
                      \r\n\r\nExample: '12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "JWT"
                },
                Scheme = "JWT",
                Name = "JWT",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// 6. Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient",
        b =>
        {
            b
                .WithOrigins(urls)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
//for logging
var path = Directory.GetCurrentDirectory();
var app = builder.Build();
var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        seeder.SeedData();
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UsePathBase("/api/v1");

app.MapControllers();

app.Run();