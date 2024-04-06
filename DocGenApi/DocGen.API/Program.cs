var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Routing and map controllers
app.UseRouting();

// Endpoint routing for controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});

app.Run();