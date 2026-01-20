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

            helper.Events.GameLoop.UpdateTicking += this.ButtonStateCatcher;
            helper.Events.Input.ButtonReleased += this.OnTap;
        }
        public void ButtonStateCatcher(object s, UpdateTickingEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.activeClickableMenu != null) { return; }

            var keyState = Game1.currentLocation.tapToMove.mobileKeyStates;

            if (!wasATapped && keyState.useToolButtonPressed && Game1.player.CurrentItem != null)
            {
                TriggerActionManager.Raise("kazutopi1.KT_ButtonAPressed", targetItem: Game1.player.CurrentItem);
                wasATapped = true;
            }
            else if (!keyState.useToolButtonPressed)
            {
                wasATapped = false;
            }

            if (!wasBTapped && keyState.actionButtonPressed && Game1.player.CurrentItem != null)
            {
                TriggerActionManager.Raise("kazutopi1.KT_ButtonBPressed", targetItem: Game1.player.CurrentItem);
                wasBTapped = true;
            }
            else if (!keyState.actionButtonPressed)
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
