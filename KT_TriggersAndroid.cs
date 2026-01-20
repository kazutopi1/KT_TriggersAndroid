using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Triggers;
using System;

namespace KT_Triggers
{
    internal sealed class Trigger : Mod
    {
        private bool wasATapped = false;
        private bool wasBTapped = false;

        public override void Entry(IModHelper helper)
        {
            if (Constants.TargetPlatform != GamePlatform.Android)
            {
                var ex = new Exception();
                Monitor.Log($"This mod only supports Android. {ex.ToString()}", LogLevel.Error);
                return;
            }

            TriggerActionManager.RegisterTrigger("kazutopi1.KT_ButtonAPressed");
            TriggerActionManager.RegisterTrigger("kazutopi1.KT_ButtonBPressed");
            TriggerActionManager.RegisterTrigger("kazutopi1.KT_OnTap");

            helper.Events.GameLoop.UpdateTicked += this.ButtonStateCatcher;
            Helper.Events.Input.ButtonReleased += this.OnTap;
        }
        public void ButtonStateCatcher(object s, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.activeClickableMenu != null) { return; }

            if (!wasATapped && Game1.virtualJoypad.ButtonAPressed && Game1.player.CurrentItem != null)
            {
                TriggerActionManager.Raise("kazutopi1.KT_ButtonAPressed", targetItem: Game1.player.CurrentItem);
                wasATapped = true;
            }
            else if (!Game1.virtualJoypad.ButtonAPressed)
            {
                wasATapped = false;
            }

            if (!wasBTapped && Game1.virtualJoypad.ButtonBPressed && Game1.player.CurrentItem != null)
            {
                TriggerActionManager.Raise("kazutopi1.KT_ButtonBPressed", targetItem: Game1.player.CurrentItem);
                wasBTapped = true;
            }
            else if (!Game1.virtualJoypad.ButtonBPressed)
            {
                wasBTapped = false;
            }
        }
        public void OnTap(object s, ButtonReleasedEventArgs e)
        {
            if (!Context.IsPlayerFree || !Context.IsWorldReady) { return; }

            if (e.Button == SButton.MouseLeft)
            {
                if (e.Cursor.Tile == Game1.player.getTileLocation() && Game1.player.CurrentItem != null)
                {
                    if (Game1.options.weaponControl is 0 or 1)
                    {
                        TriggerActionManager.Raise("kazutopi1.KT_OnTap", targetItem: Game1.player.CurrentItem);
                    }
                }
            }
        }
    }
}
