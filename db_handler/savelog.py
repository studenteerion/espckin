import os
from datetime import datetime
import json

FILE_PATH = "../log.json"


def savelog(user,route):
    timestamp = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    print(timestamp)

    new_data = {"timestamp": timestamp, "client_auth": user,"request_route": route}

    if os.path.exists(FILE_PATH):
        with open(FILE_PATH, "a") as file:
            file.write(json.dumps(new_data) + "\n")

        print("New data appended successfully!")

    else:
        print("File does not exist.")