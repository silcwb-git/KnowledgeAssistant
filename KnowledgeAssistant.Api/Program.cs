using KnowledgeAssistant.Api.Options;
using KnowledgeAssistant.Api.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KnowledgeAssistant API", Version = "v1" });
});

// Registro dos serviços (DI)
builder.Services.Configure<AiOptions>(builder.Configuration.GetSection(AiOptions.SectionName));
builder.Services.AddSingleton<InMemoryKnowledgeStore>();
builder.Services.AddScoped<IKnowledgeSearch, SimpleKnowledgeSearch>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddHttpClient<IAiClient, GptMakerAiClient>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();