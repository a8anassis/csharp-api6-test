
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SchoolApp.Data;
using SchoolApp.Repositories;
using SchoolApp.Services;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

using System.Text;
using UsersStudentsMVCApp.Configuration;

namespace SchoolApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<SchoolAppDbContext>(options => options.UseSqlServer(connString));

            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddRepositories();

            builder.Services.AddScoped(provider =>
              new MapperConfiguration(cfg =>
              {
                  cfg.AddProfile(new MapperConfig());
              })
          .CreateMapper());

            ///Add Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    //return new JsonWebToken(token); in .NET 8
                    /// Override the default token signature validation an do NOT validtae the signature
                    /// Just return the token
                    SignatureValidator = (token, validator) => { return new JwtSecurityToken(token); }
                };
            });


            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.IncludeErrorDetails = true;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = "https://codingfactory.aueb.gr",

            //        ValidateAudience = true,
            //        ValidAudience = "https://api.codingfactory.aueb.gr",

            //        ValidateLifetime = true, // ensure not expired

            //        ValidateIssuerSigningKey = true,

            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            //            //.GetBytes(builder.Configuration.GetConnectionString("SecretKey")!))
            //            .GetBytes(builder.Configuration["Authentiation: SecretKey"]!))

            //    };
            //});

            // Add services to the container.

            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
            });

            builder.Services.AddCors(options => {
                options.AddPolicy("AngularClient",
                    b => b.WithOrigins("http://localhost:4200") // Assuming Angular runs on localhost:4200
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            /// System.Text.JSON
            /*builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                // Adding a converter for string enums
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });*/

            // If NewtonSoft would be used for json serialization / deserialization
            // We have to add the NuGet dependencies and the following config

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
