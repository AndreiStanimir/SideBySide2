<template>
  <div class="app-container">
    <header class="app-header">
      <h1>Side by Side Translator</h1>
      <div class="toolbar">
        <button @click="openFile">Open Document</button>
        <button @click="saveTranslation">Save Translation</button>
        <button @click="checkApiStatus">Check API Status</button>
      </div>
    </header>
    
    <main class="main-content">
      <div v-if="!currentDocument" class="welcome-screen">
        <h2>Welcome to Side by Side Translator</h2>
        <p>Open a document to get started</p>
        <button @click="openFile" class="open-file-btn">Open Document</button>
      </div>
      
      <div v-else class="document-workspace">
        <div class="panel pdf-viewer">
          <div class="panel-header">Original Document</div>
          <div class="panel-content">
            <!-- PDF Viewer Component will go here -->
            <p>PDF Viewer Placeholder</p>
          </div>
        </div>
        
        <div class="panel translation-memory">
          <div class="panel-header">Translation Memory</div>
          <div class="panel-content">
            <!-- Translation Memory Component will go here -->
            <p>Translation Memory Placeholder</p>
          </div>
        </div>
        
        <div class="panel translated-preview">
          <div class="panel-header">Translated Preview</div>
          <div class="panel-content">
            <!-- Translated Preview Component will go here -->
            <p>Translated Preview Placeholder</p>
          </div>
        </div>
      </div>
    </main>
    
    <footer class="app-footer">
      <div class="status-indicator">
        <span :class="['status-dot', apiStatus]"></span>
        API Status: {{ apiStatusText }}
      </div>
      <div class="app-info">
        Side by Side Translator v{{ appVersion }}
      </div>
    </footer>
  </div>
</template>

<script>
export default {
  name: 'App',
  data() {
    return {
      currentDocument: null,
      apiStatus: 'unknown', // 'online', 'offline', 'unknown'
      appVersion: '0.1.0'
    }
  },
  computed: {
    apiStatusText() {
      const statusMap = {
        online: 'Connected',
        offline: 'Disconnected',
        unknown: 'Unknown'
      };
      return statusMap[this.apiStatus];
    }
  },
  methods: {
    async openFile() {
      try {
        const fileData = await window.api.openFile();
        if (fileData) {
          this.currentDocument = fileData;
          console.log('File opened:', fileData.name);
        }
      } catch (error) {
        console.error('Error opening file:', error);
      }
    },
    
    async saveTranslation() {
      if (!this.currentDocument) return;
      
      try {
        const options = {
          defaultPath: this.currentDocument.name.replace(/\.[^/.]+$/, '') + '_translated.pdf',
          fileContent: 'Placeholder for actual translated content'  // This would be actual content
        };
        
        const result = await window.api.saveFile(options);
        if (result.success) {
          console.log('File saved to:', result.path);
        } else {
          console.error('Failed to save file');
        }
      } catch (error) {
        console.error('Error saving file:', error);
      }
    },
    
    async checkApiStatus() {
      try {
        const result = await window.api.checkStatus();
        this.apiStatus = result.status;
      } catch (error) {
        this.apiStatus = 'offline';
        console.error('Error checking API status:', error);
      }
    }
  },
  mounted() {
    // Check API status on component mount
    this.checkApiStatus();
    
    // Get app version
    try {
      this.appVersion = window.api.getAppVersion() || '0.1.0';
    } catch (error) {
      console.error('Error getting app version:', error);
    }
  }
}
</script>

<style>
body {
  margin: 0;
  padding: 0;
  font-family: Arial, sans-serif;
}

.app-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.app-header {
  background-color: #2c3e50;
  color: white;
  padding: 10px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.toolbar {
  display: flex;
  gap: 10px;
}

.toolbar button {
  padding: 8px 12px;
  background-color: #3498db;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.toolbar button:hover {
  background-color: #2980b9;
}

.main-content {
  flex: 1;
  overflow: hidden;
  padding: 10px;
}

.welcome-screen {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100%;
  text-align: center;
}

.open-file-btn {
  margin-top: 20px;
  padding: 10px 20px;
  background-color: #2ecc71;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
}

.open-file-btn:hover {
  background-color: #27ae60;
}

.document-workspace {
  display: flex;
  height: 100%;
  gap: 10px;
}

.panel {
  flex: 1;
  border: 1px solid #ddd;
  border-radius: 4px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

.panel-header {
  background-color: #f5f5f5;
  padding: 10px;
  font-weight: bold;
  border-bottom: 1px solid #ddd;
}

.panel-content {
  flex: 1;
  overflow: auto;
  padding: 10px;
}

.app-footer {
  background-color: #f5f5f5;
  padding: 10px 20px;
  display: flex;
  justify-content: space-between;
  border-top: 1px solid #ddd;
}

.status-indicator {
  display: flex;
  align-items: center;
  gap: 5px;
}

.status-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  display: inline-block;
}

.status-dot.online {
  background-color: #2ecc71;
}

.status-dot.offline {
  background-color: #e74c3c;
}

.status-dot.unknown {
  background-color: #f1c40f;
}
</style> 