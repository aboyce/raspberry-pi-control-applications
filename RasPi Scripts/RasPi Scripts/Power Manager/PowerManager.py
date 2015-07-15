# ************************************************************************************************************************************
# * If you are using arguments, they must be in the format [SYSTEM NAME ('S' or 'D')] and then [BUTTON TO PRESS('P' or 'R' or 'PH']  *
# ************************************************************************************************************************************

import sys
import time

# PowerManager.py version
VERSION_NUMBER = "1.1"

BUTTON_PRESS_TIME = 1  # seconds
BUTTON_HOLD_TIME = 5  # seconds

# GPIO Ports
SERVER_POWER = 11
SERVER_RESET = 15
DESKTOP_POWER = 12
DESKTOP_RESET = 16

machine_to_control = ""
button_to_press = ""


# For debugging, prints the message out in the format [time] - [script(version)] - [message]
# For consistency, 'message' should be either, 'Debug: MESSAGE', 'Info: MESSAGE', 'Warn: MESSAGE', 'Error: MESSAGE'.
# Example; Log('Error: no values provided')
def log(message):
    log_message = time.strftime("%H:%M:%S") + ' - PowerManager(v' + VERSION_NUMBER + ') - ' + '[ ' + message + ' ]'
    print(log_message)
    return


def valid_arguments():
    log('Debug: valid_arguments')
    if len(sys.argv > 1):
        if sys.argv[1] == 'S' or sys.argv[1] == 'D':
            global machine_to_control
            machine_to_control = sys.argv[1]
            log('Info: setting machine to control to ' + machine_to_control)
        if sys.argv[2] == 'P' or sys.argv[2] == 'R' or sys.argv[2] == 'PH':
            global button_to_press
            button_to_press = sys.argv[2]
            log('Info: setting button to press to ' + button_to_press)
        return True
    else:
        return False


def user_interface():  # Will return False if the user wants to exit the application.
    log('Debug: user_interface')
    message_to_ask = "What do you want to control: "

    print "\033[1m" + "Power Manager v" + VERSION_NUMBER + "\033[0m"
    print ""

    while True:
        print "For Server -> " + "\033[1m" + "S" + "\033[0m"
        print "For Desktop -> " + "\033[1m" + "D" + "\033[0m"
        print "(To exit -> " + "\033[1m" + "exit" + "\033[0m" + ")"
        global machine_to_control
        machine_to_control = raw_input(message_to_ask)
        if machine_to_control == "S" or machine_to_control == "D":
            break
        elif machine_to_control == "exit":
            return False
        print ""
        print "Please enter a correct value!"
        print ""
    print ""

    if machine_to_control == "S":
        message_to_ask = "What button on the Server do you want to press: "
    elif machine_to_control == "D":
        message_to_ask = "What button on the Server do you want to press: "

    while True:
        print "For Power -> " + "\033[1m" + "P" + "\033[0m"
        print "For Reset -> " + "\033[1m" + "R" + "\033[0m"
        print "For Power Hold -> " + "\033[1m" + "PH" + "\033[0m"
        print "(To exit -> " + "\033[1m" + "exit" + "\033[0m" + ")"
        global button_to_press
        button_to_press = raw_input(message_to_ask)
        if button_to_press == "P" or button_to_press == "R" or button_to_press == "PH":
            break
        elif button_to_press == "exit":
            return False
        print ""
        print "Please enter a correct value!"
        print ""
    print ""

    return True


def press_button():
    log('Debug: press_button')
    import Rpi.GPIO as GPIO

    GPIO.setmode(GPIO.BOARD)

    GPIO.setup(SERVER_POWER, GPIO.OUT)
    GPIO.setup(SERVER_RESET, GPIO.OUT)
    GPIO.setup(DESKTOP_POWER, GPIO.OUT)
    GPIO.setup(DESKTOP_RESET, GPIO.OUT)
    log('Debug: set up GPIO')

    try:
        if machine_to_control == "S":
            if button_to_press == "P":  # Server Power
                GPIO.output(SERVER_POWER, True)
                log('Info: pressing server power button')
                time.sleep(BUTTON_PRESS_TIME)
                GPIO.output(SERVER_POWER, False)
                log('Info: button released')
            elif button_to_press == "R":  # Server Reset
                GPIO.output(SERVER_RESET, True)
                log('Info: pressing server reset button')
                time.sleep(BUTTON_PRESS_TIME)
                GPIO.output(SERVER_RESET, False)
                log('Info: button released')
            elif button_to_press == "PH":  # Server Power Hold
                GPIO.output(SERVER_POWER, True)
                log('Info: holding server power button')
                time.sleep(BUTTON_HOLD_TIME)
                GPIO.output(SERVER_POWER, False)
                log('Info: button released')
        else:
            log('Error: machine variables not set correctly')
            GPIO.cleanup()
            return

        if machine_to_control == "D":
            if button_to_press == "P":  # Desktop Power
                GPIO.output(DESKTOP_POWER, True)
                log('Info: pressing desktop power button')
                time.sleep(BUTTON_PRESS_TIME)
                GPIO.output(DESKTOP_POWER, False)
                log('Info: button released')
            elif button_to_press == "R":  # Desktop Reset
                GPIO.output(DESKTOP_RESET, True)
                log('Info: pressing desktop reset button')
                time.sleep(BUTTON_PRESS_TIME)
                GPIO.output(DESKTOP_RESET, False)
                log('Info: button released')
            elif button_to_press == "PH":  # Desktop Power Hold
                GPIO.output(DESKTOP_POWER, True)
                log('Info: holding desktop power button')
                time.sleep(BUTTON_HOLD_TIME)
                GPIO.output(DESKTOP_POWER, False)
                log('Info: button released')
        else:
            log('Error: desktop variables not set correctly')
            GPIO.cleanup()
            return
    # TODO: catch and log the error.
    finally:
        GPIO.cleanup()


def main():
    log('Info: started')

    if sys.argv > 1:
        log('Info: Arguments detected')
        if not valid_arguments():
            log('Info: stopping due to invalid arguments')
            return
    else:
        if not user_interface():
            log('Info: stopping due to user choice')
            return

    if not press_button():
        log('Info: stopping due to problem pressing the button')
        return

    log('Info: finished successfully')

# The Python interpreter checks that the module is running as the main program, if it is imported from another module
#  the '__name__' value will be that module's name.
# Apparently it is a convention to do this, and not really necessary for this project but is future proofing
#  and good practice.
if __name__ == "__main__":
    main()
