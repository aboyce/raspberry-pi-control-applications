# The script should be ran with arguments, they should be;
#   - the '-u' [url] to send the request
#   - the '-d' [door id] that should be opened if the credentials are valid
#   - the '-c' [card id] from the card that is requesting to open the door
#   - to enable lite mode '-lite', if not present will be by defaulted to off
# The script will;
#   - Check the arguments and extract them the best it can, the 'url' is checked to see if it is reachable.
#   - Send a request off to the 'url' with the 'door_id' and 'card_id', this should return True or False.
#   - If True is returned it will send a pulse to the assigned GPIO port.

# ********************************************************************************************************************
# ********************************************************************************************************************
# USER CHANGEABLE VARIABLES

# DoorManagement.py version
VERSION_NUMBER = '0.9'

TIME_TO_OPEN_DOOR = 2  # seconds

# Server door Ids that this script can control
DOOR_ID_ONE = '1'
DOOR_ID_TWO = '2'
DOOR_ID_THREE = '3'
DOOR_ID_FOUR = '4'

# GPIO Ports/Doors (The GPIO ports that are to be opened for each of the above door Ids)
DOOR_RELAY_ONE = 5
DOOR_RELAY_TWO = 6
DOOR_RELAY_THREE = 13
DOOR_RELAY_FOUR = 19

# ********************************************************************************************************************
# ********************************************************************************************************************

import sys
import time
import requests

door_to_gpio_dict = {DOOR_ID_ONE: DOOR_RELAY_ONE, DOOR_ID_TWO: DOOR_RELAY_TWO,
                     DOOR_ID_THREE: DOOR_RELAY_THREE, DOOR_ID_FOUR: DOOR_RELAY_FOUR}

# When calling the scripts remotely it is often not required to have detailed logging return to the user.
# With lite_mode it should only return a single string that the caller can then use to see if the script ran ok or not.
# By default it will log everything, the argument '-lite' will enable lite_mode.
lite_mode = False

# string url
url = None
# int door_id
door_id = None
# string card_id
card_id = None


# For debugging, prints the message out in the format [time] - [script(version)] - [message]
# For consistency, 'message' should be either, 'Debug: MESSAGE', 'Info: MESSAGE', 'Warn: MESSAGE', 'Error: MESSAGE'.
# By default will log the message, unless 'lite mode' is enabled where only one message should be printed. However if
#  the message is to be returned to the user you can use the bool to force that through even in lite mode.
# Example; Log('Error: no values provided')     Example; Log('Debug: Finished Script', True)
def log(message, log_on_lite_mode=False):
    if not lite_mode:  # If it is not lite mode, print the full log.
        log_message = \
            time.strftime("%H:%M:%S") + ' - DoorManagement(v' + VERSION_NUMBER + ') - ' + '[ ' + message + ' ]'
        print(log_message)
    else:  # If it is lite mode.
        if log_on_lite_mode:  # Check if we are overriding it.
            print(message)


