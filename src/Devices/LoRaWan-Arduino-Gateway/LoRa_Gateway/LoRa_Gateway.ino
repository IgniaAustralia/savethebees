/*******************************************************************************
 * Copyright (c) 2015 Matthijs Kooijman
 *
 * Permission is hereby granted, free of charge, to anyone
 * obtaining a copy of this document and accompanying files,
 * to do whatever they want with them without any restriction,
 * including, but not limited to, copying, modification and redistribution.
 * NO WARRANTY OF ANY KIND IS PROVIDED.
 *
 * This example transmits data on hardcoded channel and receives data
 * when not transmitting. Running this sketch on two nodes should allow
 * them to communicate.
 *******************************************************************************/

#include <lmic.h>
#include <hal/hal.h>
#include <SPI.h>
#include <Console.h>
#include <Bridge.h>
#include <Process.h>

#include <ArduinoJson.h>

#if !defined(DISABLE_INVERT_IQ_ON_RX)
#error This example requires DISABLE_INVERT_IQ_ON_RX to be set. Update \
       config.h in the lmic library to set it.
#endif

// How often to send a packet. Note that this sketch bypasses the normal
// LMIC duty cycle limiting, so when you change anything in this sketch
// (payload length, frequency, spreading factor), be sure to check if
// this interval should not also be increased.
// See this spreadsheet for an easy airtime and duty cycle calculator:
// https://docs.google.com/spreadsheets/d/1voGAtQAjC1qBmaVuP1ApNKs1ekgUjavHuVQIXyYSvNc 
#define TX_INTERVAL 5000

// Pin mapping
const lmic_pinmap lmic_pins = {
    .nss = 10,
    .rxtx = LMIC_UNUSED_PIN,
    .rst = 9,
    .dio = {2, 6, 7},
};


// These callbacks are only used in over-the-air activation, so they are
// left empty here (we cannot leave them out completely unless
// DISABLE_JOIN is set in config.h, otherwise the linker will complain).
void os_getArtEui (u1_t* buf) { }
void os_getDevEui (u1_t* buf) { }
void os_getDevKey (u1_t* buf) { }

void onEvent (ev_t ev) {
Console.print(os_getTime());
    Console.print(": ");
    switch(ev) {
        case EV_SCAN_TIMEOUT:
            Console.println(F("EV_SCAN_TIMEOUT"));
            break;
        case EV_BEACON_FOUND:
            Console.println(F("EV_BEACON_FOUND"));
            break;
        case EV_BEACON_MISSED:
            Console.println(F("EV_BEACON_MISSED"));
            break;
        case EV_BEACON_TRACKED:
            Console.println(F("EV_BEACON_TRACKED"));
            break;
        case EV_JOINING:
            Console.println(F("EV_JOINING"));
            break;
        case EV_JOINED:
            Console.println(F("EV_JOINED"));
            break;
        case EV_RFU1:
            Console.println(F("EV_RFU1"));
            break;
        case EV_JOIN_FAILED:
            Console.println(F("EV_JOIN_FAILED"));
            break;
        case EV_REJOIN_FAILED:
            Console.println(F("EV_REJOIN_FAILED"));
            break;
        case EV_TXCOMPLETE:
            Console.println(F("EV_TXCOMPLETE (includes waiting for RX windows)"));
            if (LMIC.txrxFlags & TXRX_ACK)
              Console.println(F("Received ack"));
            if (LMIC.dataLen) {
              Console.println(F("Received "));
              Console.println(LMIC.dataLen);
              Console.println((char*)LMIC.frame);
              Console.println(F(" bytes of payload"));
            }
            break;
        case EV_LOST_TSYNC:
            Console.println(F("EV_LOST_TSYNC"));
            break;
        case EV_RESET:
            Console.println(F("EV_RESET"));
            break;
        case EV_RXCOMPLETE:
            // data received in ping slot
            Console.println(F("EV_RXCOMPLETE"));
            break;
        case EV_LINK_DEAD:
            Console.println(F("EV_LINK_DEAD"));
            break;
        case EV_LINK_ALIVE:
            Console.println(F("EV_LINK_ALIVE"));
            break;
         default:
            Console.println(F("Unknown event"));
            break;
    }
}

osjob_t txjob;
osjob_t timeoutjob;
static void tx_func (osjob_t* job);

