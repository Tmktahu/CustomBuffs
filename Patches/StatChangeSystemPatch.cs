using CustomBuffs.Services;
using HarmonyLib;
using ProjectM;
using ProjectM.Gameplay.Systems;
using ProjectM.Scripting;
using Stunlock.Core;
using System.Collections.Concurrent;
using Unity.Collections;
using Unity.Entities;
using static CustomBuffs.Services.BuffService;

namespace CustomBuffs.Patches;

[HarmonyPatch]
internal static class StatChangeSystemPatch
{
    static readonly ConcurrentDictionary<ulong, DateTime> _lastDamageTime = [];
    static readonly PrefabGUID _holyDebuffT1 = new(1593142604); //  Prefab 'PrefabGuid(1593142604)': Buff_General_Holy_Area_T01
    static readonly PrefabGUID _holyDebuffT2 = new(-621774510); //  Prefab 'PrefabGuid(-621774510)': Buff_General_Holy_Area_T02

    [HarmonyPatch(typeof(StatChangeSystem), nameof(StatChangeSystem.OnUpdate))]
    [HarmonyPrefix]
    static bool OnUpdatePrefix(StatChangeSystem __instance)
    {
        if (!Core._initialized) return true;

        var entityArray = __instance._Query.ToEntityArray(Allocator.Temp);

        try
        {
            EntityManager entityManager = Core.EntityManager;

            for (int i = 0; i < entityArray.Length; i++)
            {
                Entity entity = entityArray[i];
                StatChangeEvent statChangeEvent = entityManager.GetComponentData<StatChangeEvent>(entity);

                PrefabGUID sourcePrefabGuid = statChangeEvent.Source.GetPrefabGuid();

                // if it's either of the holy debuffs, we don't want to process it further
                if (sourcePrefabGuid.Equals(_holyDebuffT1) || sourcePrefabGuid.Equals(_holyDebuffT2))
                {
                    Entity target = statChangeEvent.Entity; // this is the target of the stat change
                    if (HasBuff(target, Services.Buffs.HumanBuff.HumanBuffBase))
                    {
                        entity.Destroy(true);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Core.Log.LogWarning($"[StatChangeSystem] Exception: {e}");
        }

        return true; // Continue with the original method
    }
}