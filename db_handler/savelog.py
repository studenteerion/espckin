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
    new_data = json.loads("{"+f'"timestamp": "{timestamp}", "client_auth": {user}, "request_route": "{route}"'+"}")

    if os.path.exists(FILE_PATH):
        with open(FILE_PATH, "r+") as file:
            file.seek(0, 2)
            pos = file.tell()
            file.seek(pos-4)
            file.write(","+json.dumps(new_data, indent=2, ensure_ascii=False)+"\n]\n")
            user = json.loads(user)
            print (f"{syslog()}{Fore.LIGHTGREEN_EX}client_auth{Fore.RESET} {user.get("user")}-{user.get("key")} - - {Fore.LIGHTGREEN_EX}route{Fore.RESET} {route}")

    else:
        print("File does not exist.")