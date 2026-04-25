# PresTech - BackEnd
## 🚀 Descripción del Proyecto
Este repositorio contiene el código fuente del BackEnd para el sitio web de PresTech. Es la capa de servicios y lógica de negocio responsable de gestionar la información, la autenticación de usuarios y la comunicación con la base de datos para la aplicación web.

El objetivo principal es proporcionar una API robusta y escalable para que el FrontEnd (sitio web de PresTech) pueda interactuar de manera eficiente.

## 💻 Tecnologías Utilizadas
El proyecto fue desarrollado utilizando el ecosistema de Microsoft .NET, lo que garantiza un rendimiento óptimo y un desarrollo estructurado.

Lenguaje: C#

Framework: .NET / ASP.NET Core (Asumido por el uso de C# y ser un Backend web moderno)

Gestión de Dependencias: NuGet

Base de Datos: MySQL

ORM (Mapeo Objeto-Relacional): Entity Framework Core

## ⚙️ Configuración e Instalación
Sigue estos pasos para obtener una copia operativa del proyecto en tu máquina local con fines de desarrollo y pruebas.

Requisitos Previos
Asegúrate de tener instalado lo siguiente:

.NET SDK: Versión 9.0 o superior.

IDE: Visual Studio

Base de Datos: MySQL descargado

⚙️ Pasos de Instalación

```bash
    git clone https://github.com/RigoMarquez/PresTech---BackEnd.git
cd PresTech---BackEnd/PresTechBackEnd
dotnet restore
dotnet ef database update
   ```
▶️ Ejecución del Proyecto

Una vez configurado, puedes ejecutar el backend desde tu terminal:

```bash
    dotnet run
   ```
La API estará disponible en la URL especificada en la configuración del proyecto
(usualmente https://localhost:5001 o http://localhost:5000).

🔗 Proyecto Relacionado

Frontend (React): https://github.com/RigoMarquez/ProyectoWebPresTech-Fronted.git

🤝 Trabajo en Equipo

Este proyecto fue desarrollado como proyecto académico en equipo.

Mi participación incluyó:

Desarrollo y mantenimiento del backend en ASP.NET Core

Implementación de lógica de negocio y endpoints

Integración con la base de datos mediante Entity Framework Core

Apoyo en la integración con el frontend y pruebas del sistema

👨‍💻 Autores

Rigoberto Márquez Fernández

Jorge Iván Ramos Murgas

📌 Nota

Este repositorio se conserva con fines educativos y de portafolio, y representa un proyecto desarrollado durante el proceso de formación académica en tecnologías backend con .NET.
