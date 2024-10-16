using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Filters;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using Microsoft.OpenApi.Models;
using NZWalks.API.Repositories.Images;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //injected service

// Load DbContext to the program
// ---------------------ADDED---------------------------------------//
builder.Services.AddControllers(options => {
    options.Filters.Add(new LoggingFilter("App"));
    options.Filters.Add(new ValidateModel());
});

builder.Services.AddDbContext<NZWalksDbContext>(
    options => options.UseSqlServer(
        builder
        .Configuration
        .GetConnectionString("NZWalksConnectionString")
    )
);

// Inject Auth DB
builder.Services.AddDbContext<NZWalksAuthDbContext>(
    options => options.UseSqlServer(
        builder
        .Configuration
        .GetConnectionString("NZWalksAuthConnectionString")
    )
);

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]
            )
        )
    }
);

builder.Services
.AddIdentityCore<IdentityUser>()
.AddRoles<IdentityRole>()
.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
.AddEntityFrameworkStores<NZWalksAuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(
    options => {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    }
);

builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo {
            Title = "NZ Walks API Sergio",
            Version = "v1"
        }
    );

    options.AddSecurityDefinition(
        JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        }
    );

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Name = JwtBearerDefaults.AuthenticationScheme,
                Scheme = "Oauth2",
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddHttpContextAccessor();
// -----------------------------------------------------------------//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// added
app.UseAuthentication();

app.UseAuthorization();
// added
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Images")
    )
});

app.MapControllers();

app.Run();
