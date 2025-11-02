# SE4458 Assignment 2: Song Playlist REST API

This repository contains a Song Playlist REST API developed with .NET 8 Web API for the SE4458 Assignment 2. The project is deployed live on Azure App Service.

## 1. Project Links

* **Code (GitHub Repository):**
    `https://github.com/kaan-devv/se4458-playlist-api`

* **Deployment (Live Swagger UI):**
    `https://se4458-kaan-api-dbbaescgdnhshbb7.canadacentral-01.azurewebsites.net/swagger/index.html`

## 2. Project Design

The project was designed using a standard Controller-Model structure within the .NET 8 Web API framework, based on the provided "To-do app template".

* **`Models/Song.cs`**: Defines the properties of the Song object (e.g., Title, Artist, Album).
* **`Controllers/SongsController.cs`**: Contains the main API logic and exposes the following endpoints as required by the "Group 2 - Playlist" assignment:
    * `GET /api/Songs`: Lists all songs.
    * `POST /api/Songs`: Creates a new song.
    * `GET /api/Songs/{id}`: Retrieves a specific song by its ID.
    * `PUT /api/Songs/{id}`: Updates a specific song by its ID.
    * `DELETE /api/Songs/{id}`: Deletes a specific song by its ID.
    * `POST /api/Songs/Search`: Searches for songs based on a given criterion.
**Database:** As required by the assignment, all data is stored in an in-memory database using `Microsoft.EntityFrameworkCore.InMemory`.

## 3. Assumptions

* **Unique ID:** It was a core design decision that every song added to the database would be assigned a unique, auto-incrementing ID by the database. This ID is used as the primary key for `GET (by id)`, `PUT`, and `DELETE` operations.
* **Search Function:** It was assumed that the `POST /api/Songs/Search` endpoint should perform a search **within the song titles** based on a `query` parameter provided in the JSON request body.

## 4. Issues Encountered

* **Azure Deployment Error (HTTP 500.30):** Upon the initial `zip deploy` to Azure App Service, the application failed to start, returning an `HTTP Error 500.30 - ASP.NET Core app failed to start`.
* **Resolution:** To diagnose the issue, the **"Log stream"** tool in the Azure Portal was used. The logs indicated that the application was failing during startup. The root cause was found to be the `app.UseFileServer()` configuration in `Program.cs`. This code was attempting to map a physical path for a `"Static"` folder that did not exist in the Azure App Service deployment environment.
* **Fix:** The `app.UseFileServer` code block (lines 59-65) was commented out, as it was not required for a pure API project. After redeploying the application with this change, the startup error was resolved, and the application started successfully.
