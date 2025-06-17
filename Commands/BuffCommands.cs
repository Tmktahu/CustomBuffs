using VampireCommandFramework;
using CustomBuffs.Services;
using Unity.Entities;

namespace CustomBuffs.Commands;

[CommandGroup(name: "custombuffs", shortHand: "cbuff")]
internal class BuffCommands
{
  [Command("add", "Adds a buff to a player", adminOnly: true)]
  public static void AddBuffCommand(ChatCommandContext ctx, string buffId, string playerName)
  {
    if (!BuffService.AvailableBuffs.ContainsKey(buffId))
    {
      ctx.Reply($"Custom buff '{buffId}' not found");
      return;
    }

    Entity playerEntity;
    Entity userEntity;

    if (!EntityService.Instance.TryFindPlayer(playerName, out playerEntity, out userEntity))
    {
      ctx.Reply($"Player '{playerName}' not found");
      return;
    }

    BuffService.AddBuffToPlayer(playerEntity, buffId);
    ctx.Reply($"Added custom buff '{buffId}' to player '{playerName}'");
  }

  [Command("remove", "Removes a buff from a player", adminOnly: true)]
  public static void RemoveBuffCommand(ChatCommandContext ctx, string buffId, string playerName)
  {
    if (!BuffService.AvailableBuffs.ContainsKey(buffId))
    {
      ctx.Reply($"Custom buff '{buffId}' not found");
      return;
    }

    Entity playerEntity;
    Entity userEntity;

    if (!EntityService.Instance.TryFindPlayer(playerName, out playerEntity, out userEntity))
    {
      ctx.Reply($"Player '{playerName}' not found");
      return;
    }

    BuffService.RemoveBuffFromPlayer(playerEntity, buffId);
    ctx.Reply($"Removed custom buff '{buffId}' from player '{playerName}'");
  }

  [Command("list", "Lists all buffs for a player", adminOnly: true)]
  public static void ListBuffsCommand(ChatCommandContext ctx, string playerName)
  {
    // if a player name was not provided, then we want to list all buffs for all players
    if (string.IsNullOrEmpty(playerName))
    {
      ctx.Reply("Please provide a player name to list buffs for.");
      // TODO: Implement listing buffs for all players
      return;
    }

    Entity playerEntity;
    Entity userEntity;

    if (!EntityService.Instance.TryFindPlayer(playerName, out playerEntity, out userEntity))
    {
      ctx.Reply($"Player '{playerName}' not found");
      return;
    }

    BuffService.GetBuffsForPlayer(playerEntity, out var buffs);
    if (buffs.Count == 0)
    {
      ctx.Reply($"Player '{playerName}' has no custom buffs.");
      return;
    }

    // Format the buffs into a readable string
    buffs = buffs.Select(b => b.ToString()).ToList();

    ctx.Reply($"Custom buffs on '{playerName}': {string.Join(", ", buffs)}");
  }
}