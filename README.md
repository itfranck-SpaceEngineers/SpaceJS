# SpaceJS
A Javascript Programmable Block mod for Space Engineers.

This MOD is considered experimental alpha and should NOT be used on live servers or games.

# Features

## Javascript Interpreter as a State Machine

* Since the javascript interpreter is a state machine, the programmable blocks can be throttled at the statement level.  This means that a javascript application can go into an infinite loop and/or run forever and never cause complexity errors or slow down the server.
* The "throttle" prevents more then a set amount of statements from getting executed across ALL programmable blocks.  So having 20 programmable blocks should not slow down the server.  Instead, each programmable block will run slower.
* Since the interpreted code executes in a sandbox, it is much safer.

## API

### Console

* Use ```console.log``` to write text to the Custom Info section of the programmable block (the right side where some blocks show status).
```javascript
console.log("Hello World");
```

### Events

* Subscribe and unsubscibe from actions.  Actions you are subscribed to automatically get added as actions in button panels and cockpits so you can trigger javascript code to run when to press a button, etc.  In the following example, after running the code initially, "AnAction" will show up in a button panel for the programmable block as an action.  If you assign that action and trigger it, the message will be output, then the action will get removed.  So if you press the button panel button a second time, nothing will happen.
```javascript
function doSomething(action)
{
  event.offAction("AnAction", doSomething); // unsubscribe from the action
  console.log(action + " action was just triggered and then removed.");
}

event.onAction("AnAction", doSomething); // subscribe to the action
```

* more events to be added later



### Under Construction

More functionality will be added to the javascript API soon.



[![IMAGE ALT TEXT HERE](http://img.youtube.com/vi/Ja02--7lH9U/0.jpg)](http://www.youtube.com/watch?v=Ja02--7lH9U)

