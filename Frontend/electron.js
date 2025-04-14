const { app, BrowserWindow, ipcMain, dialog } = require('electron');
const path = require('path');
const fs = require('fs');
const axios = require('axios');

// Keep a global reference of the window object to prevent garbage collection
let mainWindow;

// API endpoint base URL configuration
const API_URL = process.env.NODE_ENV === 'development' 
  ? 'http://localhost:5000/api' 
  : 'http://localhost:5000/api'; // Should be configurable in production

// Handle certificate errors for development
if (process.env.NODE_ENV === 'development') {
  app.commandLine.appendSwitch('ignore-certificate-errors');
  app.commandLine.appendSwitch('allow-insecure-localhost', 'true');
}

function createWindow() {
  // Create the browser window
  mainWindow = new BrowserWindow({
    width: 1280,
    height: 800,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
      preload: path.join(__dirname, 'preload.js'),
      // Disable web security in development for easier testing
      webSecurity: process.env.NODE_ENV !== 'development',
      allowRunningInsecureContent: process.env.NODE_ENV === 'development'
    },
    icon: path.join(__dirname, 'public/icon.png'),
    title: 'Side by Side Translator',
    show: false
  });

  // Load the app entry point
  if (process.env.NODE_ENV === 'development') {
    // In development, load from Vite dev server
    // Dynamically detect the port from process env if available
    const port = process.env.VITE_DEV_SERVER_PORT || 5173;
    console.log(`Loading from development server: http://localhost:${port}`);
    mainWindow.loadURL(`http://localhost:${port}`);
    
    // Open DevTools in development mode
    mainWindow.webContents.openDevTools();
  } else {
    // In production, load from built files
    mainWindow.loadFile(path.join(__dirname, 'dist/index.html'));
  }

  // Show window when ready to prevent flickering
  mainWindow.on('ready-to-show', () => {
    mainWindow.show();
  });

  // Emitted when the window is closed
  mainWindow.on('closed', () => {
    mainWindow = null;
  });
}

// Create window when Electron has finished initialization
app.whenReady().then(() => {
  createWindow();

  // On macOS it's common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open
  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) createWindow();
  });
});

// Quit when all windows are closed, except on macOS
app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') app.quit();
});

// In this file you can include the rest of your app's specific main process code
// IPC handlers for communication with the renderer process

// Handler for opening files
ipcMain.handle('dialog:openFile', async () => {
  const { canceled, filePaths } = await dialog.showOpenDialog({
    properties: ['openFile'],
    filters: [
      { name: 'Documents', extensions: ['pdf', 'docx', 'doc', 'txt'] },
      { name: 'Images', extensions: ['jpg', 'jpeg', 'png', 'tiff', 'bmp'] },
      { name: 'All Files', extensions: ['*'] }
    ]
  });
  
  if (canceled) {
    return null;
  } else {
    // Return file path and read file data
    const filePath = filePaths[0];
    
    try {
      // For large files, you might want to stream instead
      const fileContent = fs.readFileSync(filePath);
      const fileName = path.basename(filePath);
      const fileExtension = path.extname(filePath).toLowerCase();
      
      return {
        path: filePath,
        name: fileName,
        extension: fileExtension,
        data: fileContent
      };
    } catch (error) {
      console.error('Error reading file:', error);
      throw error;
    }
  }
});

// Handler for saving files
ipcMain.handle('dialog:saveFile', async (event, { fileContent, defaultPath, filters }) => {
  const { canceled, filePath } = await dialog.showSaveDialog({
    defaultPath,
    filters: filters || [
      { name: 'PDF Documents', extensions: ['pdf'] },
      { name: 'Word Documents', extensions: ['docx'] },
      { name: 'All Files', extensions: ['*'] }
    ]
  });
  
  if (canceled) {
    return { success: false };
  } else {
    try {
      fs.writeFileSync(filePath, fileContent);
      return { success: true, path: filePath };
    } catch (error) {
      console.error('Error saving file:', error);
      return { success: false, error: error.message };
    }
  }
});

// Handler for API requests
ipcMain.handle('api:request', async (event, { method, endpoint, data, params }) => {
  try {
    const response = await axios({
      method: method || 'get',
      url: `${API_URL}/${endpoint}`,
      data,
      params
    });
    
    return { success: true, data: response.data };
  } catch (error) {
    console.error('API request error:', error);
    return { 
      success: false, 
      error: error.response ? error.response.data : error.message 
    };
  }
});

// Check API server status
ipcMain.handle('api:status', async () => {
  try {
    const response = await axios.get(`${API_URL}/health`);
    return { status: 'online', data: response.data };
  } catch (error) {
    console.error('API health check error:', error);
    return { status: 'offline', error: error.message };
  }
}); 