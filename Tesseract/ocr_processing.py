#!/usr/bin/env python3
"""
OCR Processing Service for SideBySide Translator Application.
This service processes PDF documents and images for text extraction.
"""

import os
import sys
import json
import time
import logging
from typing import Dict, List, Optional, Any

import cv2
import numpy as np
import pytesseract
from PIL import Image
from pdf2image import convert_from_path, convert_from_bytes

# Configure logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
    handlers=[logging.StreamHandler()]
)
logger = logging.getLogger('ocr-service')

class OCRProcessor:
    """OCR Processor for PDF and image documents."""
    
    def __init__(self, language: str = 'eng') -> None:
        """
        Initialize OCR processor.
        
        Args:
            language: Language code for OCR (default: English)
        """
        self.language = language
        logger.info(f"OCR Processor initialized with language: {language}")
    
    def process_pdf(self, pdf_path: str, dpi: int = 300) -> List[Dict[str, Any]]:
        """
        Process a PDF document for OCR.
        
        Args:
            pdf_path: Path to the PDF file
            dpi: DPI for PDF rendering (higher value means better quality but slower processing)
            
        Returns:
            List of dictionaries with OCR results per page
        """
        logger.info(f"Processing PDF: {pdf_path}")
        
        try:
            # Convert PDF to images
            pages = convert_from_path(pdf_path, dpi=dpi)
            
            results = []
            for i, page in enumerate(pages):
                logger.info(f"Processing page {i+1}/{len(pages)}")
                
                # Convert PIL Image to numpy array for OpenCV
                img_np = np.array(page)
                
                # Process the image
                ocr_result = self._process_image_array(img_np)
                
                results.append({
                    'page': i+1,
                    'text': ocr_result,
                    'width': page.width,
                    'height': page.height
                })
            
            return results
            
        except Exception as e:
            logger.error(f"Error processing PDF: {e}")
            return []
    
    def process_image(self, image_path: str) -> Dict[str, Any]:
        """
        Process an image file for OCR.
        
        Args:
            image_path: Path to the image file
            
        Returns:
            Dictionary with OCR results
        """
        logger.info(f"Processing image: {image_path}")
        
        try:
            # Read image with OpenCV
            img = cv2.imread(image_path)
            
            # Process the image
            ocr_result = self._process_image_array(img)
            
            return {
                'text': ocr_result,
                'width': img.shape[1],
                'height': img.shape[0]
            }
            
        except Exception as e:
            logger.error(f"Error processing image: {e}")
            return {'text': '', 'width': 0, 'height': 0}
    
    def _process_image_array(self, img_array: np.ndarray) -> str:
        """
        Process a numpy image array with OCR.
        
        Args:
            img_array: Numpy array representing the image
            
        Returns:
            Extracted text
        """
        # Convert to grayscale if it's a color image
        if len(img_array.shape) == 3:
            gray = cv2.cvtColor(img_array, cv2.COLOR_BGR2GRAY)
        else:
            gray = img_array
        
        # Apply some preprocessing to improve OCR accuracy
        # Threshold the image
        _, thresh = cv2.threshold(gray, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)
        
        # Noise removal
        kernel = np.ones((1, 1), np.uint8)
        opening = cv2.morphologyEx(thresh, cv2.MORPH_OPEN, kernel, iterations=1)
        
        # Perform OCR
        text = pytesseract.image_to_string(opening, lang=self.language)
        
        return text.strip()

def main():
    """Main function to start the OCR service."""
    logger.info("Starting OCR processing service")
    
    # This is a placeholder for future REST API or message queue integration
    # For now, this script can be used directly for processing files
    
    if len(sys.argv) > 1:
        file_path = sys.argv[1]
        language = sys.argv[2] if len(sys.argv) > 2 else 'eng'
        
        processor = OCRProcessor(language=language)
        
        if file_path.lower().endswith('.pdf'):
            results = processor.process_pdf(file_path)
            print(json.dumps(results, indent=2))
        elif file_path.lower().endswith(('.jpg', '.jpeg', '.png', '.tiff', '.tif', '.bmp')):
            result = processor.process_image(file_path)
            print(json.dumps(result, indent=2))
        else:
            logger.error(f"Unsupported file format: {file_path}")
    else:
        logger.info("No file specified. Service is ready for integration.")

if __name__ == "__main__":
    main() 