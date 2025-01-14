from flask import Flask, request, jsonify
import cv2
import numpy as np
import easyocr
from ultralytics import YOLO
import re
import base64

# Function to check results (custom logic you defined)
def checkResult(detections):
    # Check if detections list is empty
    if not detections:
        return detections

    # If there's only one detection, return it directly
    if len(detections) == 1:
        return detections

    results = []

    # Logic to handle merging (modify this logic based on your needs)
    for i in range(len(detections) - 1):
        detection1 = detections[i]
        detection2 = detections[i + 1]

        # Check if the two detections are sufficiently close to be merged (based on your threshold)
        # Example: Use bounding box overlap or proximity to decide
        if detection1[3] > detection2[1] and detection1[2] > detection2[0]:  # Check if x1, y1 overlap with x2, y2
            # Here, I'm just merging detections without assuming they have 7 elements
            merged_detection = detection1 + detection2  # Customize this based on your needs
            results.append(merged_detection)
        else:
            # No merging needed, just append the detection as is
            results.append(detection1)

    # Return merged or original detections
    return results

def image_to_base64(image):
    try:
        _, buffer = cv2.imencode('.jpg', image)
        img_bytes = buffer.tobytes()
        return base64.b64encode(img_bytes).decode('utf-8')
    except Exception as e:
        raise ValueError("Error encoding image to base64: " + str(e))

# Initialize Flask app
app = Flask(__name__)

# Initialize EasyOCR and YOLO models
reader = easyocr.Reader(['en'])  # Initialize EasyOCR for text recognition (English language)
model = YOLO("plai.pt")  # Load the YOLO model for object detection

# API endpoint to process an image
@app.route('/process_image', methods=['POST'])
def process_image():
    if 'image' not in request.files:
        return jsonify({'error': 'No image found in request'}), 400

    file = request.files['image']
    if file.content_type not in ['image/jpeg', 'image/png']:
        return jsonify({'error': 'Invalid image format. Only JPEG and PNG are supported'}), 400

    try:
        file_bytes = file.read()
        np_array = np.frombuffer(file_bytes, np.uint8)
        frame = cv2.imdecode(np_array, cv2.IMREAD_COLOR)
    except Exception as e:
        return jsonify({'error': f'Error processing image: {str(e)}'}), 500

    ocr_results = []

    # Perform object detection using YOLO model
    results = model(frame)

    for result in results:
        for box in result.boxes:
            x1, y1, x2, y2 = map(int, box.xyxy[0].tolist())
            cropped_frame = frame[y1:y2, x1:x2]

            # Use EasyOCR to extract text from the cropped region
            ocr_result = reader.readtext(cropped_frame)

            for (bbox, text, prob) in ocr_result:
                cleaned_text = re.sub(r'[^a-zA-Z0-9]', '', text.upper())  # Uppercase and remove special characters
                ocr_results.append(cleaned_text)

                # Draw the bounding box on the original image
                cv2.rectangle(frame, (x1, y1), (x2, y2), (0, 255, 0), 2)

    # Use checkResult function on the OCR results list
    processed_results = checkResult(ocr_results)
    modified_image_base64 = image_to_base64(frame)

    return jsonify({
        'text': processed_results,  # List of extracted text from the image
        'image': modified_image_base64
    })

# Run the Flask app
if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5000)