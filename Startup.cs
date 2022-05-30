using DisneyAPI.Data;
using DisneyAPI.Models;
using DisneyAPI.Repositories;
using DisneyAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme);

            services.AddDbContext<DisneyDbContext>(
                optionsbuilder =>optionsbuilder.UseSqlServer(Configuration.GetConnectionString("DisneyConnectionString"))
            );
            
            services.AddDbContext<UserDbContext>(
                optionsbuilder => optionsbuilder.UseSqlServer(Configuration.GetConnectionString("UsersConnectionString"))
            );

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UserDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options => 
            { 
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters 
                { 
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "https://localhost:5001",
                    ValidIssuer = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySuperLargaAUTORIZADORAdeUsuarios"))
                };
            });

            services.AddScoped<ICharactersService, CharactersService>();
            services.AddScoped<ICharactersRepository, CharactersRepository>();
            services.AddScoped<IMoviesOrSeriesService, MoviesOrSeriesService>();
            services.AddScoped<IMoviesOrSeriesRepository, MoviesOrSeriesRepository>();

            services.AddControllers();

            services.AddRouting(routing => routing.LowercaseUrls = true);

            AddCors(services);

            AddSwagger(services);
            
        }

        private void AddCors(IServiceCollection services) 
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Disney API",
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:5001",
                                                          "https://localhost:5000");
                                  });
            });
        }

        private void AddSwagger(IServiceCollection services) 
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Disney {groupName}",
                    Version = groupName,
                    Description = "Disney API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "Jwt",
                    In = ParameterLocation.Header,
                    Description = "Ingrese 'Bearer [token]' para poder autenticarse dentro de la aplicación"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new List<string>()
                    }
                });
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Disney API V1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Disney API");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
