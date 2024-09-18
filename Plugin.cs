using BepInEx;
using BepInEx.Logging;
using DuperMod.Player;
using HarmonyLib;

namespace DuperMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Chujowy {MyPluginInfo.PLUGIN_GUID} działa (chb)");

        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
        gameObject.AddComponent<PlayerCoordinates>();
    }
}