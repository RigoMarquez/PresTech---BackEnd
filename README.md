🚀 Instalación y Configuración
Prerrequisitos

.NET SDK 6.0+
SQL Server o SQL Server Express
Visual Studio 2022 o VS Code

Pasos de Instalación

Clonar el repositorio

bash   git clone https://github.com/JorgeIRamos/PresTech-BackEnd.git
   cd PresTech-BackEnd

Restaurar dependencias

bash   dotnet restore

Configurar la cadena de conexión
Edita el archivo appsettings.json con tu cadena de conexión:

json   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=PresTechDB;Trusted_Connection=True;"
     }
   }

Aplicar migraciones de base de datos

bash   dotnet ef database update

Ejecutar la aplicación

bash   dotnet run
La API estará disponible en: https://localhost:7105 o http://localhost:5159
