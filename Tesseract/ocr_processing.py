#!/usr/bin/env python3
"""
OCR Processing Service for SideBySide Translator Application.
This service processes PDF documents using ocrmypdf.
"""

import os
import sys
import json
import time
import logging
import shutil
import uuid
import subprocess
from typing import Dict, List, Optional, Any
from pathlib import Path

# Debug paths and environment
print("Python executable:", sys.executable)
print("PATH:", os.environ.get('PATH', ''))
print("Current directory:", os.getcwd())
print("Directory contents:", os.listdir())

# Try to locate ocrmypdf executable
try:
    ocrmypdf_path = subprocess.check_output(["which", "ocrmypdf"], text=True).strip()
    print("ocrmypdf found at:", ocrmypdf_path)
except subprocess.CalledProcessError:
    print("ERROR: ocrmypdf not found in PATH")
    # Try to find it in common locations
    for path in ["/usr/bin/ocrmypdf", "/usr/local/bin/ocrmypdf", "/opt/bin/ocrmypdf"]:
        if os.path.exists(path):
            print(f"Found ocrmypdf at {path}")
            ocrmypdf_path = path
            break
    else:
        print("ERROR: Could not find ocrmypdf in any standard location")
        ocrmypdf_path = "ocrmypdf"  # Default, will likely fail if not in PATH

import uvicorn
from fastapi import FastAPI, File, UploadFile, HTTPException, BackgroundTasks, Query
from fastapi.responses import FileResponse, JSONResponse

# Configure logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
    handlers=[logging.StreamHandler()]
)
logger = logging.getLogger('ocrmypdf-service')

# Initialize FastAPI app
app = FastAPI(title="OCRmyPDF Service", description="OCR service for the SideBySide Translator Application")

# Define input and output directories
INPUT_DIR = Path("/app/input")
OUTPUT_DIR = Path("/app/output")

class OCRProcessor:
    """OCR Processor for PDF documents using ocrmypdf."""
    
    def __init__(self) -> None:
        """Initialize OCR processor."""
        logger.info("OCR Processor initialized")
        
        # Ensure directories exist
        INPUT_DIR.mkdir(exist_ok=True)
        OUTPUT_DIR.mkdir(exist_ok=True)
        
        # Store the path to ocrmypdf
        self.ocrmypdf_path = ocrmypdf_path
    
    def process_pdf(self, input_file: Path, output_file: Path, language: str = 'eng', 
                    optimize: int = 1, skip_text: bool = False) -> Dict[str, Any]:
        """
        Process a PDF document for OCR using ocrmypdf.
        
        Args:
            input_file: Path to the input PDF file
            output_file: Path where the OCR'd PDF will be saved
            language: Language code for OCR (default: English)
            optimize: Optimization level (0-3)
            skip_text: Whether to skip pages that already contain text
            
        Returns:
            Dictionary with OCR results
        """
        logger.info(f"Processing PDF: {input_file} to {output_file}")
        
        try:
            # Build command
            cmd = [
                self.ocrmypdf_path,
                f"--language", language,
                f"--optimize", str(optimize),
            ]
            
            if skip_text:
                cmd.append("--skip-text")
                
            # Add input and output files
            cmd.extend([str(input_file), str(output_file)])
            
            # Call ocrmypdf
            logger.info(f"Running command: {' '.join(cmd)}")
            start_time = time.time()
            result = subprocess.run(cmd, capture_output=True, text=True)
            processing_time = time.time() - start_time
            
            # Check result
            if result.returncode != 0:
                error_msg = result.stderr.strip()
                logger.error(f"Error processing PDF: {error_msg}")
                return {
                    "success": False,
                    "error": error_msg,
                    "processing_time": processing_time
                }
                
            return {
                "success": True,
                "output_file": str(output_file),
                "processing_time": processing_time,
                "stdout": result.stdout.strip(),
                "stderr": result.stderr.strip()
            }
            
        except Exception as e:
            logger.error(f"Exception processing PDF: {e}")
            return {
                "success": False,
                "error": str(e)
            }

# Initialize OCR processor
ocr_processor = OCRProcessor()

@app.get("/", summary="API Root")
async def root():
    """Root endpoint to verify the API is running."""
    return {"status": "ok", "service": "OCRmyPDF API", "ocrmypdf_path": ocrmypdf_path}

