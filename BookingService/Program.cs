using Amazon.DynamoDBv2;
using BookingService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Configure AWS Services
// --------------------
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonDynamoDB>();

// --------------------
// Dependency Injection
// --------------------
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// --------------------
// Add Controllers + Swagger
// --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------
// Build App
// --------------------
var app = builder.Build();

// --------------------
// Middleware
// --------------------
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

// --------------------
// Map Controllers
// --------------------
app.MapControllers();

// --------------------
// Run Application
// --------------------
app.Run();
