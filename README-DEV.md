# Side by Side Translator - Development Guide

This guide provides detailed instructions for setting up and running the Side by Side Translator application for development and testing purposes.

## Prerequisites

### Required Software

1. **.NET 9 SDK and Runtime**
   - Download and install from [.NET 9 Download Page](https://dotnet.microsoft.com/download/dotnet/9.0)
   - Verify installation: `dotnet --version`

2. **Node.js and npm**
   - Download and install from [Node.js Website](https://nodejs.org/)
   - Recommended version: 18.x LTS or newer
   - Verify installation:
     ```bash
     node --version
     npm --version
     ```

3. **Docker and Docker Compose**
   - Download and install [Docker Desktop](https://www.docker.com/products/docker-desktop)
   - Verify installation:
     ```bash
     docker --version
     docker-compose --version
     ```

4. **MongoDB Compass** (Optional, for database management)
   - Download from [MongoDB Compass](https://www.mongodb.com/products/compass)

### IDE Setup

We recommend using Visual Studio Code with the following extensions:
- Docker
- C# Dev Kit
- Vue Language Features (Volar)
- ESLint
- Prettier

## Getting Started

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/SideBySide2.git
   cd SideBySide2
   ```

2. **Start Backend Services**
   ```bash
   # Start all Docker containers
   docker-compose up -d

   # Verify containers are running
   docker-compose ps
   ```

   This will start:
   - API Service (port 5000)
   - MongoDB (port 27017)
   - Redis (port 6379)
   - Tesseract OCR Service

3. **Setup Frontend Development Environment**
   ```bash
   # Navigate to Frontend directory
   cd Frontend

   # Install dependencies
   npm install

   # Start development server
   npm run dev
   ```

   The Electron app should launch automatically in development mode.

## Development Workflow

### Backend Development

1. **API Service**
   - Location: `Backend/SideBySideAPI/`
   - Run tests:
     ```bash
     cd Backend/SideBySideAPI
     dotnet test
     ```
   - The API is hot-reloaded in the Docker container

2. **OCR Service**
   - Location: `Tesseract/`
   - Changes to `ocr_processing.py` will be reflected immediately due to volume mounting

### Frontend Development

1. **Electron/Vue Development**
   - Main process: `Frontend/electron.js`
   - Preload script: `Frontend/preload.js`
   - Vue app: `Frontend/src/`
   - Hot reload is enabled by default

2. **Building the Frontend**
   ```bash
   cd Frontend
   npm run build
   ```

3. **Creating Installers**
   ```bash
   cd Frontend
   npm run make
   ```

## Testing

### Backend Testing

1. **API Tests**
   ```bash
   cd Backend/SideBySideAPI
   dotnet test
   ```

2. **Manual API Testing**
   - Use MongoDB Compass to connect to `mongodb://localhost:27017`
   - API endpoints available at `http://localhost:5000/api`

### Frontend Testing

1. **Development Mode**
   ```bash
   cd Frontend
   npm run dev
   ```

2. **Production Build Testing**
   ```bash
   cd Frontend
   npm run build
   npm run preview
   ```

## Common Issues and Solutions

1. **Docker Containers Not Starting**
   - Check Docker logs: `docker-compose logs`
   - Ensure ports 5000, 27017, and 6379 are available
   - Try rebuilding: `docker-compose up -d --build`

2. **Frontend Development Issues**
   - Clear npm cache: `npm cache clean --force`
   - Delete node_modules: `rm -rf node_modules`
   - Reinstall dependencies: `npm install`

3. **Database Connection Issues**
   - Verify MongoDB container is running: `docker-compose ps`
   - Check MongoDB logs: `docker-compose logs mongodb`
   - Ensure connection string is correct in API settings

## Debugging

### Backend Debugging

1. **API Service**
   - Use Visual Studio Code's attach to process feature
   - Docker container is configured for remote debugging

2. **OCR Service**
   - Logs available through Docker: `docker-compose logs tesseract`
   - Add print statements in `ocr_processing.py` for debugging

### Frontend Debugging

1. **Electron Main Process**
   - DevTools available in development mode
   - Console logs visible in terminal

2. **Vue App**
   - Vue DevTools available in development mode
   - Browser DevTools for component inspection

## Contributing

1. Create a feature branch from main
2. Make your changes
3. Run all tests
4. Submit a pull request

## Additional Resources

- [Vue.js Documentation](https://vuejs.org/)
- [Electron Documentation](https://www.electronjs.org/docs)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Docker Documentation](https://docs.docker.com/) 