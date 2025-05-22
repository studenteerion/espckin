from paddleocr import PaddleOCR
import logging
import re
from message_handler import addMessage


# Set logging levels to suppress unnecessary output
logging.getLogger('ppocr').setLevel(logging.ERROR)
logging.getLogger('paddle').setLevel(logging.ERROR)

# Initialize PaddleOCR
ocr = PaddleOCR(use_angle_cls=True, lang='en')

min_plate_length = 3
max_plate_length = 8

def isPlate(text):
    if min_plate_length <= len(text) <= max_plate_length:
        return text

def read(cropped_image, camera, original_frame, bbox):
    """Function to perform OCR using PaddleOCR on cropped image."""
    result = ocr.ocr(cropped_image, cls=True)

    if not result or result[0] is None:
        # print("No text detected.")
        return

    # print(result)

    # Extract detected text
    detected_texts = [line[1][0] for line in result[0]]

    # Print detected texts
    for text in detected_texts:
        text = text.upper()
        text = re.sub(r'[^a-zA-Z0-9]', '', text)
        
        if isPlate(text):
            print("Detected text:", text)
            addMessage(text, camera, original_frame, bbox)
        
    #with open('detected_texts.txt', 'a') as file:
    #    for text in detected_texts:
    #        file.write(f"{text}\n")  # Write each detected text followed by a new line

    return detected_texts

async def detect(frame, camera, model):  # ASYNC
    """Function to detect objects in the image and perform OCR on cropped images."""
    
    #results = model(frame)
    results = model(frame, verbose=False) 

    if not results:
        print(f"No results for camera {camera['id']}")
        return

    # Get bounding boxes and class labels from the detection results
    xyxy = results[0].boxes.xyxy.cpu().numpy()  # Bounding box coordinates
    classes = results[0].boxes.cls.cpu().numpy()  # Class IDs
    image = frame  # Assuming frame is the original image
    cropped_images = []  # List to store cropped license plates

    # Loop over detected objects and crop only license plates
    for box, cls in zip(xyxy, classes):
        if cls == 0:  # Class ID 0 corresponds to "license-plate"
            x_min, y_min, x_max, y_max = map(int, box)
            cropped_image = image[y_min:y_max, x_min:x_max]
            cropped_images.append(cropped_image)

    # Process each cropped license plate
    if cropped_images:
        for cropped_image in cropped_images:
            read(cropped_image, camera, image, [x_min, y_min, x_max, y_max])  # Call the read function only for cropped license plates