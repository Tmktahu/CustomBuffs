using BepInEx.Logging;
using ProjectM.Scripting;
using CustomBuffs.Services;
using Unity.Entities;
using Unity.Collections;

namespace CustomBuffs;

internal static class Core
{
  public static bool _initialized = false;
  public static World World { get; } = GetServerWorld() ?? throw new Exception("There is no Server world!");
  private static SystemService _systemService;
  public static SystemService SystemService => _systemService ??= new(World);
  public static EntityManager EntityManager => World.EntityManager;
  public static ServerGameManager ServerGameManager => SystemService.ServerScriptMapper.GetServerGameManager();
  public static ManualLogSource Log => Plugin.LogInstance;

  public static void Initialize()
  {
    if (_initialized) return;

    Log.LogInfo("CustomBuffs Core initialized");

    _initialized = true;
  }
  private static World GetServerWorld()
  {
    return World.s_AllWorlds.ToArray().FirstOrDefault(world => world.Name == "Server");
  }
}

public readonly struct NativeAccessor<T> : IDisposable where T : unmanaged
{
  static NativeArray<T> _array;
  public NativeAccessor(NativeArray<T> array)
  {
    _array = array;
  }
  public T this[int index]
  {
    get => _array[index];
    set => _array[index] = value;
  }
  public int Length => _array.Length;
  public NativeArray<T>.Enumerator GetEnumerator() => _array.GetEnumerator();
  public void Dispose() => _array.Dispose();
}