# Tries to populate 'url', 'door_id', 'card_id' and '-lite' variables.
# Returns: True if it could extract values. False if not.
def valid_arguments():

    length_of_args = len(sys.argv)  # Just to stop calling len() repeatedly.

    if length_of_args > 1:  # args[0] is the file path so will always be there.
        is_value = None  # Used to distinguish if an unknown value is a part of a pair (i.e. -u and 8.8.8.8).

        # To detect lite mode, we have to do a separate pass, as it could be at the end of the arguments and we may
        #   have already done some logging before reaching the -lite.
        for arg_index in range(length_of_args):  # Work our way through all of the arguments.
            if sys.argv[arg_index] == '-lite':  # Detect if it is lite mode
                global lite_mode
                lite_mode = True
                break  # As soon as we reach it, exit the loop as that is all we needed.

        for arg_index in range(length_of_args):  # Work our way through all of the arguments, again...

            if arg_index == 0:  # We don't need to try the first argument as it is the file path, so move on.
                continue

            if sys.argv[arg_index] == 'help':  # If the user requested help, just log it and no point continuing.
                log('REQUIRED ARGS (-u -d -c): \'url\' \'door_id\' \'card_id\'')
                log('Info: Requested help')
                return False

            elif sys.argv[arg_index] == '-u':  # Try to get the url.
                if (arg_index + 1) < length_of_args:  # Prevent an out of bounds error, in case there is no value.
                    log('Info: url is present')
                    # Check to see if we can contact the url.
                    import subprocess
                    proc = subprocess.Popen(['ping', '-c', '1', sys.argv[arg_index + 1]], stdout=subprocess.PIPE)
                    stdout, stderr = proc.communicate()
                    if proc.returncode == 0:
                        log('Info: server responded')
                        global url
                        url = str(sys.argv[arg_index + 1])
                        log('Info: url = ' + url)
                        is_value = arg_index + 1  # So that we know the next value is the one we have just used.
                        continue  # We have the value, there is no point continuing with this pass.
                    else:
                        log('Error: the server is unavailable')
                        return False
                else:
                    log('Error: detected the url but the value is out of the bounds of the array')
                    return False

            elif sys.argv[arg_index] == '-d':  # Try to get the door_id.
                if (arg_index + 1) < length_of_args:  # Prevent an out of bounds error, in case there is no value.
                    log('Info: door_id is present')
                    global door_id
                    door_id = sys.argv[arg_index + 1]
                    log('Info: door_id = ' + door_id)
                    if door_id not in door_to_gpio_dict:  # See if the door_id is one we can actually open.
                        log('Error: the door_id ' + door_id + ' is not managed by this device')
                        return False
                    is_value = arg_index + 1  # So that we know the next value is the one we have just used.
                    continue  # We have the value, there is no point continuing with this pass.
                else:
                    log('Error: detected the door_id but the value is out of the bounds of the array')
                    return False

            elif sys.argv[arg_index] == '-c':  # Try to get the card_id
                if (arg_index + 1) < length_of_args:   # Prevent an out of bounds error, in case there is no value.
                    log('Info: card_id is present')
                    global card_id
                    card_id = sys.argv[arg_index + 1]
                    log('Info: card_id = ' + card_id)
                    is_value = arg_index + 1  # So that we know the next value is the one we have just used.
                    continue  # We have the value, there is no point continuing with this pass.
                else:
                    log('Error: detected the card_id but the value is out of the bounds of the array')
                    return False

            else:
                if arg_index is not is_value and sys.argv[arg_index] != '-lite':
                    log('Error: unknown variable ' + str(sys.argv[arg_index]))
                    return False
    else:
        log('Error: no arguments provided')
        return False

    if url is None or door_id is None or card_id is None:
        log('Error: Not all of the required arguments have been provided')
        return False

    return True


def send_data():
    log('Debug: send_data(' + str(url) + ', ' + str(door_id) + ', ' + str(card_id) + ')')

    try:
        server_url = 'http://' + url + '/'
        session = requests.Session()
        response = session.post(url=server_url, data={'door_id': door_id, 'card_id': card_id})

        # The server should return 'True' if the request is valid and the door should be opened.
        server_response = response.text
    except:
        log('Error: exception caught when contacting server', True)
        return False

    log('Debug: server response = ' + server_response)
    if server_response == 'True':
        log('Info: credentials valid')
        return True
    else:
        log('Info: credentials failed', True)
        return False


def open_door():
    import os
    import RPi.GPIO as GPIO

    if os.name == 'nt':
        log('Warn: script running on a Windows OS, cannot open door as no GPIO')
        return False

    gpio_to_open = door_to_gpio_dict.get(door_id)
    log('Info: Door to open is ' + str(door_id) + ' on GPIO number ' + str(gpio_to_open))

    if gpio_to_open is None:
        log('Error: could not get GPIO for the door_id ' + door_id)
        return False

    GPIO.setmode(GPIO.BCM)
    GPIO.setup(gpio_to_open, GPIO.OUT)
    GPIO.output(gpio_to_open, GPIO.LOW)
    time.sleep(TIME_TO_OPEN_DOOR)
    GPIO.output(gpio_to_open, GPIO.HIGH)
    GPIO.cleanup()

    return True


def main():

    if not valid_arguments():
        log('Error: invalid arguments', True)
        return

    if not send_data():
        # Will have '-lite' logged within the method.
        return

    if not open_door():
        log('Error: not able to open the door', True)
        return

    log('Info: Finished successfully', True)


# The Python interpreter checks that the module is running as the main program, if it is imported from another module
#  the '__name__' value will be that module's name.
# Apparently it is a convention to do this, and not really necessary for this project but is future proofing
#  and good practice.
if __name__ == "__main__":
    main()
