using FluentValidation;
using Fluxo.Api.Exceptions;
using Fluxo.Application.Accounts.Common;
using Fluxo.Application.Categories.Common;
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
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code,
        applyThemeToRedirectedOutput: true
    )
    .CreateBootstrapLogger();

var exitCode = 0;

try
{
    Log.Information("Starting up");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(
        (_, __, configuration) =>
            configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override(
                    "Microsoft.EntityFrameworkCore.Database.Command",
                    LogEventLevel.Information
                )
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                    theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code,
                    applyThemeToRedirectedOutput: true
                )
    );

    builder.Services.AddOpenApi();
    builder.Services.AddControllers();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddDbContext<FluxoDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("Fluxo.Infrastructure")
        )
    );

    builder.Services.AddScoped<IFluxoDbContext>(provider =>
        provider.GetRequiredService<FluxoDbContext>()
    );

    builder.Services.AddScoped<IAccountUniquenessChecker, AccountUniquenessChecker>();
    builder.Services.AddScoped<ICategoryUniquenessChecker, CategoryUniquenessChecker>();

    builder.Services.AddValidatorsFromAssemblyContaining<CreateTransactionCommandValidator>();

    builder.Services.Scan(scan =>
        scan.FromAssemblies(
                typeof(ICreateTransactionCommandHandler).Assembly,
                typeof(FluxoDbContext).Assembly
            )
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Handler")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
    );

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            "AllowVueClient",
            policy => policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()
        );
    });

    var app = builder.Build();

    app.UseExceptionHandler();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        options.GetLevel = (httpContext, _, ex) =>
            ex is not null
            || httpContext.Response.StatusCode >= StatusCodes.Status500InternalServerError
                ? LogEventLevel.Error
            : httpContext.Response.StatusCode >= StatusCodes.Status400BadRequest
                ? LogEventLevel.Warning
            : LogEventLevel.Information;
    });

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
catch (Exception ex) when (ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Application start-up failed");
    exitCode = 1;
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

Environment.Exit(exitCode);
