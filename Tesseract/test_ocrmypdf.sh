#!/bin/bash
# Script to test the OCRmyPDF container

# Make the script directory the current directory
cd "$(dirname "$0")"

# Create a sample_pdfs directory if it doesn't exist
mkdir -p sample_pdfs

# Check if a PDF file was provided as an argument
if [ $# -eq 0 ]; then
  echo "Usage: $0 path/to/pdf_file.pdf"
  echo "No PDF file provided. Will only build and run the container."
  TEST_FILE=""
else
  TEST_FILE="$1"
  # Copy the test file to the sample_pdfs directory
  cp "$TEST_FILE" sample_pdfs/
  TEST_FILENAME=$(basename "$TEST_FILE")
  echo "Will test with file: $TEST_FILENAME"
fi

# Build the Docker image
echo "Building Docker image..."
sudo docker build -t sidebyside-ocrmypdf .

# Run the container with port mapping and volume mounting
echo "Running container..."
sudo docker run -d -p 8000:8000 \
  -v "$(pwd)/sample_pdfs:/app/input" \
  --name ocrmypdf-test \
  sidebyside-ocrmypdf

echo "Container is running on http://localhost:8000"
echo "You can access the API documentation at http://localhost:8000/docs"

# If a test file was provided, test the API
if [ -n "$TEST_FILE" ]; then
  echo "Waiting for the container to start up..."
  sleep 3
  
  echo "Testing the API with $TEST_FILENAME..."
  RESPONSE=$(curl -s -X POST "http://localhost:8000/ocr/" \
    -H "accept: application/json" \
    -H "Content-Type: multipart/form-data" \
    -F "file=@sample_pdfs/$TEST_FILENAME" \
    -F "language=eng" \
    -F "optimize=1" \
    -F "skip_text=false")
  
  echo "Response from API:"
  echo "$RESPONSE"
  
  JOB_ID=$(echo $RESPONSE | grep -o '"job_id": "[^"]*' | cut -d'"' -f4)
  
  if [ -n "$JOB_ID" ]; then
    echo "Job ID: $JOB_ID"
    echo "You can check the status with:"
    echo "curl http://localhost:8000/status/$JOB_ID"
    echo "And download the processed file with:"
    echo "curl -o processed.pdf http://localhost:8000/download/$JOB_ID"
  fi
fi

echo ""
echo "To stop the container, run:"
echo "sudo docker stop ocrmypdf-test && sudo docker rm ocrmypdf-test" 