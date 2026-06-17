using FluentValidation;
using Fluxo.Api.Exceptions;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Fluxo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((_, __, configuration) => configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
            theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code
        ));

    builder.Services.AddOpenApi();
    builder.Services.AddControllers();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddDbContext<FluxoDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IFluxoDbContext>(provider =>
        provider.GetRequiredService<FluxoDbContext>());
    //builder.Services.AddScoped<IGetTransactionsHandler, GetTransactionsHandler>();
    //builder.Services.AddScoped<ICreateTransactionHandler, CreateTransactionHandler>();

    builder.Services.AddValidatorsFromAssemblyContaining<CreateTransactionCommandValidator>();

    builder.Services.Scan(scan => scan
        .FromAssemblies(
            typeof(ICreateTransactionCommandHandler).Assembly,
            typeof(FluxoDbContext).Assembly)
        .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Handler")))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVueClient", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
    });


    var app = builder.Build();

    app.UseExceptionHandler();
    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseCors("AllowVueClient");

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}