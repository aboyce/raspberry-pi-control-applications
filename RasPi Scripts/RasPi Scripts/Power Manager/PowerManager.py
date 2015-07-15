#************************************************************************************************************************************
#* If you are using arguments, they must be in the format [SYSTEM NAME ('S' or 'D')] and then [BUTTON TO PRESS('P' or 'R' or 'PH']  *
#************************************************************************************************************************************
# ADJUSTABLE VALUES

PROGRAM_NAME = "Power Manager 1.0"

# Will be changed later to ask other questions
MESSAGE_TO_ASK = "What do you want to control: "

BUTTON_PRESS_TIME = 1 #seconds
BUTTON_HOLD_TIME = 5 #seconds

# GPIO Ports
SERVER_POWER = 11;
SERVER_RESET = 15;
DESKTOP_POWER = 12;
DESKTOP_RESET = 16;

#************************************************************************************************************************************

import RPi.GPIO as GPIO
import sys
from time import sleep

GPIO.setmode(GPIO.BOARD)

GPIO.setup(SERVER_POWER, GPIO.OUT)
GPIO.setup(SERVER_RESET, GPIO.OUT)
GPIO.setup(DESKTOP_POWER, GPIO.OUT)
GPIO.setup(DESKTOP_RESET, GPIO.OUT)

machineToControl = ""
buttonToPress = ""
exit = False
usingArgs = False
errorCount = 0

# Check if we are using arguments or the user interface, then validate the arguments and set them.
if (len(sys.argv) > 1):
	usingArgs = True
	if (sys.argv[1] == "S" or sys.argv[1] == "D"):
		machineToControl = sys.argv[1]
	else: 
		exit = True
	if (sys.argv[2] == "P" or sys.argv[2] == "R" or sys.argv[2] == "PH"):
		buttonToPress = sys.argv[2]
	else:
		exit = True

if (usingArgs == False):
	print "\033[1m" + "Welcome to " + PROGRAM_NAME + "\033[0m"
	print ""

	# Find out which machine to control.
	while True:
		print "For Server -> " + "\033[1m" + "S" + "\033[0m"
		print "For Desktop -> " + "\033[1m" + "D" + "\033[0m"
		print "(To exit -> " + "\033[1m" + "exit" + "\033[0m" + ")"
		machineToControl = raw_input(MESSAGE_TO_ASK)
		if (machineToControl == "S" or machineToControl == "D"):
			break
		elif (machineToControl == "exit"):
			exit = True
			break
		print ""
		print "You have not entered a correct value!"
		print ""
	print ""

	# Changing message depending on machine selected.
	if (machineToControl == "S"):
		MESSAGE_TO_ASK = "What button on the Server do you want to press: "
	elif (machineToControl == "D"):
		MESSAGE_TO_ASK = "What button on the Server do you want to press: "
	else:
		errorCount += 1
	
	# Find out which button to press, or leave if exiting.
	if (exit == False):
		while True:
			print "For Power -> " + "\033[1m" + "P" + "\033[0m"
			print "For Reset -> " + "\033[1m" + "R" + "\033[0m"
			print "For Power Hold -> " + "\033[1m" + "PH" + "\033[0m"
			print "(To exit -> " + "\033[1m" + "exit" + "\033[0m" + ")"
			buttonToPress = raw_input("What button do you want to press: ")
			if (buttonToPress == "P" or buttonToPress == "R" or buttonToPress == "PH"):
				break
			elif (buttonToPress == "exit"):
				exit = True
				break
			print ""
			print "You have not entered a correct value!"
			print ""
		print ""
	
buttonPressingMessage = "Pressing button ..."
buttonReleaseMessage = "Button Pressed"

# Press the button on the machine, or leave if exiting/bad arguments.
if (exit == False):
	if (machineToControl == "S"):
		if (buttonToPress == "P"): # Server Power
			GPIO.output(SERVER_POWER, True)
			if(usingArgs == False):
				print buttonPressingMessage
			sleep(BUTTON_PRESS_TIME)
			GPIO.output(SERVER_POWER, False)
			if(usingArgs == False):
				print buttonReleaseMessage
		elif (buttonToPress == "R"): # Server Reset
			GPIO.output(SERVER_RESET, True)
			if(usingArgs == False):
				print buttonPressingMessage
			sleep(BUTTON_PRESS_TIME)
			GPIO.output(SERVER_RESET, False)
			if(usingArgs == False):
				print buttonReleaseMessage
		elif (buttonToPress == "PH"): # Server Power Hold
			GPIO.output(SERVER_POWER, True)
			if(usingArgs == False):
				print buttonPressingMessage
			sleep(BUTTON_HOLD_TIME)
			GPIO.output(SERVER_POWER, False)
			if(usingArgs == False):
				print buttonReleaseMessage
		else:
			errorCount += 1
			
	elif (machineToControl == "D"):
		if (buttonToPress == "P"): # Desktop Power
			GPIO.output(DESKTOP_POWER, True)
			if(usingArgs == False):
				print buttonPressingMessage
			sleep(BUTTON_PRESS_TIME)
			GPIO.output(DESKTOP_POWER, False)
			if(usingArgs == False):
				print buttonReleaseMessage
		elif (buttonToPress == "R"): # Desktop Reset
			GPIO.output(DESKTOP_RESET, True)
			if(usingArgs == False):
				print buttonPressingMessage
			sleep(BUTTON_PRESS_TIME)
			GPIO.output(DESKTOP_RESET, False)
			if(usingArgs == False):
				print buttonReleaseMessage
		elif (buttonToPress == "PH"): # Desktop Power Hold
			GPIO.output(DESKTOP_POWER, True)
			if(usingArgs == False):
				print buttonPressingMessage
			sleep(BUTTON_HOLD_TIME)
			GPIO.output(DESKTOP_POWER, False)
			if(usingArgs == False):
				print buttonReleaseMessage
		else:
			errorCount += 1
	else:
		errorCount += 1

# Checking up and closing
if (exit == False and usingArgs == False):
	if (errorCount != 0): # error occurred [should be impossible for an error to occur]
		print str(errorCount) + " error(s) occurred, please fix code!?."
	else:
		print ""
		print "Thank you for using " + PROGRAM_NAME
else: # Just exit the script
	if(usingArgs == False):
		print "Exiting ..."
	
GPIO.cleanup()
