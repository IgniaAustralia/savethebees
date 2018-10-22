#include <Wire.h>
#include "BME280.h"
#include "Arduino.h"

//BME280 bmeInternal;
//BME280 bmeExternal;

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

	// Weight - mocked due to additional hardware requirements
	dtostrf(22.72, 5, 2, _weightStr);

	sprintf(_bufferStr, "%s,%s,%s,%s,%s,%s,%s,%s,%d", _deviceId, _tempIntStr, _pressureIntStr, _humidIntStr, _tempExtStr, _pressureExtStr, _humidExtStr, _weightStr, _loudness);

	//Serial.println(_bufferStr);

	return _bufferStr;

}
