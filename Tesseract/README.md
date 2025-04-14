# OCRmyPDF Service for SideBySide Translator

This service provides OCR capabilities for the SideBySide Translator application using OCRmyPDF in a containerized FastAPI application.

## Features

- PDF OCR processing with OCRmyPDF
- REST API for file submission and retrieval
- Background processing with job status tracking
- Configurable optimization and language settings

## API Endpoints

The service provides the following REST API endpoints:

- `POST /ocr/` - Submit a PDF for OCR processing
- `GET /status/{job_id}` - Check the status of an OCR job
- `GET /download/{job_id}` - Download the processed PDF file
- `DELETE /job/{job_id}` - Delete a job and its associated files

## Getting Started

### Running with Docker

```bash
# Build the Docker image
docker build -t sidebyside-ocrmypdf .

# Run the container
docker run -p 8000:8000 -v /path/to/pdfs:/app/input sidebyside-ocrmypdf
```

### Using the Test Script

A test script is provided to simplify testing the OCR service:

```bash
# Make script executable (if needed)
chmod +x test_ocrmypdf.sh

# Run with a test PDF
./test_ocrmypdf.sh /path/to/your/file.pdf
```

## API Usage Examples

### Submit a PDF for OCR

```bash
curl -X POST "http://localhost:8000/ocr/" \
  -H "accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/path/to/your/file.pdf" \
  -F "language=eng" \
  -F "optimize=1" \
  -F "skip_text=false"
```

Response:
```json
{
  "job_id": "1234-5678-90ab-cdef",
  "status": "processing"
}
```

### Check Job Status

```bash
curl -X GET "http://localhost:8000/status/1234-5678-90ab-cdef"
```

Response:
```json
{
  "success": true,
  "output_file": "/app/output/1234-5678-90ab-cdef.pdf",
  "processing_time": 5.432,
  "stdout": "Added 3 pages",
  "stderr": ""
}
```

### Download Processed PDF

```bash
curl -X GET "http://localhost:8000/download/1234-5678-90ab-cdef" -o processed.pdf
```

### Delete a Job

```bash
curl -X DELETE "http://localhost:8000/job/1234-5678-90ab-cdef"
```

Response:
```json
{
  "job_id": "1234-5678-90ab-cdef",
  "status": "deleted"
}
```

## Configuration

The OCR service can be configured with the following parameters when submitting a job:

- `language` - OCR language code (default: "eng")
- `optimize` - Optimization level 0-3 (default: 1)
- `skip_text` - Whether to skip pages that already contain text (default: false)

## Integration with SideBySide Translator

This service is integrated with the SideBySide Translator application as follows:

1. The .NET API service communicates with this OCR service to process PDF files
2. Processed PDFs are used for text extraction and translation
3. The service is configured in the docker-compose.yml file

## Supported Languages

The default image includes the following languages:
- English (eng)
- German (deu)
- French (fra)
- Spanish (spa)
- Portuguese (por)
- Simplified Chinese (chi_sim)

To add additional languages, update the Dockerfile following the instructions in the [OCRmyPDF Docker documentation](https://ocrmypdf.readthedocs.io/en/latest/docker.html#adding-languages-to-the-docker-image). 