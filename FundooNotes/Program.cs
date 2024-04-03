using BussinesLayer.Interface;
using BussinesLayer.Service;
using Microsoft.Extensions.Options;
using ModelLayer.Email;
using Repository.Context;
using Repository.Interface;
using Repository.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddScoped<IRegistration, RegistrationService>();
builder.Services.AddScoped<IRegistrationbl, RegistrationServicebl>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IEmail, EmailService>();
builder.Services.AddScoped<IEmailbl, EmailServicebl>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);
builder.Services.AddScoped<INotes, NotesService>();
builder.Services.AddScoped<INotesbl,NotesServicebl>();
builder.Services.AddScoped<ICollaboration, CollaborationService>();
builder.Services.AddScoped<ICollaborationbl, CollaborationServicebl>();

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
