import { defineStore } from 'pinia'
import apiService from '../services/api'

export const useDocumentStore = defineStore({
  id: 'document',
  
  state: () => ({
    currentDocument: null,
    documentPages: [],
    currentPage: 1,
    isProcessing: false,
    error: null,
    translationMemory: [],
    translatedContent: {}
  }),
  
  getters: {
    isDocumentLoaded: (state) => !!state.currentDocument,
    currentPageContent: (state) => state.documentPages[state.currentPage - 1] || null,
    pageCount: (state) => state.documentPages.length,
    translationForCurrentPage: (state) => {
      return state.translatedContent[state.currentPage] || '';
    }
  },
  
  actions: {
    async loadDocument(fileData) {
      this.isProcessing = true;
      this.error = null;
      
      try {
        this.currentDocument = fileData;
        
        // Process document with OCR if it's a PDF or image
        if (['.pdf', '.jpg', '.jpeg', '.png', '.tiff', '.bmp'].includes(fileData.extension)) {
          const response = await apiService.processDocument(fileData.data, fileData.name);
          
          if (response.success) {
            this.documentPages = response.data.pages;
          } else {
            throw new Error(response.error || 'Failed to process document');
          }
        } else {
          // Handle text documents directly
          this.documentPages = [{
            text: new TextDecoder('utf-8').decode(fileData.data),
            page: 1
          }];
        }
        
        this.currentPage = 1;
      } catch (error) {
        this.error = error.message || 'An error occurred while loading the document';
        console.error('Document loading error:', error);
      } finally {
        this.isProcessing = false;
      }
    },
    
    setCurrentPage(pageNumber) {
      if (pageNumber > 0 && pageNumber <= this.pageCount) {
        this.currentPage = pageNumber;
      }
    },
    
    async getTranslationMemoryMatches(text, sourceLang = 'en', targetLang = 'fr') {
      try {
        const response = await apiService.getTranslationMemory(text, sourceLang, targetLang);
        
        if (response.success) {
          this.translationMemory = response.data.matches;
          return response.data.matches;
        } else {
          console.error('Failed to get translation memory:', response.error);
          return [];
        }
      } catch (error) {
        console.error('Translation memory error:', error);
        return [];
      }
    },
    
    async translateTextWithMT(text, sourceLang = 'en', targetLang = 'fr') {
      try {
        const response = await apiService.getMachineTranslation(text, sourceLang, targetLang);
        
        if (response.success) {
          return response.data.translation;
        } else {
          console.error('Machine translation error:', response.error);
          return null;
        }
      } catch (error) {
        console.error('Machine translation error:', error);
        return null;
      }
    },
    
    updateTranslation(pageNumber, translation) {
      this.translatedContent = {
        ...this.translatedContent,
        [pageNumber]: translation
      };
    },
    
    clearDocument() {
      this.currentDocument = null;
      this.documentPages = [];
      this.currentPage = 1;
      this.translationMemory = [];
      this.translatedContent = {};
      this.error = null;
    }
  }
}) 