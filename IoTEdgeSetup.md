Raspberry Pi 3 Model B was our selected device for IoT edge

#### Configure Internet Connection on PI
follow [this](https://medium.com/@r_musitelli/raspberry-pi-3-headless-setup-109fe2a8f5ec)
1) Use card reader on a laptop, flash the SD card with Raspbian Stretch Lite and enable SSH
2) Plug SD card back into the PI device, ensure it is properly powered (normal laptop usb with <2.1A voltage will not work)
3) Wait for PI to boot up. You can connect PI to a monitor using HDMI to see the boot progress if preferred.
4) Once booted up, connect PI to your laptop using an Enthernet cable, then ping `raspberry.local` to verify you can hit the device
5) SSH to `raspberry.local` (default credential was stated in the article link above) using Putty
6) Follow the remaining of the article above and setup WIFI

#### Install Azure IoT Edge Module on PI
follow [this](https://blog.jongallant.com/2017/11/azure-iot-edge-raspberrypi/)
