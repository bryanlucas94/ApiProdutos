using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("Produtos"));


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseSwagger();


app.MapPost("/Produtos", async (Produto produto, AppDbContext dbContext) =>
{
    dbContext.Produtos.Add(produto);
    await dbContext.SaveChangesAsync();

    return produto;
});


app.MapGet("/Produtos", async (AppDbContext dbContext) =>
        await dbContext.Produtos.ToListAsync());


app.MapGet("/Produtos/ {id}", async (int id,AppDbContext dbContext) =>
        await dbContext.Produtos.FirstOrDefaultAsync(a => a.Id == id));


app.MapDelete("/Produtos/{id}", async (int id, AppDbContext dbContext) =>
{
    var produto = await dbContext.Produtos.FirstOrDefaultAsync(a => a.Id == id);
    
    if (produto == null)
    {
        dbContext.Produtos.Remove(produto);
        await dbContext.SaveChangesAsync();
    }    

    return;
});


app.UseSwaggerUI();
app.Run();

public class Produto { 

    public int Id { get; set; }
    public string? Nome { get; set; }    
    public int valor_unitario { get; set; }
    public int qtd_estoque { get; set; }

}

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions options) : base(options)
    {

    }
    public DbSet<Produto> Produtos { get; set; }
}
