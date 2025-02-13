import os
from datetime import datetime
import json
from colorama import init, Fore

init()

FILE_PATH = "./log.json"
day = datetime.now().strftime('%d')
month = datetime.now().strftime('%m')
year = datetime.now().strftime('%Y')
hour = datetime.now().strftime('%H')
min = datetime.now().strftime('%M')
sec = datetime.now().strftime('%S')

def syslog():
    composed = f"[{year}/{month}/{day} - {hour}:{min}:{sec}] - {Fore.LIGHTBLUE_EX}SYS{Fore.RESET} - "
    return composed

def savelog(user,route):
    timestamp = datetime.now().strftime('%Y/%m/%d - %H:%M:%S')
    user_str = "{"+f' "user": "{json.loads(user).get("user")}", "key": "{json.loads(user).get("key")}" '+"}"
    new_data = f'\n    "timestamp": "{timestamp}",\n    "client_auth": {user_str},\n    "request_route": "{route}"'

    if os.path.exists(FILE_PATH):
        with open(FILE_PATH, "r+") as file:
            file.seek(0, 2)
            pos = file.tell()
            file.seek(pos-5)
            file.write(",\n  {"+new_data + "\n  }\n]\n")
            user = json.loads(user)
            print (f"{syslog()}{Fore.LIGHTGREEN_EX}client_auth{Fore.RESET} {user.get("user")}-{user.get("key")} - - {Fore.LIGHTGREEN_EX}route{Fore.RESET} {route}")

    else:
        print("File does not exist.")