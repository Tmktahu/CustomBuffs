using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;

namespace CustomBuffs.Services;

public class EntityService
{
  private static EntityService _instance;
  public static EntityService Instance => _instance ??= new EntityService();

  public bool TryFindPlayer(string playerName, out Entity playerEntity, out Entity userEntity)
  {
    var em = Core.EntityManager;
    playerEntity = Entity.Null;
    userEntity = Entity.Null;

    var userEntities = em.CreateEntityQuery(ComponentType.ReadOnly<User>())
                       .ToEntityArray(Allocator.Temp);

    foreach (var entity in userEntities)
    {
      var userData = em.GetComponentData<User>(entity);
      if (userData.CharacterName.ToString().Equals(playerName, StringComparison.OrdinalIgnoreCase))
      {
        userEntity = entity;
        playerEntity = userData.LocalCharacter._Entity;
        return true;
      }
    }

    return false;
  }
}