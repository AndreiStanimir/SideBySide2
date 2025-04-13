FROM ubuntu:22.04

# Install Tesseract OCR and dependencies
RUN apt-get update && \
    apt-get install -y tesseract-ocr \
    tesseract-ocr-eng \
    tesseract-ocr-deu \
    tesseract-ocr-fra \
    tesseract-ocr-ita \
    tesseract-ocr-spa \
    tesseract-ocr-por \
    tesseract-ocr-rus \
    tesseract-ocr-jpn \
    tesseract-ocr-chi-sim \
    tesseract-ocr-chi-tra \
    python3 \
    python3-pip \
    python3-opencv \
    libtesseract-dev \
    libgl1-mesa-glx \
    libglib2.0-0 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Install Python packages for OCR processing
RUN pip3 install --no-cache-dir pytesseract pillow numpy pdf2image

# Create data directory for tesseract trained data
WORKDIR /usr/share/tesseract-ocr/4.00/tessdata

# Create application directory
WORKDIR /app

# Copy the OCR processing script
COPY ./ocr_processing.py .

# Set entrypoint to Python script
ENTRYPOINT ["python3", "ocr_processing.py"] 