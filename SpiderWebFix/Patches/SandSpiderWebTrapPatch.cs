using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace SpiderWebFix.Patches;

[HarmonyPatch(typeof(SandSpiderWebTrap))]
public static class SandSpiderWebTrapPatch {
    [HarmonyPatch(nameof(SandSpiderWebTrap.Update))]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    private static void UpdatePostfix(SandSpiderWebTrap __instance) {
        SpiderWebFix.LogDebug("Updating Cobweb...", LogLevel.VERY_VERBOSE);

        if (StartOfRound.Instance is null or {
                IsServer: false, IsHost: false,
            }) {
            SpiderWebFix.LogDebug("We're not host or StartOfRound is null, skipping...", LogLevel.VERY_VERBOSE);
            return;
        }

        if (__instance is not {
                webHasBeenBroken: true,
            }) {
            SpiderWebFix.LogDebug("Cobweb is not broken, skipping...", LogLevel.VERY_VERBOSE);
            return;
        }

        __instance.transform.position = Vector3.left * 10000F;
        __instance.webHasBeenBroken = false;
        __instance.enabled = false;

        SpiderWebFix.LogDebug("Teleporting pesky cobweb :>");
    }
}