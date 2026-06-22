FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Fluxo.Api/Fluxo.Api.csproj", "Fluxo.Api/"]
COPY ["Fluxo.Application/Fluxo.Application.csproj", "Fluxo.Application/"]
COPY ["Fluxo.Domain/Fluxo.Domain.csproj", "Fluxo.Domain/"]
COPY ["Fluxo.Infrastructure/Fluxo.Infrastructure.csproj", "Fluxo.Infrastructure/"]

RUN dotnet restore "Fluxo.Api/Fluxo.Api.csproj"

COPY . .
WORKDIR "/src/Fluxo.Api"
RUN dotnet build "Fluxo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fluxo.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fluxo.Api.dll"]