FROM jbarlow83/ocrmypdf:latest

# Install additional system dependencies
RUN apt-get update && apt-get install -y \
    python3-dev \
    python3-pip \
    python3-venv \
    build-essential \
    libjpeg-dev \
    libffi-dev \
    wget \
    # Clean up to reduce image size
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Create and setup virtual environment
RUN python3 -m venv /opt/venv
ENV VIRTUAL_ENV=/opt/venv
ENV PATH="$VIRTUAL_ENV/bin:$PATH"

# Install Python packages in the virtual environment
RUN /opt/venv/bin/pip install --no-cache-dir --upgrade pip && \
    /opt/venv/bin/pip install --no-cache-dir \
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

# Verify ocrmypdf is available and show its location
RUN which ocrmypdf

# Expose port for API
EXPOSE 8000

# Set entrypoint to the Python script using venv Python
ENTRYPOINT ["/opt/venv/bin/python", "/app/ocr_processing.py"]