# RasPi-Control

Project; RasPi Controller
C# WPF application used to manage both scripts to run and Raspberry Pi configurations to easily test various scripts on remote Pis. 
Uses SSH to send across a command to run scripts (that should be on the Pi), arguments can also be added to alter what the script should do on that occasion.

Project; NFC Card Reader
C# WPF application to read details from a NFC Card reader (tested with ACR122U-A9) and then relay that information to a Raspberry Pi to then check with a server and decide to open a door/take action or not. Script is in 'RasPi Scripts'.

Project; RasPi Scripts
A selection of Python scripts that are intended to be located on a Raspberry Pi and called to run. If you have any problem, note most require root privileges to access the hardware (GPIO) and also require arguments to use.

