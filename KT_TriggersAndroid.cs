using StardewValley;
using StardewValley.Mobile;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley.Triggers;
using System;

namespace KT_Triggers
{
    internal sealed class Tiger : Mod
    {
        private static IMonitor M;

        public override void Entry(IModHelper helper)
        {
            M = this.Monitor;

            if (Constants.TargetPlatform != GamePlatform.Android)
            {
                var ex = new Exception();
                Monitor.Log($"This mod only supports Android. {ex.ToString()}", LogLevel.Error);
                return;
            }

            TriggerActionManager.RegisterTrigger("kazutopi1.KT_ButtonAPressed");
            TriggerActionManager.RegisterTrigger("kazutopi1.KT_ButtonBPressed");
            TriggerActionManager.RegisterTrigger("kazutopi1.KT_OnTap");

            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(VirtualJoypad), nameof(VirtualJoypad.CheckForTapJoystickAndButtons)),
                postfix: new HarmonyMethod(typeof(Tiger), nameof(Tiger.ButtonAOrButtonB))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(TapToMove), nameof(TapToMove.OnTap)),
                postfix: new HarmonyMethod(typeof(Tiger), nameof(Tiger.OnTap))
            );
        }
        public static void ButtonAOrButtonB(VirtualJoypad __instance)
        {
            try
            {
                if (__instance.ButtonAPressed)
                {
                    if (Game1.currentLocation.tapToMove.mobileKeyStates.useToolButtonPressed)
                    {
                        if (Game1.player.CurrentItem != null)
                        {
                            TriggerActionManager.Raise("kazutopi1.KT_ButtonAPressed", targetItem: Game1.player.CurrentItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M.LogOnce($"Failed to raise trigger. {ex.ToString()}", LogLevel.Error);
            }

            try
            {
                if (__instance.ButtonBPressed)
                {
                    if (Game1.currentLocation.tapToMove.mobileKeyStates.actionButtonPressed)
                    {
                        if (Game1.player.CurrentItem != null)
                        {
                            TriggerActionManager.Raise("kazutopi1.KT_ButtonBPressed", targetItem: Game1.player.CurrentItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M.LogOnce($"Failed to raise trigger. {ex.ToString()}", LogLevel.Error);
            }
        }
        public static void OnTap(int mouseX, int mouseY, int viewportX, int viewportY)
        {
            try
            {
                if (!Context.IsWorldReady || !Context.IsPlayerFree) { return; }

                int tappedTileX = (mouseX + viewportX) / Game1.tileSize;
                int tappedTileY = (mouseY + viewportY) / Game1.tileSize;

                int playerTileX = Game1.player.TilePoint.X;
                int playerTileY = Game1.player.TilePoint.Y;

                if (tappedTileX == playerTileX && tappedTileY == playerTileY)
                {
                    if (Game1.options.weaponControl is 0 or 1)
                    {
                        if (Game1.player.CurrentItem != null)
                        {
                            TriggerActionManager.Raise("kazutopi1.KT_OnTap", targetItem: Game1.player.CurrentItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M.LogOnce($"Failed to raise trigger: {ex.ToString()}", LogLevel.Error);
            }
        }
    }
}
