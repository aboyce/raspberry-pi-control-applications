import RPi.GPIO as GPIO
import time

SLEEP_TIME = 0.5

GPIO.setmode(GPIO.BCM)

pinList = [14, 15, 18, 23, 24, 25, 8, 7]

for p in pinList:
    GPIO.setup(p, GPIO.OUT)
    GPIO.output(p, GPIO.HIGH)


for p in pinList:
    GPIO.output(p, GPIO.LOW)
    print("Pin Number: " + str(p) + " turned on")
    time.sleep(SLEEP_TIME)

for p in pinList:
    GPIO.output(p, GPIO.HIGH)
    print("Pin Number: " + str(p) + " turned off")
    time.sleep(SLEEP_TIME)

GPIO.cleanup()