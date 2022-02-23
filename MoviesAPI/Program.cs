using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MoviesAPI;
using MoviesAPI.APIBehavior;
using MoviesAPI.Filters;
using MoviesAPI.Helpers;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
    //options.Filters.Add(typeof(ParseBadRequest));
}).ConfigureApiBehaviorOptions(BadRequestBehavior.Parse);
builder.Services.AddResponseCaching();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    var frontendURL = builder.Configuration["frontend_url"];
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
            .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
    });
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IFileStorageService, AzureStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
