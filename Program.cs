using ContactNotesAPI.Helpers;
using ContactNotesAPI.Models;
using ContactNotesAPI.Repositories;
using ContactNotesAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ContactNotesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new CreateNoteDtoInputFormatter());
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
