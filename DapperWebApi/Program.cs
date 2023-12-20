
using DapperWebApi.Data;
using DapperWebApi.Middlewares;
using DapperWebApi.Repo;

namespace DapperWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddXmlSerializerFormatters();

            //builder.Services.AddDbContext<DapperDbContext>(); //NOT USING AS EF CORE IS NOT USED IN THIS CASE
            builder.Services.AddScoped<DapperDbContext>();
            builder.Services.AddScoped<IVillaRepo,VillaRepo>();

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

            app.UseAuthorization();


            app.MapControllers();

            //middleware - can place anywhere between builder.build and app.run
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.Run();
        }
    }
}