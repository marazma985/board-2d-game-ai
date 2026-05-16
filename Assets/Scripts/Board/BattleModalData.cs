using System.Collections.Generic;
using UnityEngine;

public sealed class BattleModalData
{
    public string PlayerName { get; set; }
    public Sprite PlayerSprite { get; set; }
    public IReadOnlyList<BattlePowerEntry> PlayerPowerEntries { get; set; }
    public int PlayerTotalPower { get; set; }
    public string EnemyName { get; set; }
    public Sprite EnemySprite { get; set; }
    public IReadOnlyList<BattlePowerEntry> EnemyPowerEntries { get; set; }
    public int EnemyTotalPower { get; set; }
}
