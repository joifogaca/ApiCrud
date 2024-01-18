using ApiCrud.Data;
using ApiCrud.Estudantes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapGet("Hello-wordl", () => "Hello Word!");
//EstudantesRotas.AddRotasEstudantes(app); => Sem passar o this como parametro no metódo se chama assim

app.AddRotasEstudantes();
app.UseHttpsRedirection();



app.Run();


