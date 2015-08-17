import RPi.GPIO as GPIO
import time
import sys

SLEEP_TIME = 0.5

# Relay to GPIO pin
RELAY_ONE = 5
RELAY_TWO = 6
RELAY_THREE = 13
RELAY_FOUR = 19
RELAY_FIVE = 26
RELAY_SIX = 16
RELAY_SEVEN = 20
RELAY_EIGHT = 21

lite_mode = False
GPIO.setmode(GPIO.BCM)
gpio_list = [RELAY_ONE, RELAY_TWO, RELAY_THREE, RELAY_FOUR, RELAY_FIVE, RELAY_SIX, RELAY_SEVEN, RELAY_EIGHT]

if len(sys.argv > 1):  # argv[0] is the file path
    if sys.argv[1] == '-lite':
        lite_mode = True

for p in gpio_list:
    GPIO.setup(p, GPIO.OUT)
    GPIO.output(p, GPIO.HIGH)

for p in gpio_list:
    GPIO.output(p, GPIO.LOW)
    if not lite_mode:
        print("Pin Number: " + str(p) + " turned on")
    time.sleep(SLEEP_TIME)

for p in gpio_list:
    GPIO.output(p, GPIO.HIGH)
    if not lite_mode:
        print("Pin Number: " + str(p) + " turned off")
    time.sleep(SLEEP_TIME)

print('Successful test')

GPIO.cleanup()
