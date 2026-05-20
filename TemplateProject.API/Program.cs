using TemplateProject.API.DI;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
//#if (userTracking)
builder.Services.AddCurrentUser();
//#endif
//#if (autoDI)
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
//#endif
builder.Services.AddControllers();
builder.Services.AddOpenApi();

//#if (mapster)
builder.Services.AddMapsterMapping();
//#endif

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
