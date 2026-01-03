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
        private static bool wasATapped = false;
        private static bool wasBTapped = false;
        private static IMonitor M;
        private static bool alreadyLoggedA = false;
        private static bool alreadyLoggedB = false;
        private static bool alreadyLoggedTap = false;

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
                original: AccessTools.PropertyGetter(typeof(VirtualJoypad), nameof(VirtualJoypad.ButtonAPressed)),
                postfix: new HarmonyMethod(typeof(Tiger), nameof(Tiger.ButtonA))
            );
            harmony.Patch(
                original: AccessTools.PropertyGetter(typeof(VirtualJoypad), nameof(VirtualJoypad.ButtonBPressed)),
                postfix: new HarmonyMethod(typeof(Tiger), nameof(Tiger.ButtonB))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(TapToMove), nameof(TapToMove.OnTap)),
                postfix: new HarmonyMethod(typeof(Tiger), nameof(Tiger.OnTap))
            );

            helper.Events.GameLoop.ReturnedToTitle += Reset1;
            helper.Events.GameLoop.SaveLoaded += Reset2;
        }
        private void Reset1(object sender, ReturnedToTitleEventArgs e)
        {
            wasATapped = false;
            wasBTapped = false;
        }
        private void Reset2(object sender, SaveLoadedEventArgs e)
        {
            wasATapped = false;
            wasBTapped = false;
        }
        private static void ButtonA(ref bool __result)
        {
            try
            {
                if (__result && !wasATapped && Context.IsWorldReady && Context.IsPlayerFree)
                {
                    TriggerActionManager.Raise("kazutopi1.KT_ButtonAPressed");
                }
                wasATapped = __result;
            }
            catch (Exception ex)
            {
                if (!alreadyLoggedA)
                {
                    M.Log($"Failed to raise trigger. {ex.ToString()}", LogLevel.Error);
                    alreadyLoggedA = true;
                }
            }
        }
        private static void ButtonB(ref bool __result)
        {
            try
            {
                if (__result && !wasBTapped && Context.IsWorldReady && Context.IsPlayerFree)
                {
                    TriggerActionManager.Raise("kazutopi1.KT_ButtonBPressed");
                }
                wasBTapped = __result;
            }
            catch (Exception ex)
            {
                if (!alreadyLoggedB)
                {
                    M.Log($"Failed to raise trigger. {ex.ToString()}", LogLevel.Error);
                    alreadyLoggedB = true;
                }
            }
        }
        public static void OnTap(int mouseX, int mouseY, int viewportX, int viewportY)
        {
            try
            {
                int tappedTileX = (mouseX + viewportX) / Game1.tileSize;
                int tappedTileY = (mouseY + viewportY) / Game1.tileSize;

                int playerTileX = Game1.player.TilePoint.X;
                int playerTileY = Game1.player.TilePoint.Y;

                if (tappedTileX == playerTileX && tappedTileY == playerTileY)
                {
                    if (Game1.options.weaponControl is 0 or 1)
                    {
                        TriggerActionManager.Raise("kazutopi1.KT_OnTap");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!alreadyLoggedTap)
                {
                    M.Log($"Failed to raise trigger: {ex.ToString()}", LogLevel.Error);
                    alreadyLoggedTap = true;
                }
            }
        }
    }
}
