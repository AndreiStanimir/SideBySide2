"use strict";
const { app, BrowserWindow, ipcMain, dialog } = require("electron");
const path = require("path");
const fs = require("fs");
const axios = require("axios");
let mainWindow;
const API_URL = "http://localhost:5000/api";
function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1280,
    height: 800,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
      preload: path.join(__dirname, "preload.js")
    },
    icon: path.join(__dirname, "build/icon.png"),
    title: "Side by Side Translator",
    show: false
  });
  {
    mainWindow.loadURL("http://localhost:3000");
    mainWindow.webContents.openDevTools();
  }
  mainWindow.on("ready-to-show", () => {
    mainWindow.show();
  });
  mainWindow.on("closed", () => {
    mainWindow = null;
  });
}
app.whenReady().then(() => {
  createWindow();
  app.on("activate", () => {
    if (BrowserWindow.getAllWindows().length === 0)
      createWindow();
  });
});
app.on("window-all-closed", () => {
  if (process.platform !== "darwin")
    app.quit();
});
ipcMain.handle("dialog:openFile", async () => {
  const { canceled, filePaths } = await dialog.showOpenDialog({
    properties: ["openFile"],
    filters: [
      { name: "Documents", extensions: ["pdf", "docx", "doc", "txt"] },
      { name: "Images", extensions: ["jpg", "jpeg", "png", "tiff", "bmp"] },
      { name: "All Files", extensions: ["*"] }
    ]
  });
  if (canceled) {
    return null;
  } else {
    const filePath = filePaths[0];
    try {
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
      console.error("Error reading file:", error);
      throw error;
    }
  }
});
ipcMain.handle("dialog:saveFile", async (event, { fileContent, defaultPath, filters }) => {
  const { canceled, filePath } = await dialog.showSaveDialog({
    defaultPath,
    filters: filters || [
      { name: "PDF Documents", extensions: ["pdf"] },
      { name: "Word Documents", extensions: ["docx"] },
      { name: "All Files", extensions: ["*"] }
    ]
  });
  if (canceled) {
    return { success: false };
  } else {
    try {
      fs.writeFileSync(filePath, fileContent);
      return { success: true, path: filePath };
    } catch (error) {
      console.error("Error saving file:", error);
      return { success: false, error: error.message };
    }
  }
});
ipcMain.handle("api:request", async (event, { method, endpoint, data, params }) => {
  try {
    const response = await axios({
      method: method || "get",
      url: `${API_URL}/${endpoint}`,
      data,
      params
    });
    return { success: true, data: response.data };
  } catch (error) {
    console.error("API request error:", error);
    return {
      success: false,
      error: error.response ? error.response.data : error.message
    };
  }
});
ipcMain.handle("api:status", async () => {
  try {
    const response = await axios.get(`${API_URL}/health`);
    return { status: "online", data: response.data };
  } catch (error) {
    console.error("API health check error:", error);
    return { status: "offline", error: error.message };
  }
});
