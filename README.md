# KT Triggers (Android)

Adds 3 new custom Triggers for Data/TriggerActions for Android users to utilize.

`kazutopi1.KT_ButtonAPressed`:
Raised when player is currently holding an item and virtual button A(Left) is pressed/tapped.

`kazutopi1.KT_ButtonBPressed`:
Raised when player is currently holding an item and virtual button B(Right) is pressed/tapped.

`kazutopi1.KT_OnTap`:
Raised when the current movement control is `Tap-to-move & Auto-Attack` and the tile the player standing in is pressed/tapped while currently holding an item.

Example: This snippet will give the player 1 gold every time virtual button B is pressed while holding item 388.
```
{
  "Format": "2.8.0",
  "Changes": [
    {
      "Action": "EditData",
      "Target": "Data/TriggerActions",
      "Entries": {
        "Test1": {
          "Id": "YourTriggerActionId",
          "Trigger": "kazutopi1.KT_ButtonBPressed",
          "Condition": "ITEM_ID Target 388",
          "Actions": [
            "AddMoney 1"
          ],
          "MarkActionApplied": false
        }
      }
    }
  ]
}
```
