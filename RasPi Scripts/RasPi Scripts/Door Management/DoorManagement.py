# The script should be ran with arguments (in the correct order), they should be;
#   - the 'url' to send the request
#   - the 'doorId' that should be opened if the credentials are valid
#   - the 'cardId' from the card that is requesting to open the door
# The script will;
#   - Check the arguments and extract them the best it can, the 'url' is checked to see if it is reachable.
#   - Send a request off to the 'url' with the 'doorId' and 'cardId', this should return True or False.
#   - If True is returned it will send a pulse to the assigned GPIO port.

import sys
import time
import requests

VERSION_NUMBER = '0.1'
URLTESTING = 'http://add.url.here'

#GPIO Ports
RELAY_ONE = 1
RELAY_TWO = 2


# string url
url = None
# int doorId
doorId = None
# string cardId
cardId = None

# For debugging, prints the message out in the format [time] - [script(version)] - [message]
# For consistency, 'message' should be either, 'Debug: MESSAGE', 'Info: MESSAGE', 'Warn: MESSAGE', 'Error: MESSAGE'. Example; Log('Error: no values provided')
def Log(message):
    logMessage = time.strftime("%H:%M:%S") + ' - HFRRasPi(v' + VERSION_NUMBER + ') - ' + '[ ' + message + ' ]'
    print(logMessage)
    return

# Tries to populate 'url', 'doorId' and 'cardId' variables.
# Returns: True if it could extract values. The error message if not.
def validArguments():
    lengthOfArgs = len(sys.argv)

    # The args[0] is the file path.
    if(lengthOfArgs > 1):
        if(sys.argv[1] == 'help'):
            Log('REQUIRED ARGS (u d c): \'url\' \'doorId\' \'cardId\'')
            Log('Info: Requested help')
            return False

            req = requests.head(URLTESTING)
            stat = req.status_code

        # Check we have all of the arguments.
        if(lengthOfArgs == 4):
            # Checking url (assume it is if it contains 'http').
            if('http' in sys.argv[1]):
                Log('Info: url is present')
                # Check to see if we get a code 200 from the url.
                if((requests.head(sys.argv[1])).status_code == 200):
                    Log('Info: server responded')
                    global url
                    url = str(sys.argv[1])
                    Log('Info: url = ' + url)
                else:
                    Log('Error: the server is unavailable')
                    return False
            else:
                Log('Error: the url is not present, please use full address (http://...)')
                return False
            # Checking doorId (assume it is there if it is an int)'
            if(sys.argv[2].isdigit()):
                Log('Info: doorId is present')
                global doorId
                doorId = sys.argv[2]
                Log('Info: doorId = ' + doorId)
            else:
                Log('Error: the doorId is not an int')
                return False
            # Checking cardId (assume it is there if it is present, unsure on the Id format currently).
            if(sys.argv[3] != ""):
                Log('Info: cardId is present')
                global cardId
                cardId = sys.argv[3]
                Log('Info: cardId = ' + cardId)
            else:
                Log('Error: the cardId is not present')
                return False
        else:
            Log('Error: please provide correct number of arguments')
            return False

    return True


def sendData():
    Log('Debug: sendData(' + url + ', ' + doorId + ', ' + cardId + ')')

    try:
        session = requests.Session()
        response = session.post(
            url = url,
            data = {'DoorId': doorId, 'CardId': cardId},
            )
        # The server should return 'True' if the request is valid and the door should be opened.
        serverResponse = response.text
    except:
        Log('Error: exception caught when contacting server')
        return False

    Log('Debug: server response = ' + serverResponse)
    if(serverResponse == 'True'):
        Log('Info: credentials valid')
        return True
    else:
        Log('Info: credentials failed')
        return False

def openDoor():
    import os
    import RPi.GPIO as GPIO

    if (os.name == 'nt'):
        Log('Warn: script running on a Windows OS, cannot open door directly')
        return False

    GPIO.setmode(GPIO.BOARD)



    GPIO.cleanup()


def main():
    Log('Info: started')

    if (validArguments() == False):
        Log('Info: stopping due to invalid arguments')
        return

    if (sendData() == False):
        Log('Info: stopping due to being decline from server')
        return

    if (openDoor() == False):
        Log('Failure...')

    #printDebug("%s" % validArgs)

    Log('Finished')



#printDebug('DoorId = ' + DoorId)
#printDebug('CardId = ' + CardId)


#"""numOfArguments = len(sys.argv)

#for val in sys.argv:
#    print(val)

#print('')

#printDebug('This is a test')"""




#printDebug('Finished')


# The Python interpreter checks that the module as the main program, if it is imported from another module the '__name__' value will be that module's name. 
# Apparently it is a convention to do this, and not really necessary for this project but is future proofing and good practice.
if (__name__ == "__main__"):
    main()