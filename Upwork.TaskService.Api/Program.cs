using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Upwork.TaskService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomainServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.TagActionsBy(x => new[] { x.GroupName });
    options.DocInclusionPredicate((docName, description) => true);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.EnableTryItOutByDefault();
    options.DefaultModelsExpandDepth(-1);
    options.DisplayRequestDuration();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
