![Build Status](https://img.shields.io/github/actions/workflow/status/marlenhalvorsen/Pawsistant/dotnet.yml?label=.NET%20Build&style=for-the-badge)

Pawsistant is a full-stack web application built with Blazor WebAssembly (frontend) and ASP.NET Core (backend), designed to help dog owners get personalized training tips through AI-powered chat.

The backend is structured using a clean architecture approach, featuring controller–service–repository layers. Interfaces and adapters are already in place to support future integration of JWT-based authentication and a more flexible data access layer.

The frontend follows a modular structure with reusable components, pages, and services, promoting maintainability and scalability. Models are shared between frontend and backend through a shared library to ensure consistency and type safety across the stack.

## Branching Strategy

This project uses a feature-based branching model.

Branches are cleaned up after merging, but examples include:

- `feature-added-register-endpoint`
- `bugfix-yml-file`

See closed pull requests for full history.
