# Investigation summary

![PicoUSB](img/Overview.jpg)
![PicoUSB](img/Overview-back.jpg)

## Introduction

Purchased from [Elecrow.com](https://www.elecrow.com/picousb-raspberry-pi-pico-rp2040-powered-bad-usb-rubber-ducky.html)

This is a low cost circuit board Bad USB device, built around Raspberry Pi PR2040(pico). [Github](https://github.com/TomBrlek/PicoUSB) contains details on how to use the device, and links to other useful resources.

This device is technically running python, which is an interpreter for the simple language used to create an attack.

### Pros and Cons

| Pros                                                                  | Cons                     |
| :-------------------------------------------------------------------- | :----------------------- |
| Cheap (~£10)                                                          | Not a normal looking USB |
| Easy to setup (walkthrough and [video](https://youtu.be/jKH6WgFiaB0)) | No Case                  |
| Simple language to write attack                                       |                          |
| Can interact with mouse and volume as well as keyboard                |                          |
| Can be programmed via USB                                             |                          |

#### There are 3D printable cases available

* [Thingverse - PicoUsb case](https://www.thingiverse.com/thing:7022646)

### Uses

* Keystroke Injection Attack (BadUSB)
* Mouse Jiggler
* Mouse injection attack

## Issues

You brick the device during setup.

## Detailed investigation

### Creating an attack

1. Download the latest [CircuitPython](https://circuitpython.org/board/raspberry_pi_pico/) for Rasperry Pi Pico.
2. Insert PicoUSB into your USB drive while holding the "Boot" button. (Opens it in a bootloader mode. The first time you do this you don't have to hold the "Boot" button)
3. Copy `CircuitPython.uf2` file to the USB and wait for it to finish setting up. (will close and reopen the explorer, be patient)
4. Create the payload, following the [example](https://github.com/TomBrlek/PicoUSB/blob/main/src/example.txt).

```txt
    delay(4)
    write(Hello, World!)
    press(enter)
```

5. Copy the src folder of the [repo](https://github.com/TomBrlek/PicoUSB/tree/main/src) to the USB device.
6. Save the script as `pico_usb.txt` on the USB device via explorer.

### Operation

* Inserting the PicoUSB while not holding any buttons will not show as a USB drive and will execute the "bad usb" code found in the pico_usb.txt.
* Inserting it while holding the "Mode" button will not execute any "bad usb" code and will show as a USB drive. This way you can freely edit the code.
* If you insert it while holding the "Boot" button, it will open in bootloader mode. This is only used the first time the device is configured and never again.

### Setup

### Registry information

Detection under windows:

This is a Raspbery pi Pico under the hood so we expect (Values taken from [the sz development](https://the-sz.com/products/usbid/index.php?v=0x2E8A)):

| Vendor id | Product Id |
| --------- | ---------- |
| 0x2E8A    | 0x003      |

#### Device 1

![Device1](img/Device1.jpg)

##### Registry Entries for Device 1

| Vendor id | Product Id | Interface | Description         | Notes                           | Keys                                                                                       |
| --------- | ---------- | --------- | ------------------- | ------------------------------- | ------------------------------------------------------------------------------------------ |
| 0x239A    | 0x80F4     |           | RootDevice          | "ParentIdPrefix"="6&169dd6e5&0" | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4\E66368254F853226        |
| 0x239A    | 0x80F4     | 00        | USB Serial Device   |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_00\6&169dd6e5&0&0000 |
| 0x239A    | 0x80F4     | 02        | USB Input Device    |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_02\6&169dd6e5&0&0002 |
| 0x239A    | 0x80F4     | 03        | CircuitPython Audio |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_03\6&169dd6e5&0&0003 |

#### Device 2

![Device2](img/Device2.jpg)

##### Boot Mode

| Vendor id | Product Id | Interface | Description             | Notes                         | Keys                                                                                       |
| --------- | ---------- | --------- | ----------------------- | ----------------------------- | ------------------------------------------------------------------------------------------ |
| 0x2E8A    | 0x0003     |           | Root Device             | ParentIdPrefix="6&22cf742d&0" | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_2E8A&PID_0003\E0C9125B0D9B            |
| 0x2E8A    | 0x0003     | 00        | USB Mass Storage Device |                               | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_2E8A&PID_0003&MI_00\6&22cf742d&0&0000 |
| 0x2E8A    | 0x0003     | 01        | RP2 Boot                |                               | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_2E8A&PID_0003&MI_01\6&22cf742d&0&0001 |
|           |            |           |                         |                               |                                                                                            |

| Vendor id | Product Id | Revision | Description        | Notes | Keys                                                                                                         |
| --------- | ---------- | -------- | ------------------ | ----- | ------------------------------------------------------------------------------------------------------------ |
| RPI       | RP2        | 3        | RPI RP2 USB Device |       | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USBSTOR\Disk&Ven_RPI&Prod_RP2&Rev_3\7&1abfde23&0&E0C9125B0D9B&0 |

##### Registry Entries for Device 2

Machine restarted before insertion

| Vendor id | Product Id | Interface | Description         | Notes                           | Keys                                                                                       |
| --------- | ---------- | --------- | ------------------- | ------------------------------- | ------------------------------------------------------------------------------------------ |
| 0x239A    | 0x80F4     |           | Root Device         | "ParentIdPrefix"="6&18f19057&0" | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4\E66368254F3E9F27        |
| 0x239A    | 0x80F4     | 00        | USB Serial Device   |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_00\6&18f19057&0&0000 |
| 0x239A    | 0x80F4     | 02        | USB Input Device    |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_02\6&18f19057&0&0002 |
| 0x239A    | 0x80F4     | 03        | CircuitPython Audio |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_03\6&18f19057&0&0003 |

#### Device 3

![Device3](img/Device3.jpg)

##### Registry Entries for Device 3

| Vendor id | Product Id | Interface | Description         | Notes                         | Keys                                                                                       |
| --------- | ---------- | --------- | ------------------- | ----------------------------- | ------------------------------------------------------------------------------------------ |
| 0x239A    | 0x80F4     |           | Root Device         | ParentIdPrefix"="6&285546c3&0 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4\E66368254F828728        |
| 0x239A    | 0x80F4     | 00        | USB Serial Device   |                               | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_00\6&285546c3&0&0000 |
| 0x239A    | 0x80F4     | 02        | USB Input Device    |                               | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_02\6&285546c3&0&0002 |
| 0x239A    | 0x80F4     | 03        | CircuitPython Audio |                               | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_03\6&285546c3&0&0003 |
|           |            |           |                     |                               |                                                                                            |

#### Device 4

![Device4](img/Device4.jpg)

##### Registry Entries for Device 4

| Vendor id | Product Id | Interface | Description         | Notes                          | Keys                                                                                      |
| --------- | ---------- | --------- | ------------------- | ------------------------------ | ----------------------------------------------------------------------------------------- |
| 0x239A    | 0x80F4     |           | Root Device         | "ParentIdPrefix"="6&831d513&0" | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4\E66368254F554721       |
| 0x239A    | 0x80F4     | 00        | USB Serial Device   |                                | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_00\6&831d513&0&0000 |
| 0x239A    | 0x80F4     | 02        | USB Input Device    |                                | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_02\6&831d513&0&0002 |
| 0x239A    | 0x80F4     | 03        | CircuitPython Audio |                                | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_03\6&831d513&0&0003 |
|           |            |           |                     |                                |                                                                                           |

#### Device 5

![Device5](img/Device5.jpg)

##### Registry Entries for Device 5

| Vendor id | Product Id | Interface | Description         | Notes                           | Keys                                                                                       |
| --------- | ---------- | --------- | ------------------- | ------------------------------- | ------------------------------------------------------------------------------------------ |
| 0x239A    | 0x80F4     |           | Root Device         | "ParentIdPrefix"="6&2d726e30&0" | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4\E66368254F0F8921        |
| 0x239A    | 0x80F4     | 00        | USB Serial Device   |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_00\6&2d726e30&0&0000 |
| 0x239A    | 0x80F4     | 02        | USB Input Device    |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_02\6&2d726e30&0&0002 |
| 0x239A    | 0x80F4     | 03        | CircuitPython Audio |                                 | HKEY_LOCAL_MACHINE\System\ControlSet001\Enum\USB\VID_239A&PID_80F4&MI_03\6&2d726e30&0&0003 |

#### Notes

The device instance ids are provided from the device. This allows tracing between computers.

| Device   | Device Serial Number |
| -------- | -------------------- |
| Device 1 | E66368254F853226     |
| Device 2 | E66368254F3E9F27     |
| Device 3 | E66368254F828728     |
| Device 4 | E66368254F554721     |
| Device 5 | E66368254F0F8921     |
