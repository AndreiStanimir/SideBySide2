/**
 * API Service for communicating with the backend
 */

const apiService = {
  /**
   * Check the API server status
   * @returns {Promise<Object>} Status object
   */
  async checkStatus() {
    return await window.api.checkStatus();
  },

  /**
   * Process a document with OCR
   * @param {ArrayBuffer} fileData - The file data to process
   * @param {String} fileName - The name of the file
   * @returns {Promise<Object>} OCR result
   */
  async processDocument(fileData, fileName) {
    return await window.api.request({
      method: 'post',
      endpoint: 'documents/process',
      data: {
        file: fileData,
        fileName: fileName
      }
    });
  },

  /**
   * Get translation memory matches
   * @param {String} text - The text to find matches for
   * @param {String} sourceLang - Source language code
   * @param {String} targetLang - Target language code
   * @returns {Promise<Object>} Translation memory matches
   */
  async getTranslationMemory(text, sourceLang = 'en', targetLang = 'fr') {
    return await window.api.request({
      method: 'get',
      endpoint: 'translation/memory',
      params: {
        text,
        sourceLang,
        targetLang
      }
    });
  },

  /**
   * Save a segment to translation memory
   * @param {Object} segment - Translation memory segment
   * @returns {Promise<Object>} Response
   */
  async saveToTranslationMemory(segment) {
    return await window.api.request({
      method: 'post',
      endpoint: 'translation/memory',
      data: segment
    });
  },

  /**
   * Get machine translation for text
   * @param {String} text - Text to translate
   * @param {String} sourceLang - Source language code
   * @param {String} targetLang - Target language code
   * @returns {Promise<Object>} Translation
   */
  async getMachineTranslation(text, sourceLang = 'en', targetLang = 'fr') {
    return await window.api.request({
      method: 'get',
      endpoint: 'translation/machine',
      params: {
        text,
        sourceLang,
        targetLang
      }
    });
  }
};

export default apiService; 