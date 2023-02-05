using Upwork.TaskService;
using Upwork.TaskService.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationModule();

builder.Services.AddControllers(x =>
{
    x.Filters.Add<ExceptionActionFilter>();
});
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
