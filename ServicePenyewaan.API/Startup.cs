using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServicePenyewaan.API.ServiceHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePenyewaan.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConstantsClass.JWT_KEY = Configuration.GetSection("JwtKey").Value;
            ConstantsClass.JWT_ISSUER = Configuration.GetSection("JwtIssuer").Value;
            ConstantsClass.JWT_EXPIRES_DAYS = Configuration.GetSection("JwtExpireDays").Value;
            ConstantsClass.CONNECTION_STRINGS = Configuration.GetConnectionString("PenyewaanDb");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.AddMvcCore()
                .AddNewtonsoftJson()
                .AddApiExplorer();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = ConstantsClass.JWT_ISSUER,
                    ValidAudience = ConstantsClass.JWT_ISSUER,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConstantsClass.JWT_KEY)),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ServicePenyewaan.API",
                    Version = "v1",
                    Description = "Penyewaan Management",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Administrator",
                        Email = "admin@gmail.com",
                        Url = new Uri("https://example.com/website")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "Your License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.OAuth2,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("http://localhost:5679/api/services/gateway/v1/auth/login", UriKind.Absolute),
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "OAuth2"
                            }
                        },
                        new string[]{ }
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                //string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // pick comments from classes, include controller comments
                //c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                // enable the annotations on Controller classes [SwaggerTag]
                //c.EnableAnnotations();

                c.OperationFilter<AuthorizeCheckOperationFilter>();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServicePenyewaan.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
