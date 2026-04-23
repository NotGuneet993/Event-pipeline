# Stream Handler
The stream handler will control the adding and removal of events. The class will handle registering, removing, and streaming of objects.

---
The event streamer consists of 3 methods:
* void <b>AddEvent</b>(StreamHandlerConfig config, params string[] eventNames)
* void <b>RemoveEvent</b>(string s)
* EventRecord <b>ProcessEvent</b>(e)

## Constructor
The constructor takes a required StreamHandlerConfig config object and an arbitrary number of strings the user would like to listen for.

`StreamHandler(StreamHandlerConfig, "Event-Name", "Kernel-Event-Name", "Network-Event-Name")`


## Methods
### Add Event
This function takes in a single string you'd like to register to the event listener. The default processing function will be applied. The function has a void return type.

`StreamHandler.AddEvent("Event-Name")`

### Remove Event
This function takes in a single string you'd like to remove to the event listener. The default processing function will be applied. The function has a void return type. If the event is not registered, the function will do nothing.

`StreamHandler.RemoveEvent("Event-Name")`

### Data processing function

## Event Record
WIP

## Stream Handler Config
WIP