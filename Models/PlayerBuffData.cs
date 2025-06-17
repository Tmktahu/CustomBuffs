namespace CustomBuffs.Models;

public class PlayerBuffData
{
  public Dictionary<string, List<string>> PlayerBuffs { get; set; } = new Dictionary<string, List<string>>();

  public void AddBuffToPlayer(string playerId, string buffName)
  {
    if (!PlayerBuffs.ContainsKey(playerId))
    {
      PlayerBuffs[playerId] = new List<string>();
    }

    if (!PlayerBuffs[playerId].Contains(buffName))
    {
      PlayerBuffs[playerId].Add(buffName);
    }
  }

  public void RemoveBuffFromPlayer(string playerId, string buffName)
  {
    if (PlayerBuffs.ContainsKey(playerId) && PlayerBuffs[playerId].Contains(buffName))
    {
      PlayerBuffs[playerId].Remove(buffName);
    }
  }

  public List<string> GetPlayerBuffs(string playerId)
  {
    if (PlayerBuffs.ContainsKey(playerId))
    {
      return PlayerBuffs[playerId];
    }
    return new List<string>();
  }
}