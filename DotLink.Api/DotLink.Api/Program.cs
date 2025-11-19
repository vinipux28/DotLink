using DotLink.Application.Configuration;
using DotLink.Application.Features.Users.RegisterUser;
using DotLink.Application.PipelineBehaviors;
using DotLink.Application.Repositories;
using DotLink.Application.Services;
using DotLink.Infrastructure.Data;
using DotLink.Infrastructure.Repositories;
using DotLink.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        "logs/dotlink-log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting DotLink API web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddScoped<IJwtService, JwtService>();
    builder.Services.AddScoped<IFileStorageService>(provider =>
    {
        var env = provider.GetRequiredService<IWebHostEnvironment>();

        var storageSettings = builder.Configuration.GetSection("StorageSettings");
        var relativePathSegment = storageSettings["LocalStoragePath"] ?? "uploads";

        // Prefer WebRootPath, fallback to ContentRootPath, then current directory
        var localStorageRoot = env.WebRootPath ?? env.ContentRootPath ?? Directory.GetCurrentDirectory();

        var localStoragePath = Path.Combine(localStorageRoot, relativePathSegment);

        if (!Directory.Exists(localStoragePath))
        {
            Directory.CreateDirectory(localStoragePath);
        }

        return new LocalFileStorageService(localStoragePath);
    });


    var jwtSettings = builder.Configuration.GetSection("Jwt");
    // Use UTF8 to properly encode secrets containing non-ASCII characters
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
    builder.Services.AddAuthorization();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<DotLinkDbContext>(options =>
        options.UseNpgsql(connectionString)
    );

    builder.Services.AddScoped<IPostRepository, PostRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPostVoteRepository, PostVoteRepository>();
    builder.Services.AddScoped<ICommentRepository, CommentRepository>();

    builder.Services.AddTransient<IEmailService, EmailServiceConsole>();
    builder.Services.AddTransient<IDTOMapperService, DTOMapperService>();


    // Register MediatR handlers from the application features assembly (where commands/handlers live)
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

    builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserCommand).Assembly);


    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));



    builder.Services.AddControllers(options =>
    {
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotLink API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token: Bearer {token}"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });

    builder.Services.Configure<ClientSettings>(
        builder.Configuration.GetSection(ClientSettings.SectionName)
    );


    var app = builder.Build();

    app.UseMiddleware<DotLink.Api.Middleware.ErrorHandlingMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotLink API V1");
            c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
        });
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}