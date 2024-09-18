using System.Collections.Generic;
using HarmonyLib;

namespace DuperMod.Patch;

[HarmonyPatch(typeof(LocalizationManager), "GetLocalizationString")]
public class LocalizationPatch
{
    private static readonly Dictionary<string, string> customMessages = new()
    {
        { "NPCmessagehit0", "Ouch! That hurt!" },
        { "NPCmessagehit1", "Hey! Watch it!" }
    };

    private static readonly List<string> loremIpsumPhrases = new()
    {
        "Lorem ipsum dolor sit amet",
        "Consectetur adipiscing elit",
        "Sed do eiusmod tempor incididunt",
        "Ut labore et dolore magna aliqua",
        "Ut enim ad minim veniam",
        "Quis nostrud exercitation ullamco",
        "Laboris nisi ut aliquip ex ea commodo consequat",
        "Duis aute irure dolor in reprehenderit",
        "Voluptate velit esse cillum dolore",
        "Eu fugiat nulla pariatur"
    };

    private static int currentPhraseIndex;

    [HarmonyPostfix]
    public static void Postfix(ref string __result, string key)
    {
        if (key.StartsWith("NPCmessagehit"))
        {
            var originalResult = __result;

            if (customMessages.TryGetValue(key, out var customMessage))
            {
                __result = customMessage;
            }
            else
            {
                __result = loremIpsumPhrases[currentPhraseIndex];
                currentPhraseIndex = (currentPhraseIndex + 1) % loremIpsumPhrases.Count;
            }

            Plugin.Logger.LogInfo(
                $"Localization for key {key}: Original: '{originalResult}', Modified: '{__result}'");
        }
    }
}