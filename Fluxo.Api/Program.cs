using FluentValidation;
using Fluxo.Api.Exceptions;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Fluxo.Application.Transactions.Queries.GetTransactions;
using Fluxo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<FluxoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IFluxoDbContext>(provider =>
    provider.GetRequiredService<FluxoDbContext>());
builder.Services.AddScoped<IGetTransactionsHandler, GetTransactionsHandler>();
builder.Services.AddScoped<ICreateTransactionHandler, CreateTransactionHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTransactionCommandValidator>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fluxo API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();
