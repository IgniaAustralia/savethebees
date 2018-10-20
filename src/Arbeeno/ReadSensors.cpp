#include <Wire.h>
#include <Seeed_BME280.h>
#include "Arduino.h"

char _deviceId[] = "00001"; 
int _loopDelay = 10000;

int _loudness = 0;

char _bufferStr[30];
char _tempStr[10];
char _pressureStr[10];
char _humidStr[10];
char _weightStr[10];



char* getSensorData(BME280 bme)
{
  // Temp, Barometer, Humidity 
  dtostrf(bme.getTemperature(), 5, 2, _tempStr);
  dtostrf(bme.getPressure(), 5, 2, _pressureStr);
  dtostrf(bme.getHumidity(), 5, 2,_humidStr);

  // Loudness
  _loudness = analogRead(0);

  // Weight - mocked due to additional hardware requirements
  dtostrf(22.72, 5, 2, _weightStr);
  
  sprintf (_bufferStr, "%s,%s,%s,%s,%s,%d", _deviceId, _tempStr, _pressureStr, _humidStr, _weightStr, _loudness);

  Serial.println(_bufferStr);
  return _bufferStr;


  //delay(_loopDelay);
}
