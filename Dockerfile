# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# --- COPY PROJECT FILES ---
# We need to replicate your exact folder structure inside the container
# so that the relative paths (e.g. "..//DotLink.Domain") work correctly.

COPY ["DotLink.Api/DotLink.Api/DotLink.Api.csproj", "DotLink.Api/DotLink.Api/"]

COPY ["DotLink.Application/DotLink.Application.csproj", "DotLink.Application/"]
COPY ["DotLink.Domain/DotLink.Domain.csproj", "DotLink.Domain/"]
COPY ["DotLink.Infrastructure/DotLink.Infrastructure.csproj", "DotLink.Infrastructure/"]

RUN dotnet restore "DotLink.Api/DotLink.Api/DotLink.Api.csproj"

COPY . .

WORKDIR "/src/DotLink.Api/DotLink.Api"
RUN dotnet build "DotLink.Api.csproj" -c Release -o /app/build
RUN dotnet publish "DotLink.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DotLink.Api.dll"]