# Forensics

## Current State

A forensic suite for windows. Aiming for the detecting USB devices

This tool uses the following keys in the registry to determine which USB devices have been connected to the system:

The following sources are currently scanned:

* HKLM\SYSTEM\CurrentControlSet\Enum\USB
* HKLM\SYSTEM\CurrentControlSet\Enum\USBStor
* HKLM\SYSTEM\MountedDevices

## Requirements

* Detect USB devices which have been connected to the computer
* Detect USB device installation time. (First Installation Time)
* Detect USB device insertion time. (Last Installation Time)
* Map the drive letter and information where possible to the USB device.
* Track the devices via serial number.
