# 🎬 Sitio Web de Películas

Aplicación ASP.NET Core MVC para gestionar **Películas, Actores, Directores y Géneros**.

---

## 🚀 Requisitos

- .NET 8.0 SDK o superior
- SQL Server LocalDB o una base de datos compatible
- Visual Studio 2022 / VS Code

---

## ⚙️ Configuración inicial

1. Clona este repositorio:

   ```bash
   git clone https://github.com/Liliana-Imbaquingo/SitioWebDePeliculas
   ```

2. Ingresa a la carpeta del proyecto:

   ```bash
   cd SitioWebDePeliculas
   ```

3. Restaura dependencias:

   ```bash
   dotnet restore
   ```

4. Aplica las migraciones a la base de datos:

   ```bash
   Update-Database
   ```

5. Ejecuta la aplicación:

   ```bash
   dotnet run
   ```

---

## 🧠 Características principales

- CRUD completo para:

  - Actores
  - Directores
  - Géneros
  - Películas (relacionadas con actores, director y género)

- Uso de Entity Framework Core.
- Validaciones por modelo.
- Interfaz Razor con vistas generadas desde scaffolding.

---

## 📄 Estructura

```
Controllers/
├── ActoresController.cs
├── PeliculasController.cs
├── GenerosController.cs
├── DirectoresController.cs
Data/
├── AppDbContext.cs
Models/
├── Actor.cs
├── Pelicula.cs
├── Director.cs
├── Genero.cs
Views/
├── Actores/
├── Peliculas/
```

---

## 🧰 Tecnologías usadas

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server LocalDB
- Bootstrap 5
