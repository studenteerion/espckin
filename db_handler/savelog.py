import os
from datetime import datetime
import json
from colorama import init, Fore

init()

FILE_PATH = "./log.json"


def savelog(user,route):
    timestamp = datetime.now().strftime('%Y/%m/%d - %H:%M:%S')

    new_data = {"timestamp": timestamp, "client_auth": user,"request_route": route}

    if os.path.exists(FILE_PATH):
        with open(FILE_PATH, "a") as file:
            file.write(json.dumps(new_data) + "\n")
            user = json.loads(user)
            print (f"[{timestamp}] - {Fore.LIGHTBLUE_EX}SYS{Fore.RESET} - {Fore.LIGHTGREEN_EX}client_auth{Fore.RESET} {user.get("user")}-{user.get("key")} - - {Fore.LIGHTGREEN_EX}route{Fore.RESET} {route}")

    else:
        print("File does not exist.")