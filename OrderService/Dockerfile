# Imagen base para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto y publicar
COPY . ./
RUN dotnet publish -c Release -o out

# Imagen base para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Exponer puerto
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Iniciar la app
ENTRYPOINT ["dotnet", "OrderService.dll"]