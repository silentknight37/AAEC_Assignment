using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using MessagingService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// AWS Configuration
// ----------------------
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonDynamoDB>();

// ----------------------
// Dependency Injection
// ----------------------
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// ----------------------
// Controllers & Swagger
// ----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------
// Build App
// ----------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
