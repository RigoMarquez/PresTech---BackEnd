# Imagen base para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de solución y proyecto
COPY ["PresTechBackEnd.sln", "./"]
COPY ["PresTechBackEnd/PresTechBackEnd.csproj", "PresTechBackEnd/"]

# Restaura las dependencias
RUN dotnet restore "PresTechBackEnd/PresTechBackEnd.csproj"

# Copia todo el código fuente
COPY . .

# Compila el proyecto
WORKDIR "/src/PresTechBackEnd"
RUN dotnet build "PresTechBackEnd.csproj" -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish "PresTechBackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final solo con runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PresTechBackEnd.dll"]