using System.Text;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
// Add database context
builder.Services.AddDbContext<ToDoDbContext>();
//swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());  

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/items", async (ToDoDbContext db) =>
    await db.Items.ToListAsync());

app.MapGet("/items/{id}", async (int Id, ToDoDbContext Db) =>
await Db.Items.FindAsync(Id)
    is Item todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.MapPost("/items", async (Item item, ToDoDbContext Db) =>
{
    var todoItem = new Item
    {
        IsComplete = item.IsComplete,
        Name = item.Name
    };
    Db.Items.Add(todoItem);
    await Db.SaveChangesAsync();
    return Results.Created($"/items/{todoItem.Id}", todoItem);
});

app.MapPut("/items/{id}", async (int Id, ToDoDbContext Db) =>
{
    var todo = await Db.Items.FindAsync(Id);
    if (todo is null) return Results.NotFound();
    todo.IsComplete = true;
    await Db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int Id, ToDoDbContext Db) =>
{
    if (await Db.Items.FindAsync(Id) is Item todo)
    {
        Db.Items.Remove(todo);
        await Db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});

app.Run();
