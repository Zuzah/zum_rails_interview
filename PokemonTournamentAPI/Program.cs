var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        // Allow for specific Angular default port access
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient so it can be injected into controllers
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors("AllowAngularDev");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // The Swagger endpoint path is relative to the root.
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon Tournament API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
