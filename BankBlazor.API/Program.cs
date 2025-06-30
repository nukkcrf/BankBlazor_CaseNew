
namespace BankBlazor.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }
    }
}
