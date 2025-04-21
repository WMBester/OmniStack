# OmniStack

[![Build Status](https://dev.azure.com/Morneb/OmniStack/_apis/build/status%2FWMBester.OmniStack?branchName=main)](https://dev.azure.com/Morneb/OmniStack/_build/latest?definitionId=1&branchName=main)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Morneb_OmniStack&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Morneb_OmniStack)

OmniStack is a comprehensive showcase project designed to demonstrate advanced skills in backend API development, AI integration, validation, testing, and DevOps. This project is structured to highlight best practices and modern engineering approaches, making it an ideal portfolio piece.

## Project Overview

- **Backend API:** Built with ASP.NET Core, exposing RESTful endpoints for full CRUD operations on Product entities.
- **AI Integration:** Features an endpoint that connects to OpenAI's GPT-3.5-turbo, demonstrating secure external API consumption, prompt engineering, and robust error handling.
- **Validation:** Utilizes FluentValidation to enforce business rules and ensure data integrity for all incoming requests.
- **Database:** Uses Entity Framework Core with SQLite, including code-first migrations for schema management.
- **Testing:** Includes k6 scripts for load, performance, and stress testing, as well as Dockerized integration tests and end-to-end UI testing with Selenium.
- **DevOps & CI/CD:** Provides Docker and Docker Compose files for containerization, and an Azure Pipelines YAML for automated builds and deployments.
- **Developer Experience:** Swagger UI is enabled for interactive API exploration, and health checks are available for diagnostics.

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- FluentValidation
- OpenAI API (GPT-3.5-turbo)
- Docker & Docker Compose
- Azure Pipelines (CI/CD)
- k6 (Performance Testing)
- Playwright (UI Testing)

## Why This Project Stands Out

- **Clean Architecture:** Clear separation of concerns between controllers, services, models, and validators.
- **Modern Tooling:** Demonstrates proficiency with industry-standard tools for development, testing, and deployment.
- **Production-Ready Practices:** Implements health checks, Swagger documentation, and robust error handling.
- **Extensibility:** Easily adaptable for additional features or integrations.

## Getting Started

1. Clone the repository.
2. Build and run the API using Docker Compose or the .NET CLI.
3. Access Swagger UI at the root URL for API documentation and testing.
4. Run k6 scripts from the `k6-tests` directory for performance testing.

For detailed setup and usage instructions, see the comments in the respective configuration and test files.