@app.post("/ocr/", summary="Process a PDF file with OCR")
async def process_pdf(
    background_tasks: BackgroundTasks,
    file: UploadFile = File(...),
    language: str = Query("eng", description="OCR language code"),
    optimize: int = Query(1, ge=0, le=3, description="Optimization level (0-3)"),
    skip_text: bool = Query(False, description="Skip pages that already contain text")
):
    """
    Process a PDF file with OCR and return a unique job ID.
    The processing happens in the background.
    """
    if not file.filename.lower().endswith('.pdf'):
        raise HTTPException(status_code=400, detail="Only PDF files are supported")
    
    # Generate a unique job ID
    job_id = str(uuid.uuid4())
    
    # Save the uploaded file
    input_path = INPUT_DIR / f"{job_id}.pdf"
    output_path = OUTPUT_DIR / f"{job_id}.pdf"
    
    with open(input_path, "wb") as buffer:
        shutil.copyfileobj(file.file, buffer)
    
    # Process PDF in background
    background_tasks.add_task(
        process_pdf_task, 
        job_id=job_id,
        input_path=input_path,
        output_path=output_path,
        language=language,
        optimize=optimize,
        skip_text=skip_text
    )
    
    return {"job_id": job_id, "status": "processing"}

async def process_pdf_task(job_id: str, input_path: Path, output_path: Path, 
                          language: str, optimize: int, skip_text: bool):
    """Background task to process PDF files"""
    logger.info(f"Starting background processing for job {job_id}")
    
    result = ocr_processor.process_pdf(
        input_file=input_path,
        output_file=output_path,
        language=language,
        optimize=optimize,
        skip_text=skip_text
    )
    
    # Save result to a JSON file for later retrieval
    result_path = OUTPUT_DIR / f"{job_id}.json"
    with open(result_path, "w") as f:
        json.dump(result, f)
    
    logger.info(f"Completed processing for job {job_id}")

@app.get("/status/{job_id}", summary="Check the status of an OCR job")
async def check_status(job_id: str):
    """Check the status of an OCR job by its ID"""
    output_path = OUTPUT_DIR / f"{job_id}.pdf"
    result_path = OUTPUT_DIR / f"{job_id}.json"
    
    if result_path.exists():
        # Job is completed, return results
        with open(result_path, "r") as f:
            result = json.load(f)
        return result
    
    if (INPUT_DIR / f"{job_id}.pdf").exists():
        # Job is in progress
        return {"job_id": job_id, "status": "processing"}
    
    # Job not found
    raise HTTPException(status_code=404, detail=f"Job {job_id} not found")

@app.get("/download/{job_id}", summary="Download a processed PDF file")
async def download_pdf(job_id: str):
    """Download a processed PDF file by job ID"""
    output_path = OUTPUT_DIR / f"{job_id}.pdf"
    result_path = OUTPUT_DIR / f"{job_id}.json"
    
    if not output_path.exists() or not result_path.exists():
        raise HTTPException(status_code=404, detail=f"Processed file for job {job_id} not found")
    
    return FileResponse(
        path=output_path,
        filename=f"processed_{job_id}.pdf",
        media_type="application/pdf"
    )

@app.delete("/job/{job_id}", summary="Delete a job and its associated files")
async def delete_job(job_id: str):
    """Delete a job and its associated files"""
    input_path = INPUT_DIR / f"{job_id}.pdf"
    output_path = OUTPUT_DIR / f"{job_id}.pdf"
    result_path = OUTPUT_DIR / f"{job_id}.json"
    
    deleted = False
    
    for path in [input_path, output_path, result_path]:
        if path.exists():
            path.unlink()
            deleted = True
    
    if not deleted:
        raise HTTPException(status_code=404, detail=f"Job {job_id} not found")
    
    return {"job_id": job_id, "status": "deleted"}

def main():
    """Main function to start the OCR service."""
    logger.info("Starting OCRmyPDF processing service")
    
    # Check if ocrmypdf is available
    try:
        result = subprocess.run([ocrmypdf_path, "--version"], capture_output=True, text=True)
        logger.info(f"OCRmyPDF version: {result.stdout.strip()}")
    except Exception as e:
        logger.error(f"Error checking OCRmyPDF installation: {e}")
        logger.error("Will try to continue anyway...")
    
    # Start the FastAPI server
    uvicorn.run(app, host="0.0.0.0", port=8000)

if __name__ == "__main__":
    main() 