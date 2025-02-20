import os
from datetime import datetime
import json
import console_out_color as color

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
    composed_log = json.loads(log_string)
    return composed_log



def syslog():
    composed = f'[{TIMESTAMP}] '
    composed += '- '
    composed += f'{color.lightblue('SYS')} '
    composed += '- '
    return composed

def errlog():
    composed = f'[{TIMESTAMP}] '
    composed += '- '
    composed += f'{color.lightred('ERR')} '
    composed += '- '
    return composed

def compose_console_log(type,user,route):
    if type == 's':
        composed = syslog()
    elif type == 'e':
        composed = errlog()
    composed += f'{color.lightgreen('client_auth')} '
    composed += f'{user.get("user")}-{user.get("key")} '
    composed += '- - '
    composed += f'{color.lightgreen('route')} '
    composed += f'{route}'
    return composed

def compose_file_log(log):
    composed = ','
    json_component = json.dumps(log, indent=2, ensure_ascii=False)
    composed += json_component.replace('\n',"\n  ")
    composed += '\n]\n'
    return composed

def savelog_errauth(route):
    composed = errlog()
    composed += f'{color.lightgreen('client_auth')} '
    composed += f'{color.lightred('NO USER KEY AUTH ERR')} '
    composed += '- - '
    composed += f'{color.lightgreen('route')} '
    composed += f'{route}'
    print (composed)

def savelog_access(user,route):
    generated_log = compose_log(TIMESTAMP,user,route)

    if os.path.exists(FILE_PATH):
        with open(FILE_PATH, "r+") as file:
            file.seek(0, 2)
            pos = file.tell()
            file.seek(pos-5)
            file.write(compose_file_log(generated_log))
            user = json.loads(user)
            print (f'{compose_console_log('s',user,route)}')

    else:
        print("File does not exist.")