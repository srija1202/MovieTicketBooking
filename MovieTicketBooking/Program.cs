using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieTicketBooking.Business.Repository;
using MovieTicketBooking.Business.Service;
using MovieTicketBooking.Data;
using MovieTicketBooking.Repository.Interface;
using MovieTicketBooking.Service.Interface;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "Movie Ticket Booking",
        Description = "ASP.Net application"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Enter your Json Web Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

// Configure database connection
builder.Services.Configure<DatabaseConnection>(builder.Configuration.GetSection(nameof(DatabaseConnection)));
builder.Services.AddSingleton<IDatabaseConnection>(x => x.GetRequiredService<IOptions<DatabaseConnection>>().Value);

// Dependency Injection for Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ITheaterService, TheaterService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Dependency Injection for Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITheaterRepository, TheaterRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Configure AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure Authentication using JWT Bearer
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidIssuers = new string[] { builder.Configuration["Jwt:Issuer"] },
        ValidAudiences = new string[] { builder.Configuration["Jwt:Audience"] },
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configure Authorization policies
builder.Services.AddAuthorization(policy =>
{
    policy.AddPolicy("CustomerOnly", options =>
    {
        options.RequireRole("Customer");
    });
});

builder.Services.AddAuthorization(policy =>
{
    policy.AddPolicy("AdminOnly", options =>
    {
        options.RequireRole("Admin");
    });
});

builder.Services.AddAuthorization(policy =>
{
    policy.AddPolicy("CustomerAndAdmin", options =>
    {
        options.RequireRole("Customer", "Admin");
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCors", x =>
    {
        x.WithOrigins("http://localhost:7213")
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("EnableCors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
