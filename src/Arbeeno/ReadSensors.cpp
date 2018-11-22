#include <Wire.h>
#include "BME280.h"
#include "Arduino.h"
//https://github.com/queuetue/Q2-HX711-Arduino-Library
#include <Q2HX711.h>

//BME280 bmeInternal;
//BME280 bmeExternal;

//scales
const byte hx711_data_pin = A2;
const byte hx711_clock_pin = A3;
Q2HX711 hx711(hx711_data_pin, hx711_clock_pin);

char _deviceId[] = "00001";
int _loopDelay = 10000;

int _loudness = 0;

char _bufferStr[60];
char _tempIntStr[10];
char _pressureIntStr[10];
char _humidIntStr[10];

char _tempExtStr[10];
char _pressureExtStr[10];
char _humidExtStr[10];

char _weightStr[10];

//void setup()
//{
//	bmeInternal = BME280(0x76);
//	bmeInternal.begin();
//
//	bmeExternal = BME280(0x77);
//	bmeExternal.begin();
//
//	Serial.begin(9600);
//}

char* getSensorData(BME280 bmeInternal, BME280 bmeExternal)
{
	// Temp, Barometer, Humidity 
	dtostrf(bmeInternal.readTemp(), 5, 2, _tempIntStr);
	dtostrf(bmeInternal.readPressure(), 5, 2, _pressureIntStr);
	dtostrf(bmeInternal.readHumidity(), 5, 2, _humidIntStr);

	dtostrf(bmeExternal.readTemp(), 5, 2, _tempExtStr);
	dtostrf(bmeExternal.readPressure(), 5, 2, _pressureExtStr);
	dtostrf(bmeExternal.readHumidity(), 5, 2, _humidExtStr);

	// Loudness
	_loudness = analogRead(0);

	// Weight 
  double scale_read = hx711.read()/100.0;
  //Serial.println(scale_read);
  double scale_diff = scale_read - 85850;  //tare weight
  double kg = scale_diff * 0.004; // convert to kg
  //Serial.println(kg);
	dtostrf(kg, 5, 2, _weightStr);

	sprintf(_bufferStr, "%s,%s,%s,%s,%s,%s,%s,%s,%d", _deviceId, _tempIntStr, _pressureIntStr, _humidIntStr, _tempExtStr, _pressureExtStr, _humidExtStr, _weightStr, _loudness);

	//Serial.println(_bufferStr);

	return _bufferStr;

}
