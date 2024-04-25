using ApiOAuthPractica.Data;
using ApiOAuthPractica.Helpers;
using ApiOAuthPractica.Repositories;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());



string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryDoctores>();
builder.Services.AddDbContext<DoctoresContext>(options => options.UseSqlServer(connectionString));





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Api OAuth Doctores";
    document.Description = "Api con Token de Seguridad";
    // CONFIGURAMOS LA SEGURIDAD JWT PARA SWAGGER,
    // PERMITE AÑADIR EL TOKEN JWT A LA CABECERA.
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});




var app = builder.Build();
app.UseOpenApi();

app.UseSwaggerUI(options => {
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api OAuth Doctores");
    options.RoutePrefix = "";
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
