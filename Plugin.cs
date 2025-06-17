using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using VampireCommandFramework;

namespace CustomBuffs;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("gg.deca.VampireCommandFramework")]
internal class Plugin : BasePlugin
{
  Harmony _harmony;
  internal static Plugin Instance { get; private set; }
  public static Harmony Harmony => Instance._harmony;
  public static ManualLogSource LogInstance => Instance.Log;

  // Configuration options
  static ConfigEntry<bool> _enabled;

  // Is this running on the server?
  public static bool IsServer { get; private set; }

  public static bool Enabled => _enabled.Value;

  public override void Load()
  {
    Instance = this;

    // Determine if we're running on server or client
    IsServer = Application.productName == "VRisingServer";
    Log.LogInfo($"Plugin.Load called, IsServer: {IsServer}");

    // Initialize services
    // BuffRegistry.InitializeBuffRegistry();
    // BuffService.Initialize();

    // Apply Harmony patches
    Log.LogInfo("Applying Harmony patches...");
    _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    Log.LogInfo("Harmony patches applied successfully");

    CommandRegistry.RegisterAll();

    Log.LogInfo($"{MyPluginInfo.PLUGIN_NAME}[{MyPluginInfo.PLUGIN_VERSION}] loaded on {(IsServer ? "server" : "client")}!");
  }

  public override bool Unload()
  {
    Config.Clear();
    _harmony.UnpatchSelf();
    return true;
  }
}