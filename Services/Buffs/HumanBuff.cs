using ProjectM;
using Unity.Entities;
using Stunlock.Core;
using CustomBuffs.Resources;

namespace CustomBuffs.Services.Buffs;

internal static class HumanBuff
{
  public static readonly PrefabGUID HumanBuffBase = PrefabGUIDs.SetBonus_Silk_Twilight;
  // This buff comes with prebuilt sun immunity. It use to make you twinkle, but no longer does.

  public static bool ApplyBuff(Entity entity)
  {
    bool hasBuff = BuffService.HasBuff(entity, HumanBuffBase);

    if (!hasBuff)
    {
      // Apply the buff
      BuffService.ApplyBuff(entity, HumanBuffBase);

      if (!BuffService.TryGetBuff(entity, HumanBuffBase, out Entity buffEntity))
      {
        // there was a problem applying the buff
        return false;
      }

      var buffer = BuffService.SetupBuffer(buffEntity, entity);

      // Add garlic and silver resistance to the buffer
      ModifyUnitStatBuff_DOTS newGarlicResStatBuff = new ModifyUnitStatBuff_DOTS
      {
        StatType = UnitStatType.GarlicResistance,
        ModificationType = ModificationType.Add,
        AttributeCapType = AttributeCapType.Uncapped,
        Value = 1000f,
        Modifier = 1,
        IncreaseByStacks = false,
        ValueByStacks = 0,
        Priority = 0,
        Id = ModificationIDs.Create().NewModificationId()
      };
      buffer.Add(newGarlicResStatBuff);

      ModifyUnitStatBuff_DOTS newSilverResStatBuff = new ModifyUnitStatBuff_DOTS
      {
        StatType = UnitStatType.SilverResistance,
        ModificationType = ModificationType.Add,
        AttributeCapType = AttributeCapType.Uncapped,
        Value = 1000f,
        Modifier = 1,
        IncreaseByStacks = false,
        ValueByStacks = 0,
        Priority = 0,
        Id = ModificationIDs.Create().NewModificationId()
      };
      buffer.Add(newSilverResStatBuff);
      return true;
    }
    else
    {
      // we did not apply the buff because it already exists
      return false;
    }
  }

  public static bool RemoveBuff(Entity entity)
  {
    return BuffService.RemoveBuff(entity, HumanBuffBase);
  }

  public static bool HasBuff(Entity entity)
  {
    return BuffService.HasBuff(entity, HumanBuffBase);
  }
}