#include <Wire.h>
#include "BME280.h"
#include "Arduino.h"

char _deviceId[] = "00001";
const int _loopDelay = 1000;
const int _switchTilt = 5;

int _loudness = 0;

int _tiltSwitch = 0;
int _tmpTiltSwitch = 0;
bool _movement = false;

char _bufferStr[100];
char _tempIntStr[10];
char _pressureIntStr[10];
char _humidIntStr[10];

char _tempExtStr[10];
char _pressureExtStr[10];
char _humidExtStr[10];

char _weightStr[10];


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

	// Determine a change in the tilt switch value
	_movement = false;
	_tmpTiltSwitch = digitalRead(_switchTilt);

	if (_tmpTiltSwitch != _tiltSwitch)
	{
		_tiltSwitch = _tmpTiltSwitch;
		_movement = true;
	}

	sprintf(_bufferStr, "%s,%s,%s,%s,%s,%s,%s,%s,%d,%i", _deviceId, _tempIntStr, _pressureIntStr, _humidIntStr, _tempExtStr, _pressureExtStr, _humidExtStr, _weightStr, _loudness, _movement);

	//Serial.println(_bufferStr);

	return _bufferStr;

}
