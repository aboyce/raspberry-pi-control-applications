# ********************************************************************************************************************
# * If you are using arguments, they must be either;                                                                 *
# * -on to turn the fan on, or -off to turn the fan off                                                              *
# ********************************************************************************************************************

# FanControl.py version
VERSION_NUMBER = "0.1"

# The GPIO pin that will power the fan.
FAN_GPIO_PIN = 18


# For debugging, prints the message out in the format [time] - [script(version)] - [message]
# For consistency, 'message' should be either, 'Debug: MESSAGE', 'Info: MESSAGE', 'Warn: MESSAGE', 'Error: MESSAGE'.
# Example; Log('Error: no values provided')
def log(message):
    import time
    log_message = time.strftime("%H:%M:%S") + ' - FanControl(v' + VERSION_NUMBER + ') - ' + '[ ' + message + ' ]'
    print(log_message)
    return


# Checks to see if the argument in to turn the fan on.
def valid_arguments():
    log('Debug: valid_arguments()')
    import sys
    if len(sys.argv) > 1:
        if sys.argv[1] == '-on':
            log('Info: Read in to turn on')
            return True
        elif sys.argv[1] == '-off':
            log('Info: Read in to turn off')
            return False
        else:
            log('Info: Could not detect correct arguments')
            return None


def main():
    log('Info: FanControl v' + VERSION_NUMBER + ' started')
    log('Info: Fan on GPIO pin: ' + str(FAN_GPIO_PIN))

    turn_on = valid_arguments()

    if turn_on is None:
        return

    # We have a result from the arguments, lets do it.
    import RPi.GPIO as GPIO
    GPIO.setwarnings(False)  # Not ideal, but the only way I am able to leave the fan on, by not cleaning up the GPIO...
    GPIO.setmode(GPIO.BCM)
    GPIO.setup(FAN_GPIO_PIN, GPIO.OUT)

    if turn_on:
        log('Info: Fan on GPIO pin: ' + str(FAN_GPIO_PIN))
        log('Info: Turning fan on')
        GPIO.output(FAN_GPIO_PIN, GPIO.LOW)

    elif not turn_on:
        log('Info: Fan on GPIO pin: ' + str(FAN_GPIO_PIN))
        log('Info: Turning fan off')
        GPIO.output(FAN_GPIO_PIN, GPIO.HIGH)


if __name__ == "__main__":
    main()