// Transmit the given string and call the given function afterwards
void tx(const char *str, osjobcb_t func) {
  os_radio(RADIO_RST); // Stop RX first
  delay(1); // Wait a bit, without this os_radio below asserts, apparently because the state hasn't changed yet
  LMIC.dataLen = 0;
  while (*str)
    LMIC.frame[LMIC.dataLen++] = *str++;
  LMIC.osjob.func = func;
  os_radio(RADIO_TX);
  Console.println("TX");
}

// Enable rx mode and call func when a packet is received
void rx(osjobcb_t func) {
  LMIC.osjob.func = func;
  LMIC.rxtime = os_getTime(); // RX _now_
  // Enable "continuous" RX (e.g. without a timeout, still stops after
  // receiving a packet)
  os_radio(RADIO_RXON);
  Console.println("<RX");
  Console.println(LMIC.freq);
  Console.println(LMIC.txpow);
  Console.println(LMIC.datarate);
  Console.println(LMIC.rps);
  Console.println("RX>");
}

static void rxtimeout_func(osjob_t *job) {
  digitalWrite(LED_BUILTIN, LOW); // off
}

static void rx_func (osjob_t* job) {
  // Blink once to confirm reception and then keep the led on
  digitalWrite(LED_BUILTIN, LOW); // off
  delay(10);
  digitalWrite(LED_BUILTIN, HIGH); // on

  // Timeout RX (i.e. update led status) after 3 periods without RX
  os_setTimedCallback(&timeoutjob, os_getTime() + ms2osticks(3*TX_INTERVAL), rxtimeout_func);

  // Reschedule TX so that it should not collide with the other side's
  // next TX
  os_setTimedCallback(&txjob, os_getTime() + ms2osticks(TX_INTERVAL/2), tx_func);

  Console.print("Got ");
  Console.print(LMIC.dataLen);
  Console.println(" bytes starting at ");
  Console.print(LMIC.dataBeg);
    
  for(int i = 0; i < LMIC.dataLen; i++)
  {
    Console.println(LMIC.frame[i]);
  }
  Console.println();
  Console.println(" pushing to shim");
    pushShim();
  Console.println(" pushed to Shim");
  // Restart RX
  rx(rx_func);
}

static void txdone_func (osjob_t* job) {
  rx(rx_func);
}

// log text to USART and toggle LED
static void tx_func (osjob_t* job) {
  // say hello
  tx("", txdone_func);
  // reschedule job every TX_INTERVAL (plus a bit of random to prevent
  // systematic collisions), unless packets are received, then rx_func
  // will reschedule at half this time.
  os_setTimedCallback(job, os_getTime() + ms2osticks(TX_INTERVAL + random(500)), tx_func);
}

// application entry point
void setup() {
  //Console.begin(115200);
  //Console.println("Starting");

    Bridge.begin(115200);
  Console.begin();
    //DEBUG ONLY TO ENSURE THINGS ARE WRITING TO CONSOLE
    while (!Console);
    
  #ifdef VCC_ENABLE
  // For Pinoccio Scout boards
  pinMode(VCC_ENABLE, OUTPUT);
  digitalWrite(VCC_ENABLE, HIGH);
  delay(1000);
  #endif

  pinMode(LED_BUILTIN, OUTPUT);

  // initialize runtime env
  os_init();

  // Set up these settings once, and use them for both TX and RX

  // Use a frequency in the g3 which allows 10% duty cycling.
  LMIC.freq = 915400000;
  // Maximum TX power
  LMIC.txpow = 30;
  // Use a medium spread factor. This can be increased up to SF12 for
  // better range, but then the interval should be (significantly)
  // lowered to comply with duty cycle limits as well.
  LMIC.datarate = DR_SF9;
  // This sets CR 4/5, BW125 (except for DR_SF7B, which uses BW250)
  LMIC.rps = updr2rps(LMIC.datarate);

  Console.println("Started");


  // setup initial job
  os_setCallback(&txjob, tx_func);
}

void pushShim()
{
  
  Process p;
  p.runShellCommand("curl --header \"Content-Type: application/json\" --request POST --data '{    \"DeviceId\": 1,    \"Internal Hive Temp (celcius)\": 1,    \"Internal Hive Pressure\": 1,    \"Internal Hive Hummidity\": 1,    \"External Temp\": 1,    \"External Pressure\": 1,    \"External Humidity\": 1,    \"Weight\": 1,    \"Internal Hive Loudness\": 1,    \"Vibration State\": 1,    \"Lat\": 1,    \"Long\": 1  }' http://savethebeesproxy.azurewebsites.net/api/devicedata");
}

