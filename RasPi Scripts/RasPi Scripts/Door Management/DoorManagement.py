# The script should be ran with arguments (in the correct order), they should be;
#   - the 'url' to send the request
#   - the 'door_id' that should be opened if the credentials are valid
#   - the 'card_id' from the card that is requesting to open the door
# The script will;
#   - Check the arguments and extract them the best it can, the 'url' is checked to see if it is reachable.
#   - Send a request off to the 'url' with the 'door_id' and 'card_id', this should return True or False.
#   - If True is returned it will send a pulse to the assigned GPIO port.

import sys
import time
import requests

# DoorManagement.py version
VERSION_NUMBER = '0.3'

TIME_TO_OPEN_DOOR = 2  # seconds

# Server door IDs
DOOR_ID_ONE = None
DOOR_ID_TWO = None
DOOR_ID_THREE = None
DOOR_ID_FOUR = None

# GPIO Ports/Doors
DOOR_RELAY_ONE = None
DOOR_RELAY_TWO = None
DOOR_RELAY_THREE = None
DOOR_RELAY_FOUR = None

door_to_gpio_dict = {DOOR_ID_ONE: DOOR_RELAY_ONE, DOOR_ID_TWO: DOOR_RELAY_TWO,
                     DOOR_ID_THREE: DOOR_RELAY_THREE, DOOR_ID_FOUR: DOOR_RELAY_FOUR}

lite_mode = False

# string url
url = None
# int door_id
door_id = None
# string card_id
card_id = None


# For debugging, prints the message out in the format [time] - [script(version)] - [message]
# For consistency, 'message' should be either, 'Debug: MESSAGE', 'Info: MESSAGE', 'Warn: MESSAGE', 'Error: MESSAGE'.
# Example; Log('Error: no values provided')
def log(message):
    if not lite_mode:
        log_message = time.strftime("%H:%M:%S") + ' - DoorManagement(v' + VERSION_NUMBER + ') - ' + '[ ' + message + ' ]'
        print(log_message)
    return


# Tries to populate 'url', 'door_id' and 'card_id' variables.
# Returns: True if it could extract values. The error message if not.
def valid_arguments():
    length_of_args = len(sys.argv)

    # TODO: Re-write this so that they can be in any order and explicitly wrote out. e.g. -u bob.com -d 01 -c 882 -lite

    # The args[0] is the file path.
    if length_of_args > 1:
        if sys.argv[1] == 'help':
            log('REQUIRED ARGS (u d c): \'url\' \'door_id\' \'card_id\'')
            log('Info: Requested help')
            return False

        # Check we have all of the arguments.
        if length_of_args == 4 or length_of_args == 5:

            if sys.argv[4] == '-lite':
                global lite_mode
                lite_mode = True
            # Checking url (assume it is if it contains 'http').
            if 'http' in sys.argv[1]:
                log('Info: url is present')
                # Check to see if we get a code 200 from the url.
                if (requests.head(sys.argv[1])).status_code == 200:
                    log('Info: server responded')
                    global url
                    url = str(sys.argv[1])
                    log('Info: url = ' + url)
                else:
                    log('Error: the server is unavailable')
                    return False
            else:
                log('Error: the url is not present, please use full address (http://...)')
                return False

            # Checking door_id (assume it is there if it is an int)
            if sys.argv[2].isdigit():
                log('Info: door_id is present')
                global door_id
                door_id = sys.argv[2]
                log('Info: door_id = ' + door_id)
                if door_id not in door_to_gpio_dict:
                    log('Error: the door_id ' + door_id + ' is not managed by this device')
                    return False
            else:
                log('Error: the door_id is not an int')
                return False

            # Checking card_id (assume it is there if it is present, unsure on the Id format currently).
            if sys.argv[3] != "":  # TODO: Find a better way of doing this, currently there should always be a value
                log('Info: card_id is present')
                global card_id
                card_id = sys.argv[3]
                log('Info: card_id = ' + card_id)
            else:
                log('Error: the card_id is not present')
                return False
        else:
            log('Error: please provide correct number of arguments')
            return False
    return True


def send_data():
    log('Debug: send_data(' + str(url) + ', ' + str(door_id) + ', ' + str(card_id) + ')')

    try:
        session = requests.Session()
        response = session.post(
            url=url,
            data={'door_id': door_id, 'card_id': card_id},
        )
        # The server should return 'True' if the request is valid and the door should be opened.
        server_response = response.text
    except:
        log('Error: exception caught when contacting server')
        return False

    log('Debug: server response = ' + server_response)
    if server_response == 'True':
        log('Info: credentials valid')
        return True
    else:
        log('Info: credentials failed')
        return False


def open_door():
    import os
    import RPi.GPIO as GPIO

    if os.name == 'nt':
        log('Warn: script running on a Windows OS, cannot open door directly')
        return False

    gpio_to_open = door_to_gpio_dict.get(door_id, default=None)

    if gpio_to_open is None:
        log('Error: could not get GPIO for the door_id ' + door_id)
        return False

    GPIO.setmode(GPIO.BCM)
    GPIO.setup(gpio_to_open, GPIO.OUT)
    GPIO.output(gpio_to_open, GPIO.HIGH)
    time.sleep(TIME_TO_OPEN_DOOR)
    GPIO.output(gpio_to_open, GPIO.LOW)
    GPIO.cleanup()

    return True


def main():

    if not valid_arguments():
        log('Info: stopping due to invalid arguments')
        return

    if not send_data():
        log('Info: stopping due to being decline from server')
        return

    if not open_door():
        log('Info: stopping due to not being able to open the door')
        return

    log('Info: finished successfully')


# The Python interpreter checks that the module is running as the main program, if it is imported from another module
#  the '__name__' value will be that module's name.
# Apparently it is a convention to do this, and not really necessary for this project but is future proofing
#  and good practice.
if __name__ == "__main__":
    main()
