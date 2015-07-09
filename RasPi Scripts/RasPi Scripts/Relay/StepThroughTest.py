import RPi.GPIO as GPIO
import time

SLEEP_TIME = 0.5

# Relay to GPIO pin
RELAY_ONE = 14
RELAY_TWO = 15
RELAY_THREE = 18
RELAY_FOUR = 23
RELAY_FIVE = 24
RELAY_SIX = 25
RELAY_SEVEN = 8
RELAY_EIGHT = 7

GPIO.setmode(GPIO.BCM)

gpio_list = [RELAY_ONE, RELAY_TWO, RELAY_THREE, RELAY_FOUR, RELAY_FIVE, RELAY_SIX, RELAY_SEVEN, RELAY_EIGHT]

for p in gpio_list:
    GPIO.setup(p, GPIO.OUT)
    GPIO.output(p, GPIO.HIGH)

for p in gpio_list:
    GPIO.output(p, GPIO.LOW)
    print("Pin Number: " + str(p) + " turned on")
    time.sleep(SLEEP_TIME)

for p in gpio_list:
    GPIO.output(p, GPIO.HIGH)
    print("Pin Number: " + str(p) + " turned off")
    time.sleep(SLEEP_TIME)

GPIO.cleanup()
