using StardewValley;
using StardewValley.Mobile;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley.Triggers;
using System;

namespace KT_Triggers
{
    public class Tiger : Mod
    {
        private static bool wasATapped = false;
        private static bool wasBTapped = false;
        private static IMonitor M;
        private static bool alreadyLogged = false;

        public override void Entry(IModHelper helper)
        {
            M = this.Monitor;
            if (Constants.TargetPlatform != GamePlatform.Android)
                return;

            TriggerActionManager.RegisterTrigger("kazutopi1.KT_ButtonAPressed");
            TriggerActionManager.RegisterTrigger("kazutopi1.KT_ButtonBPressed");
            var harmony = new Harmony(this.ModManifest.UniqueID);

            try
            {
                harmony.Patch(
                    original: AccessTools.PropertyGetter(typeof(VirtualJoypad), nameof(VirtualJoypad.ButtonAPressed)),
                    postfix: new HarmonyMethod(typeof(Tiger), nameof(Tiger.ButtonA))
                );
                harmony.Patch(
                    original: AccessTools.PropertyGetter(typeof(VirtualJoypad), nameof(VirtualJoypad.ButtonBPressed)),
                    postfix: new HarmonyMethod(typeof(Tiger), nameof(Tiger.ButtonB))
                );
            }
            catch (Exception ex)
            {
                M.Log($"Patch failed. {ex.Message}", LogLevel.Error);
            }

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
                if (!alreadyLogged)
                {
                    M.Log($"Failed to raise trigger. {ex.Message}", LogLevel.Error);
                    alreadyLogged = true;
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
                if (!alreadyLogged)
                {
                    M.Log($"Failed to raise trigger. {ex.Message}", LogLevel.Error);
                    alreadyLogged = true;
                }
            }
        }
    }
}
