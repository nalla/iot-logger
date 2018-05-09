#include "DHT.h"
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <ArduinoJson.h>

#define DHTPIN 4
#define DHTTYPE DHT22

DHT dht(DHTPIN, DHTTYPE);

const char* WIFI_SSID = "";
const char* WIFI_PASS = "";
const char* DEVICE_ID = "";
const char* API_ENDPOINT = "";
const char* API_KEY = "";

WiFiClientSecure wifiClient;

void connect() {

  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(WIFI_SSID);
  
  WiFi.mode(WIFI_STA);

  if (WiFi.status() != WL_CONNECTED) {
    WiFi.begin(WIFI_SSID, WIFI_PASS);
  }

  unsigned long wifiConnectStart = millis();

  while (WiFi.status() != WL_CONNECTED) {
    if (WiFi.status() == WL_CONNECT_FAILED) {
      Serial.println("Failed to connect to WIFI. Please verify credentials: ");
      Serial.println();
      Serial.print("SSID: ");
      Serial.println(WIFI_SSID);
      Serial.print("Password: ");
      Serial.println(WIFI_PASS);
      Serial.println();
    }

    delay(500);
    Serial.println("...");

    // Only try for 5 seconds.
    if(millis() - wifiConnectStart > 10000) {
      Serial.println("Failed to connect to WiFi");
      Serial.println("Please attempt to send updated configuration parameters.");
      return;
    }
  }

  Serial.println();
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  Serial.println();
}

void setup() {
  Serial.begin(9600);
  Serial.setTimeout(2000);

  // Wait for serial to initialize.
  while(!Serial) { }

  Serial.println();
  Serial.println("Device started.");
  
  connect();
  Serial.println("Device connected.");

  readAndReport();

  Serial.println("Going into deep sleep for 30 seconds.");
  ESP.deepSleep(30e6); // 60e6 is 30 microseconds
  delay(100);
}

void readAndReport() {
  bool read = false;

  while(!read) {
    delay(400);
    
    float h = dht.readHumidity();
    float t = dht.readTemperature();
    float f = dht.readTemperature(true);
  
    if (isnan(h) || isnan(t) || isnan(f)) {
      Serial.println("Failed to read from DHT sensor!");
    } else {
      read = true;
    }
  
    if (read) {
      float hif = dht.computeHeatIndex(f, h);
      float hic = dht.computeHeatIndex(t, h, false);
    
      Serial.print("Humidity: ");
      Serial.print(h);
      Serial.print(" %\t");
      Serial.print("Temperature: ");
      Serial.print(t);
      Serial.print(" *C ");
      Serial.print(f);
      Serial.print(" *F\t");
      Serial.print("Heat index: ");
      Serial.print(hic);
      Serial.print(" *C ");
      Serial.print(hif);
      Serial.println(" *F");
      
      report(t, h, hic);
    }
  }
}

void report(double t, double h, double hi) {
  StaticJsonBuffer<600> jsonBuffer;
  JsonObject& root = jsonBuffer.createObject();
  root["deviceId"] = DEVICE_ID;
  root["temperature"] = t;
  root["humidity"] = h;
  root["heatIndex"] = hi;
  String buffer;
  root.printTo(buffer);

  HTTPClient http;
  http.begin(API_ENDPOINT);
  http.addHeader("Content-Type", "application/json");
  http.addHeader("Accept", "application/json");
  http.addHeader("apiKey", API_KEY);

  int httpCode = http.POST(buffer);

  if(httpCode != 204) {
    if(httpCode == 400) {
      Serial.println("ERR: Invalid API key!");
    } else {
      Serial.println("ERR: Unable to connect to API!");
    }
  }

  http.end();
}

void loop() {
}


