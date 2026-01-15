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
            if (!Context.IsWorldReady || !Context.IsPlayerFree) { return; }

            var v = Game1.virtualJoypad;
            var f = Game1.player;
            var keyState = Game1.currentLocation.tapToMove.mobileKeyStates;

            if (f.CurrentItem != null)
            {
                if (keyState.useToolButtonPressed && !wasATapped)
                {
                    TriggerActionManager.Raise("kazutopi1.KT_ButtonAPressed", targetItem: f.CurrentItem);
                    wasATapped = keyState.useToolButtonPressed;
                }
                else { wasATapped = false; }
            }

            if (f.CurrentItem != null)
            {
                if (keyState.actionButtonPressed && !wasBTapped)
                {
                    TriggerActionManager.Raise("kazutopi1.KT_ButtonBPressed", targetItem: f.CurrentItem);
                    wasBTapped = keyState.actionButtonPressed;
                }
                else { wasBTapped = false; }
            }
        }
        public void OnTap(object s, ButtonReleasedEventArgs e)
        {
            if (e.Button == SButton.MouseLeft && Context.IsPlayerFree && Context.IsWorldReady)
            {
                if (e.Cursor.Tile == Game1.player.getTileLocation() && Game1.player.CurrentItem != null)
                {
                    if (Game1.options.weaponControl == 0 || Game1.options.weaponControl == 1)
                    {
                        TriggerActionManager.Raise("kazutopi1.KT_OnTap", targetItem: Game1.player.CurrentItem);
                    }
                }
            }
        }
    }
}
