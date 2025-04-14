FROM jbarlow83/ocrmypdf-alpine:latest

# Install additional system dependencies
RUN apk add --no-cache \
    python3-dev \
    py3-pip \
    build-base \
    jpeg-dev \
    libffi-dev \
    # Create virtual environment
    && python3 -m venv /opt/venv

# Set environment variables
ENV VIRTUAL_ENV=/opt/venv
ENV PATH="$VIRTUAL_ENV/bin:$PATH"

# Install Python packages
RUN pip3 install --no-cache-dir --upgrade pip && \
    pip3 install --no-cache-dir \
    pillow \
    requests \
    fastapi \
    uvicorn \
    python-multipart

# Create application directory
WORKDIR /app

# Copy the OCR processing script
COPY ocr_processing.py /app/ocr_processing.py

# Make the script executable
RUN chmod +x /app/ocr_processing.py

# Create directory for processing files
RUN mkdir -p /app/input /app/output

# Expose port for API
EXPOSE 8000

# Set entrypoint to the Python script using venv Python
ENTRYPOINT ["python3", "/app/ocr_processing.py"]