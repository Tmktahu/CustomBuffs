using Unity.Entities;
using Stunlock.Core;
using ProjectM.Scripting;
using ProjectM.Network;
using ProjectM;
using ProjectM.Shared;

namespace CustomBuffs.Services;

public static class BuffService
{
  static EntityManager EntityManager => Core.EntityManager;
  static ServerGameManager ServerGameManager => Core.ServerGameManager;
  static SystemService SystemService => Core.SystemService;
  static DebugEventsSystem DebugEventsSystem => SystemService.DebugEventsSystem;

  public static string HUMAN_BUFF = "human_buff";

  public static Dictionary<string, BuffActions> AvailableBuffs = new()
{
    {
        HUMAN_BUFF,
        new BuffActions(
            applyBuff: entity => Buffs.HumanBuff.ApplyBuff(entity),
            removeBuff: entity => Buffs.HumanBuff.RemoveBuff(entity),
            hasBuff: entity => Buffs.HumanBuff.HasBuff(entity)
        )
    }
};

  public static void Initialize()
  {

  }

  // Apply a buff to an entity
  public static bool ApplyBuff(Entity entity, PrefabGUID buffPrefabGuid)
  {
    // Check if entity already has this buff
    bool hasBuff = HasBuff(entity, buffPrefabGuid);

    if (!hasBuff)
    {
      // Create buff application event
      ApplyBuffDebugEvent applyBuffDebugEvent = new()
      {
        BuffPrefabGUID = buffPrefabGuid,
        Who = entity.GetNetworkId()
      };

      // Create character reference
      FromCharacter fromCharacter = new()
      {
        Character = entity,
        User = entity.IsPlayer() ? entity.GetUserEntity() : entity
      };

      // Apply the buff
      DebugEventsSystem.ApplyBuff(fromCharacter, applyBuffDebugEvent);
      return true;
    }

    return false;
  }

  public static bool HasBuff(Entity entity, PrefabGUID buffPrefabGuid)
  {
    return ServerGameManager.HasBuff(entity, buffPrefabGuid.ToIdentifier());
  }

  public static DynamicBuffer<ModifyUnitStatBuff_DOTS> SetupBuffer(Entity buffEntity, Entity playerCharacter)
  {
    if (buffEntity.TryGetBuffer<SyncToUserBuffer>(out var syncToUsers))
    {
      if (syncToUsers.IsEmpty)
      {
        // Core.Log.LogInfo($"Adding sync buffer for garlic resistance to {buffEntity}");
        SyncToUserBuffer syncToUserBuffer = new()
        {
          UserEntity = playerCharacter.GetUserEntity()
        };
        syncToUsers.Add(syncToUserBuffer);
      }
    }

    // var prefabGuid = buffEntity.Read<PrefabGUID>();
    // Core.Log.LogInfo($"Applying game buff: {prefabGuid}");

    if (!buffEntity.TryGetBuffer<ModifyUnitStatBuff_DOTS>(out var buffer))
    {
      // Core.Log.LogInfo($"Creating new stat buffer for garlic resistance on {buffEntity}");
      buffer = EntityManager.AddBuffer<ModifyUnitStatBuff_DOTS>(buffEntity);

    }

    return buffer;
  }

  public static bool TryGetBuff(Entity entity, PrefabGUID buffPrefabGUID, out Entity buffEntity)
  {
    if (ServerGameManager.TryGetBuff(entity, buffPrefabGUID.ToIdentifier(), out buffEntity))
    {
      return true;
    }

    return false;
  }

  public static bool RemoveBuff(Entity entity, PrefabGUID buffPrefabGuid)
  {
    TryRemoveBuff(entity, buffPrefabGuid);
    // Core.Log.LogInfo($"Removed buff of type {buffPrefabGuid} from entity {entity}.");
    return true;
  }

  public static void TryRemoveBuff(Entity entity, PrefabGUID buffPrefabGuid)
  {
    if (TryGetBuff(entity, buffPrefabGuid, out Entity buffEntity))
    {
      buffEntity.DestroyBuff();
    }
  }

  public static void DestroyBuff(this Entity buffEntity)
  {
    if (buffEntity.Exists()) DestroyUtility.Destroy(EntityManager, buffEntity, DestroyDebugReason.TryRemoveBuff);
  }

  public static List<string> GetAvailableBuffs()
  {
    return AvailableBuffs.Keys.ToList();
  }

  public static bool AddBuffToPlayer(Entity characterEntity, string buffId)
  {
    if (AvailableBuffs.TryGetValue(buffId, out var buffActions))
    {
      buffActions.ApplyBuff(characterEntity);
      return true;
    }

    return false;
  }

  public static bool RemoveBuffFromPlayer(Entity characterEntity, string customBuffId)
  {
    if (AvailableBuffs.TryGetValue(customBuffId, out var buffActions))
    {
      buffActions.RemoveBuff(characterEntity);
      return true;
    }

    return false;
  }

  public static void GetBuffsForPlayer(Entity playerEntity, out List<string> buffs)
  {
    buffs = new List<string>();

    // The way we do this is we iterate over all our available buffs and check if the player has them
    foreach (var customBuffId in AvailableBuffs.Keys)
    {
      var buffActions = AvailableBuffs[customBuffId];
      if (buffActions.HasBuff(playerEntity))
      {
        buffs.Add(customBuffId);
      }
    }
  }
}

public class BuffActions
{
  public Func<Entity, bool> ApplyBuff { get; }
  public Func<Entity, bool> RemoveBuff { get; }
  public Func<Entity, bool> HasBuff { get; }

  public BuffActions(Func<Entity, bool> applyBuff, Func<Entity, bool> removeBuff, Func<Entity, bool> hasBuff)
  {
    ApplyBuff = applyBuff;
    RemoveBuff = removeBuff;
    HasBuff = hasBuff;
  }
}