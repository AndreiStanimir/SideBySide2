# GitHub Actions Workflows

This document describes the GitHub Actions workflows set up for the Side-by-Side Translator Application.

## Available Workflows

### 1. CI Workflow (`ci.yml`)

Runs tests and checks for the application whenever code is pushed or a pull request is created.

- **Backend Tests**: Builds and validates the .NET API project
- **Frontend Tests**: Validates the Vue.js/Electron frontend (tests commented out until implemented)
- **Tesseract Tests**: Runs tests for the OCRmyPDF service
- **Linting**: Checks code formatting and standards

### 2. Docker Build and Push (`docker-build.yml`)

Builds and pushes Docker images to GitHub Container Registry (GHCR).

- Builds the .NET API container
- Builds the OCRmyPDF container
- Tags images appropriately based on branch/tag
- Pushes to GHCR for later use

This workflow runs on:
- Pushes to the main branch
- New version tags (v*)
- Manual trigger

### 3. Deployment (`deploy.yml`)

Deploys the application to Railway, a free cloud platform for hosting containers.

- Deploys the Backend API service
- Deploys the OCRmyPDF service
- Requires a `RAILWAY_TOKEN` secret to be set in GitHub repository settings

This workflow runs on:
- Pushes to the main branch
- Manual trigger

### 4. Docker Compose Validation (`docker-compose-validation.yml`)

Validates the docker-compose configuration.

- Checks docker-compose.yml syntax
- Tests container startup
- Logs any issues that arise during startup

This workflow runs on:
- Pushes to the main branch that change docker-compose files
- Pull requests that change docker-compose files

## Setting Up Required Secrets

To use these workflows, the following secrets need to be set in your GitHub repository settings:

1. `RAILWAY_TOKEN`: API token for deploying to Railway
   - Get this from your Railway account settings
   - Required for the deployment workflow

## Manual Triggers

The following workflows support manual triggers from the GitHub Actions tab:
- Docker Build and Push
- Deployment

## Notes for Getting Started

1. For Railway deployment:
   - Create an account at [Railway](https://railway.app/)
   - Set up your project and services
   - Generate an API token
   - Add the token as a GitHub repository secret

2. For GitHub Container Registry:
   - The workflows automatically use the built-in `GITHUB_TOKEN` for authentication
   - No additional setup is needed for pushing to GHCR 