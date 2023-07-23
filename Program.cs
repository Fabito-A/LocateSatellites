var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
  option.AddPolicy("PoliticaCors",app =>
   {
     app.AllowAnyOrigin() // cualquier origen
        .AllowAnyHeader()   // cualquier cabecera
        .AllowAnyMethod();  // cualquier metodo get,post put & delete
   });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PoliticaCors");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
