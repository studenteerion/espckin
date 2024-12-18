import cv2
import numpy as np
import time
import easyocr
from ultralytics import YOLO
from cv2 import dnn_superres
from flask import Flask, request, jsonify
import io

# Initialize Flask app
app = Flask(__name__)

# Initialize EasyOCR and YOLO models
reader = easyocr.Reader(['en'])
model = YOLO("plai.pt")

# Initialize Super-Resolution model
sr = dnn_superres.DnnSuperResImpl_create()
model_path = "EDSR_x3.pb"
sr.readModel(model_path)
sr.setModel("edsr", 3)

# API endpoint to process an image
@app.route('/process_image', methods=['POST'])
def process_image():
    # Check if the request contains the image in the body
    if not request.data:
        return jsonify({'error': 'No image data in the request body'}), 400

    # Get the raw image data from the request body
    img_bytes = request.data

    # Convert the raw bytes into a NumPy array and then decode it into an image
    try:
        np_array = np.frombuffer(img_bytes, np.uint8)
        frame = cv2.imdecode(np_array, cv2.IMREAD_COLOR)
    except Exception as e:
        return jsonify({'error': f'Error processing image: {str(e)}'}), 500

    # Initialize counters for correct and incorrect results
    right_found = 0
    wrong_ones = 0

    # Perform inference on the uploaded frame using the YOLO model
    results = model(frame)

    # Iterate over the detection results
    for result in results:
        for box in result.boxes:
            x1, y1, x2, y2 = map(int, box.xyxy[0].tolist())
            cropped_frame = frame[y1:y2, x1:x2]

            # Apply Super-Resolution (EDSR) to upscale the cropped image
            upscaled_frame = sr.upsample(cropped_frame)

            # Use EasyOCR to extract text from the upscaled image
            ocr_result = reader.readtext(upscaled_frame)

            # Loop through the OCR results
            for (bbox, text, prob) in ocr_result:
                # Normalize text: convert to uppercase and remove spaces
                text = text.upper().replace(" ", "")

                # Check if the extracted text matches the target
                if text == "AA123AA":
                    right_found += 1
                else:
                    wrong_ones += 1

    # Return the result counts as a JSON response
    return jsonify({
        'correct_count': right_found,
        'incorrect_count': wrong_ones
    })

# Run the Flask app
if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5000)