# Event Emitter

This azure functions simply emit random message to your EventHub. 

# Installation

## Create Funcion App 

Deploy this function to a Function App (V2).

## Get EventHub Connection string 

Get the EventHub Connection string. 
NOTE: NOT EventHub namespace. 

The connection string should include EntityPath. Double check your connection string. It requres send/receive right to the Key. 

Go to App Settings of your Function App. 

| APPSETTING_NAME | VALUE |
|--|--|
| EventHubConnectionString | YOUR_EVENT_HUBS_CONNECTION_STRING |

## Run From Package deployment

The convenient way to setup this application is, to zip this application and put on somewhere and set the AppSettings like this. It is automatically deployed

| APPSETTING_NAME | VALUE |
|--|--|
| WEBSITE_RUN_FROM_PACKAGE | THE_URL_POINT_TO_THE_ZIP_FILE |

# Usage 

Send message to `http://YOUR_FUNCTION_APP/api/EventEmitter?number=1000` It create messages. 

```
{"id":"SOME_GUID", "body":"Random Strings"}
```

This app emit the random message to the EventHub during `number * 10` milliseconds. 

Don't make it longer than 5 minutes. (by default setting of Azure Functions.)

