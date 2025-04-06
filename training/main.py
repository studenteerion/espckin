#!pip install ultralytics

#!pip freeze > requirements.txt

'''import zipfile
with zipfile.ZipFile("dataset.zip", 'r') as zip_ref:
    zip_ref.extractall("dataset")'''

import os
from ultralytics import YOLO
import cv2

base_model = "yolo11n.pt"
trained_model = "./runs/detect/train4/weights/best.pt"
image = "macchina.jpg"

def train():
  model = YOLO("yolo11n.pt")

  dir = os.getcwd()
  data_path = os.path.join(dir,"dataset", "data.yaml")

  '''train_results = model.train(
      data = data_path,
      epochs = 100,
      #imgsz = 640
      device = "cuda"
    )'''

  train_results = model.train(
        data=data_path,         # Path to the dataset config
        epochs=300,             # Number of epochs to train the model
        batch=16,               # Batch size (adjust depending on your GPU capacity)
        imgsz=640,              # Image size (640 or 1280 depending on your dataset and GPU)
        device="cuda",          # Use CUDA (GPU) if available
        lr0=0.01,               # Initial learning rate for the optimizer
        lrf=0.01,               # Learning rate final value decay (optional)
        momentum=0.937,         # Momentum factor (common value)
        weight_decay=0.0005,    # Regularization to avoid overfitting
        rect=True,              # Rectangular training for more efficient batch processing
        flipud=0.5,             # Probability of flipping vertically
        fliplr=0.5,             # Probability of flipping horizontally
        hsv_h=0.015,            # HSV hue adjustment
        hsv_s=0.7,              # HSV saturation adjustment
        hsv_v=0.4,              # HSV value adjustment
        multi_scale=True,       # Multi-scale training
        scale=0.5,              # Random scaling of the images
        translate=0.1,          # Horizontal and vertical translations
        shear=0.1,              # Shear transformation
        degrees=10,             # Rotation (instead of "rotate")
        patience=50,            # Early stopping patience (if the model doesn't improve)
        save_period=1,          # Save the model checkpoint every epoch
        workers=4,              # Number of worker threads for loading the dataset
    )

  #print("training complete. Results:", train_results)


def detect(image):

  model = YOLO(trained_model)
  model.to("cuda")
  results = model(image)
  xyxy = results[0].boxes.xyxy.cpu().numpy()

  image = cv2.imread(image)

  i = 0
  for box in xyxy:
    print( i, box)
    x1, y1, x2, y2 = box
    cv2.rectangle(image, (int(x1), int(y1)), (int(x2), int(y2)), (0, 255, 0), 2)
    i+=1


  output_image_path = "output_image2.jpg"
  cv2.imwrite(output_image_path, image)


if __name__ == "__main__":
  #train()
  detect(image)