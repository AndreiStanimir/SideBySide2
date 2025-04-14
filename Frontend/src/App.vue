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
    
    <!-- API Notification -->
    <div v-if="showApiNotification" class="api-notification">
      API Connected Successfully!
    </div>
    
    <main class="main-content">
      <div v-if="!currentDocument" class="welcome-screen">
        <h2>Welcome to Side by Side Translator</h2>
        <p>Open a document to get started with translation</p>
        <button @click="openFile" class="open-file-btn">Open Document</button>
      </div>
      
      <div v-else class="document-workspace">
        <div class="panel pdf-viewer">
          <div class="panel-header">
            <span>Original Document</span>
            <div class="panel-controls">
              <button title="Zoom In">+</button>
              <button title="Zoom Out">-</button>
              <button title="Next Page">→</button>
              <button title="Previous Page">←</button>
            </div>
          </div>
          <div class="panel-content">
            <!-- PDF Viewer Component will go here -->
            <p>{{ currentDocument.name }}</p>
            <div class="pdf-placeholder">PDF Viewer</div>
          </div>
        </div>
        
        <div class="panel translation-memory">
          <div class="panel-header">
            <span>Translation Memory</span>
            <div class="panel-controls">
              <button title="Search">🔍</button>
              <button title="Filter">⚙️</button>
            </div>
          </div>
          <div class="panel-content">
            <div class="search-box">
              <input type="text" placeholder="Search translation memory..." />
              <button>Search</button>
            </div>
            <div class="segment-list">
              <div class="segment-item">
                <div class="source-text">Example source text</div>
                <div class="target-text">Example target text</div>
                <div class="match-percent">98%</div>
              </div>
              <div class="segment-item">
                <div class="source-text">Another source text example</div>
                <div class="target-text">Another target text example</div>
                <div class="match-percent">85%</div>
              </div>
            </div>
          </div>
        </div>
        
        <div class="panel translated-preview">
          <div class="panel-header">
            <span>Translated Preview</span>
            <div class="panel-controls">
              <button title="Edit Mode">✏️</button>
              <button title="Preview Mode">👁️</button>
              <button title="Export">📥</button>
            </div>
          </div>
          <div class="panel-content">
            <div class="editor-area">
              <textarea placeholder="Translated content will appear here..."></textarea>
            </div>
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
      appVersion: '0.1.0',
      showApiNotification: false
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
        console.log('API Status:', result);
        
        // Show a temporary notification about API status
        if (result.status === 'online') {
          this.showApiNotification = true;
          setTimeout(() => {
            this.showApiNotification = false;
          }, 3000);
        }
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
  background-color: white;
}

.panel-header {
  background-color: #f5f5f5;
  padding: 10px;
  font-weight: bold;
  border-bottom: 1px solid #ddd;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.panel-controls {
  display: flex;
  gap: 5px;
}

.panel-controls button {
  background-color: transparent;
  border: 1px solid #ddd;
  border-radius: 3px;
  width: 28px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.panel-controls button:hover {
  background-color: #eee;
}

.panel-content {
  flex: 1;
  overflow: auto;
  padding: 10px;
}

.search-box {
  margin-bottom: 10px;
  display: flex;
}

.search-box input {
  flex: 1;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px 0 0 4px;
}

.search-box button {
  padding: 8px 12px;
  background-color: #3498db;
  color: white;
  border: none;
  border-radius: 0 4px 4px 0;
  cursor: pointer;
}

.segment-list {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.segment-item {
  border: 1px solid #eee;
  border-radius: 4px;
  padding: 10px;
  cursor: pointer;
}

.segment-item:hover {
  background-color: #f9f9f9;
}

.source-text {
  font-weight: bold;
  margin-bottom: 5px;
}

.match-percent {
  color: #27ae60;
  font-size: 0.9em;
  text-align: right;
}

.editor-area {
  height: 100%;
}

.editor-area textarea {
  width: 100%;
  height: 100%;
  min-height: 300px;
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 10px;
  resize: none;
}

.pdf-placeholder {
  width: 100%;
  height: 400px;
  background-color: #f9f9f9;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.2em;
  color: #777;
  border: 1px dashed #ddd;
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
}

.status-dot.online {
  background-color: #2ecc71;
}

.status-dot.offline {
  background-color: #e74c3c;
}

.status-dot.unknown {
  background-color: #f39c12;
}

.app-info {
  font-size: 0.9em;
  color: #777;
}

.api-notification {
  position: fixed;
  top: 70px;
  right: 20px;
  background-color: #2ecc71;
  color: white;
  padding: 10px 20px;
  border-radius: 4px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  animation: fadeIn 0.3s, fadeOut 0.5s 2.5s;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(-20px); }
  to { opacity: 1; transform: translateY(0); }
}

@keyframes fadeOut {
  from { opacity: 1; transform: translateY(0); }
  to { opacity: 0; transform: translateY(-20px); }
}
</style> 