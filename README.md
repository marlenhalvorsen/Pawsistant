![.NET Build & Test](https://github.com/marlenhalvorsen/Pawsistant/actions/workflows/dotnet.yml/badge.svg)

Pawsistant is a full-stack web application built with Blazor WebAssembly (frontend) and ASP.NET Core (backend), designed to help dog owners get personalized training tips through AI-powered chat.

The backend is structured using a clean architecture approach, featuring controller–service–repository layers. Interfaces and adapters are already in place to support future integration of JWT-based authentication and a more flexible data access layer.

The frontend follows a modular structure with reusable components, pages, and services, promoting maintainability and scalability. Models are shared between frontend and backend through a shared library to ensure consistency and type safety across the stack.

## Branching Strategy

This project uses a feature-based branching model.

Branches are cleaned up after merging, but examples include:

- `feature-added-register-endpoint`
- `bugfix-yml-file`

See closed pull requests for full history.

## API Key Handling & Security
This project previously included an API key in the appsettings.Development.json file. The issue was identified immediately (via GitHub's secret scanning alert), and the following steps were taken:

- The exposed API key was revoked from the third-party provider (OpenRouter).

- The repository history was cleaned using git filter-repo to fully remove the sensitive file and its contents.

- .gitignore was updated to ensure appsettings.Development.json and other environment-specific configuration files are excluded from version control going forward.

- Branch protection rules were enabled to block force pushes to master, and all changes now go through pull requests.

The key was accidentally committed due to a misunderstanding of how .gitignore works: files already tracked by Git are not excluded even if added to .gitignore later. This oversight has since been addressed with both cleanup and updated Git practices.

Sensitive configuration values are now handled locally through environment-specific files and injected securely via IConfiguration. This process follows best practices for application configuration and security.
