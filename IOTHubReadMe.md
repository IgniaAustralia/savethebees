Azure Resources Created 

1. Resource Group 
        a. This will the logical construct that groups all our resources together so they can be managed as a single entity  
2. IOT HUB
        a. To register and receive device and edge data. 
        b. To route device traffic. A custom route is created to divert raw traffic to Azure Storage (BLob) 
3. Azure Storage 
        a. Blob Storage to store raw device data. 
4. Azure Stream Analytics 
        a. This was used to provide a richly structured query syntax for data analysis both in the cloud and on IoT Edge devices. 
        b. This will group raw data hourly and provide MAX Temperature, Weight information for that period. 
        c. This will also capture Longitude and Lattitude information. 
5. SQL Server and SQL Database -- Store Stuctured Data. 

----------------------------------------------------------------------------------------------------------------------------------
IOT Device Setup 

Once the IOT Hub is created, new devices can be registered and simulate traffic to it using AZURE CLI Script or the Portal. 

AZURE CLI Scripts

Create Device Identity in IOT HUB
az iot hub device-identity create --device-id '<Device ID>' --login "<Connection String>"

Update Device Twins (Add Additional Metadata)
az iot hub device-twin update -d '<IOT Device ID>' -n '<IOT HUB Name>' --set tags="{'beeKeeper':{'beeKeepId':'13'}}"

Simulate Device Traggic to IOT HUB
az iot device simulate -n <IOT Hub Name> -d <Device ID> --msg-count 1 --data '{"deviceID" : "HiveID01","Temperature": "24.22","Weight": "23332","Longiturde":" -31.953512","Lattitude":" 115.857048"}'  

IOT CLI Command References

https://docs.microsoft.com/en-us/cli/azure/ext/azure-cli-iot-ext/iot?view=azure-cli-latest

-----------------------------------------------------------------------------------------------------------------------------------
