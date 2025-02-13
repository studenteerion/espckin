import os
from datetime import datetime
import json
from colorama import init, Fore

init()

FILE_PATH = "./log.json"

DAY = datetime.now().strftime('%d')
MONTH = datetime.now().strftime('%m')
YEAR = datetime.now().strftime('%Y')
HOUR = datetime.now().strftime('%H')
MIN = datetime.now().strftime('%M')
SEC = datetime.now().strftime('%S')

TIMESTAMP = f'{YEAR}/{MONTH}/{DAY} - {HOUR}:{MIN}:{SEC}'

def compose_log(time,user,route):
    log_string = "{"
    log_string += f'"timestamp": "{time}", '
    log_string += f'"client_auth": {user}, '
    log_string += f'"request_route": "{route}"'
    log_string += "}"
    composed = json.loads(log_string)
    return composed

def write_lightblue(text):
    write = f'{Fore.LIGHTBLUE_EX}'
    write += f'{text}'
    write += f'{Fore.RESET}'
    return write

def write_lightgreen(text):
    write = f'{Fore.LIGHTGREEN_EX}'
    write += f'{text}'
    write += f'{Fore.RESET}'
    return write

def syslog():
    composed = f'[{TIMESTAMP}] '
    composed += '- '
    composed += f'{write_lightblue('SYS')} '
    composed += '- '
    return composed

def compose_console_log(type,user,route):
    if type == 's':
        composed = syslog()
    composed += f'{write_lightgreen('client_auth')} '
    composed += f'{user.get("user")}-{user.get("key")} '
    composed += '- - '
    composed += f'{write_lightgreen('route')} '
    composed += f'{route}'
    return composed

def compose_file_log(log):
    composed = ','
    json_component = json.dumps(log, indent=2, ensure_ascii=False)
    composed += json_component.replace('\n',"\n  ")
    composed += '\n]\n'
    return composed

def savelog(user,route):
    generated_log = compose_log(TIMESTAMP,user,route)

    if os.path.exists(FILE_PATH):
        with open(FILE_PATH, "r+") as file:
            file.seek(0, 2)
            pos = file.tell()
            file.seek(pos-4)
            file.write(compose_file_log(generated_log))
            user = json.loads(user)
            print (f'{compose_console_log('s',user,route)}')

    else:
        print("File does not exist.")