void pushShell()
{
     Process p;    // Create a process and call it "p"
     p.runShellCommand("mosquitto_pub -h 'SaveTheBees-Iot-Hub.azure-devices.net' -p '8883' -u 'SaveTheBees-Iot-Hub.azure-devices.net/SaveTheBees-EdgeDevice' -P 'SharedAccessSignature sr=SaveTheBees-Iot-Hub.azure-devices.net&sig=KXmJajDoQPFF%2BO6okc0EIzEOufYiC68ggR7elX8JY%2Fk%3D&se=1571574357&skn=iothubowner' -i 'SaveTheBees-EdgeDevice' -t 'devices/SaveTheBees-EdgeDevice/messages/events/' -m 'field1=00&field2=99&status=BILLYISVERYCOOL' --cafile /root/digicert.cer -V mqttv311 --tls-version tlsv1.2");
}

void pushMosquito()
{
   Process p;    // Create a process and call it "p"
 //p.runShellCommand("curl -H 'Authorization: SharedAccessSignature sr=SaveTheBees-Iot-Hub.azure-devices.net&sig=KXmJajDoQPFF%2BO6okc0EIzEOufYiC68ggR7elX8JY%2Fk%3D&se=1571574357&skn=iothubowner' -d 'BillyIsCool' https://SaveTheBees-Iot-Hub.azure-devices.net/devices/SaveTheBees-EdgeDevice/messages/events?api-version=2016-02-03");
 //p.runShellCommand("curl https://iobees.azurewebsites.net/api/Data?code=3pVPMD1H9pv88pJitsgCvaN7pFbaXsxfaktzG3hamVMaSS5mf9AMWw==&a=BillyCool");

//p.runShellCommand("mosquitto_pub -h \"SaveTheBees-Iot-Hub.azure-devices.net\" -p \"8883\" -u \"SaveTheBees-Iot-Hub.azure-devices.net/SaveTheBees-EdgeDevice\" -P \"SharedAccessSignature sr=SaveTheBees-Iot-Hub.azure-devices.net&sig=KXmJajDoQPFF%2BO6okc0EIzEOufYiC68ggR7elX8JY%2Fk%3D&se=1571574357&skn=iothubowner\" -i \"SaveTheBees-EdgeDevice\" -t \"devices/SaveTheBees-EdgeDevice/messages/events/\" -m \"field1=00&field2=99&status=BILLYCOOL\" --cafile /root/digicert.cer -V mqttv311 --tls-version tlsv1.2");
//-i \"SaveTheBees-EdgeDevice\" -t \"devices/SaveTheBees-EdgeDevice/messages/events/\" -m \"field1=00&field2=99&status=BILLYCOOL\" --cafile /root/digicert.cer -V mqttv311 --tls-version tlsv1.2");

  // Create a process and call it "p"
  p.begin("mosquitto_pub"); // Process that launch the "cat" command
  p.addParameter("-h \"SaveTheBees-Iot-Hub.azure-devices.net\""); 
  p.addParameter("-p \"8883\""); 
  p.addParameter("-u \"SaveTheBees-Iot-Hub.azure-devices.net/SaveTheBees-EdgeDevice\""); 
  p.addParameter("-P \"SharedAccessSignature sr=SaveTheBees-Iot-Hub.azure-devices.net&sig=KXmJajDoQPFF%2BO6okc0EIzEOufYiC68ggR7elX8JY%2Fk%3D&se=1571574357&skn=iothubowner\""); 
  p.addParameter("-i \"SaveTheBees-EdgeDevice\"");
  p.addParameter("-d");
  p.addParameter("-t \"devices/SaveTheBees-EdgeDevice/messages/events/\"");
  p.addParameter("-m \"field1=00&field2=100&status=BILLYCOOL\"");
  p.addParameter("--cafile /root/digicert.cer ");
  p.addParameter("-V mqttv311"); 
  p.addParameter("--tls-version tlsv1.2");
  p.run();    // Run the process and wait for its termination

  // Print command output on the SerialUSB.
  // A process output can be read with the stream methods
  while (p.available() > 0) {
    char c = p.read();
    Console.print(c);
  }
  // Ensure the last bit of data is sent.
  Console.flush();
}

void loop() {


  // execute scheduled jobs and events
  os_runloop_once();
}
