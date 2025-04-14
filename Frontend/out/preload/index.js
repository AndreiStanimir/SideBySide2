"use strict";
const { contextBridge, ipcRenderer } = require("electron");
contextBridge.exposeInMainWorld("api", {
  // File operations
  openFile: () => ipcRenderer.invoke("dialog:openFile"),
  saveFile: (options) => ipcRenderer.invoke("dialog:saveFile", options),
  // API communication
  request: (options) => ipcRenderer.invoke("api:request", options),
  checkStatus: () => ipcRenderer.invoke("api:status"),
  // Application info
  getAppVersion: () => process.env.npm_package_version,
  getAppName: () => process.env.npm_package_name
});
