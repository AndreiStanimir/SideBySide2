import { resolve } from 'path'
import { defineConfig, externalizeDepsPlugin } from 'electron-vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  main: {
    build: {
      rollupOptions: {
        input: {
          index: 'electron.js'
        }
      }
    },
    plugins: [externalizeDepsPlugin()]
  },
  preload: {
    build: {
      rollupOptions: {
        input: {
          index: 'preload.js'
        }
      }
    },
    plugins: [externalizeDepsPlugin()]
  },
  renderer: {
    resolve: {
      alias: {
        '@': resolve('src/') // Assuming 'src' is the root for renderer code
      }
    },
    server: {
      port: 5173,
      strictPort: true, // Don't try alternative ports
      https: false, // Use HTTP instead of HTTPS to avoid SSL issues
      cors: false,
      hmr: {
        protocol: 'ws',
        host: 'localhost'
      },
      proxy: {},
      watch: {
        usePolling: true
      }
    },
    build: {
      rollupOptions: {
        input: {
          index: resolve('index.html') // Use the root index.html
        }
      }
    },
    plugins: [vue()],
    root: '.', // Set the root directory to the current directory
    base: './' // Use relative paths
  }
}) 