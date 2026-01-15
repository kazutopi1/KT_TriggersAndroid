# KT Triggers (Android)

Adds 3 new custom Triggers for Data/TriggerActions for Android users to utilize.

`kazutopi1.KT_ButtonAPressed`:
Raised when player is currently holding an item and virtual button A(Left) is pressed/tapped.

`kazutopi1.KT_ButtonBPressed`:
Raised when player is currently holding an item and virtual button B(Right) is pressed/tapped.

`kazutopi1.KT_OnTap`:
Raised when the current movement control is `Tap-to-move & Auto-Attack` and the tile the player standing in is pressed/tapped while currently holding an item.

Examples: 

This snippet will give the player 1 gold every time virtual button B is pressed while holding the item `(O)388`.
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
          "Condition": "ITEM_ID Target (O)388",
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

This snippet will add a critical chance buff each time you press virtual button A while holding the item `(W)0`, making each swing have a guaranteed crit.
```
{
  "Format": "2.8.0",
  "Changes": [
    {
      "Action": "EditData",
      "Target": "Data/TriggerActions",
      "Entries": {
        "Test2": {
          "Id": "YourTriggerActionId",
          "Trigger": "kazutopi1.KT_ButtonAPressed",
          "Condition": "ITEM_ID Target (W)0",
          "Actions": [
            "AddBuff Your_BuffId 500"
          ],
          "MarkActionApplied": false
        }
      }
    },
    {
      "Action": "EditData",
      "Target": "Data/Buffs",
      "Entries": {
        "Your_BuffId": {
          "DisplayName": "YourBuffName",
          "Duration": 500,
          "IconTexture": "TileSheets/BuffsIcons",
          "IconSpriteIndex": 11,
          "Effects": {
            "CriticalChanceMultiplier": 1000
          }
        }
      }
    }
  ]
}
```
