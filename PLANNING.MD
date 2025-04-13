# Side-by-Side Translator Application - Architecture & Vision

## Project Overview

A full-stack application using Electron.js, Vue.js, and C# .NET 9 to assist translators with document translation, redaction, annotation, and other tasks.

## Project Goals

- Create a unified environment for translators to work with PDF documents
- Enable efficient translation workflow through an integrated three-panel interface
- Provide robust PDF processing with OCR capabilities
- Support document annotation, redaction, and editing in a single application
- Implement translation memory for improved productivity
- Deliver a cross-platform desktop application with professional performance

## Architecture Overview

The application follows a three-tier architecture:
1. **UI Layer**: Electron.js application with Vue.js
2. **Business Logic Layer**: C# .NET 9 backend API services
3. **Data Layer**: Database for persistent storage

### Backend Architecture
- RESTful API developed with .NET 9
- PDF processing pipeline using Tesseract OCR
- Document conversion services
- Authentication and authorization
- Translation memory database

### Frontend Architecture
- Electron.js shell for desktop application
- Vue.js for UI components and state management
- Three-panel layout:
  - Left: PDF viewer and editor
  - Middle: Translation memory suggestions
  - Right: Translated document preview
- File management system

## Data Flow

1. User loads PDF document into the application
2. Backend processes document with OCR if needed
3. Document text is extracted and displayed in editable format
4. Translation memory suggests translations for text segments
5. User edits/translates the document
6. Changes sync to translated preview in real-time
7. Annotations and redactions are applied to the document
8. Final document can be exported in various formats

## Technology Stack

### Backend
- C# .NET 9
- Tesseract OCR
- SQL Server or PostgreSQL
- Entity Framework Core
- ASP.NET Core Web API

### Frontend
- Electron.js
- Vue.js 3
- Vuex or Pinia for state management
- PDF.js for PDF rendering
- Tailwind CSS or Element UI for styling

## Project Structure

### Backend
- `/Backend/SideBySide.sln` - Main solution file
- `/Backend/SideBySideAPI/` - .NET 9 API project
  - `/Controllers/` - API endpoints
  - `/Services/` - Business logic
  - `/Models/` - Data models
  - `/Data/` - Database context
  - `/Middleware/` - Custom middleware components
  - `/Config/` - Configuration files

### Frontend
- `/Frontend/` - Electron application root
  - `/package.json` - Dependencies and scripts
  - `/electron.js` - Electron main process
  - `/src/` - Vue.js application source
    - `/App.vue` - Main Vue application
    - `/components/` - Vue components
      - `/PDFViewer.vue` - PDF viewing component
      - `/TranslationMemory.vue` - Translation suggestions
      - `/TranslatedPreview.vue` - Preview component
    - `/services/` - Frontend services
      - `/apiService.js` - Backend communication
    - `/store/` - State management
    - `/assets/` - Static resources

## Code Style Guidelines

### Backend (C#)
- Follow Microsoft's C# coding conventions
- Use async/await for asynchronous operations
- Implement interface-based design for testability
- Use dependency injection
- Include XML documentation for public APIs
- Follow RESTful API design principles

### Frontend (JavaScript/TypeScript)
- Use TypeScript for type safety
- Follow Vue.js style guide (Priority A rules are essential)
- Use Vue composition API for component logic
- Implement proper error handling
- Follow Airbnb JavaScript style guide
- Use CSS modules or scoped styles

## Development Workflow

### Local Development
1. Set up local development environment
2. Run backend API locally
3. Run Electron app in development mode
4. Use hot reloading for frontend changes
5. Implement features according to TASK.md

### Testing Strategy
- Unit tests for services and components
- Integration tests for API endpoints
- E2E tests for critical user journeys
- Manual testing for UI/UX validation

### Deployment
- Package Electron app for Windows platform
- Set up CI/CD pipeline for automated builds
- Create installer and auto-update mechanism
- Include documentation and release notes

## Performance Considerations

- PDF rendering must be efficient and responsive
- OCR processing should be performed asynchronously
- Translation memory lookups need to be fast
- UI must remain responsive during backend operations
- Consider offline capability for basic functionality

## Security Requirements

- Secure user authentication
- File encryption for sensitive documents
- Secure API communication
- Data privacy for translation memory
- Secure storage of user preferences

## Future Enhancements

- AI-assisted translation
- Cloud synchronization
- Collaborative translation features
- Extended file format support
- Mobile companion application
- Translation memory sharing 