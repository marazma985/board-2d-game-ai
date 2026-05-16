using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class BattleSystem : MonoBehaviour
{
    private const string PlayerName = "Карамелька";

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private BattleModalView battleModalView;
    [SerializeField] private List<EnemyData> enemies = new List<EnemyData>();
    [SerializeField, Min(0)] private int equipmentBonus;
    [SerializeField, Min(0)] private int cardBonus;
    [SerializeField, Min(0)] private int diceBonus;

    private BattleModalData currentBattleData;
    private Action battleCompleted;

    public bool IsBattleActive => currentBattleData != null;

    public void StartBattle()
    {
        StartBattle(null);
    }

    public void StartBattle(Action onBattleCompleted)
    {
        if (IsBattleActive)
        {
            Debug.LogWarning("Battle is already active.");
            return;
        }

        if (playerStats == null)
        {
            Debug.LogWarning("BattleSystem requires PlayerStats.");
            onBattleCompleted?.Invoke();
            return;
        }

        if (battleModalView == null)
        {
            Debug.LogWarning("BattleSystem requires BattleModalView.");
            onBattleCompleted?.Invoke();
            return;
        }

        var enemy = SelectRandomEnemy();
        if (enemy == null)
        {
            Debug.LogWarning("BattleSystem has no enemies to start battle.");
            onBattleCompleted?.Invoke();
            return;
        }

        battleCompleted = onBattleCompleted;
        currentBattleData = CreateBattleData(enemy);
        battleModalView.Show(currentBattleData);
    }

    private void OnEnable()
    {
        if (battleModalView != null)
            battleModalView.ResolveRequested += ResolveCurrentBattle;
    }

    private void OnDisable()
    {
        if (battleModalView != null)
            battleModalView.ResolveRequested -= ResolveCurrentBattle;
    }

    public void ResolveCurrentBattle()
    {
        if (currentBattleData == null)
            return;

        if (currentBattleData.PlayerTotalPower > currentBattleData.EnemyTotalPower)
        {
            Debug.Log($"Battle won: {currentBattleData.PlayerName} defeated {currentBattleData.EnemyName}.");
        }
        else
        {
            Debug.Log($"Battle lost: {currentBattleData.PlayerName} failed against {currentBattleData.EnemyName}.");
        }

        battleModalView.Hide();
        currentBattleData = null;

        var onCompleted = battleCompleted;
        battleCompleted = null;
        onCompleted?.Invoke();
    }

    private EnemyData SelectRandomEnemy()
    {
        if (enemies == null || enemies.Count == 0)
            return null;

        var validEnemies = new List<EnemyData>();
        for (var i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                validEnemies.Add(enemies[i]);
        }

        if (validEnemies.Count == 0)
            return null;

        return validEnemies[UnityEngine.Random.Range(0, validEnemies.Count)];
    }

    private BattleModalData CreateBattleData(EnemyData enemy)
    {
        var playerEntries = new List<BattlePowerEntry>
        {
            new BattlePowerEntry("Level", playerStats.Level),
            new BattlePowerEntry("Equipment bonus", equipmentBonus)
        };

        if (diceBonus > 0)
            playerEntries.Add(new BattlePowerEntry("Dice bonus", diceBonus));

        if (cardBonus > 0)
            playerEntries.Add(new BattlePowerEntry("Card bonuses", cardBonus));

        var enemyEntries = new List<BattlePowerEntry>
        {
            new BattlePowerEntry("Monster level", enemy.Level),
            new BattlePowerEntry("Monster bonuses", enemy.BonusPower)
        };

        return new BattleModalData
        {
            PlayerName = PlayerName,
            PlayerSprite = playerSprite,
            PlayerPowerEntries = playerEntries,
            PlayerTotalPower = SumEntries(playerEntries),
            EnemyName = enemy.EnemyName,
            EnemySprite = enemy.EnemySprite,
            EnemyPowerEntries = enemyEntries,
            EnemyTotalPower = SumEntries(enemyEntries)
        };
    }

    private static int SumEntries(IReadOnlyList<BattlePowerEntry> entries)
    {
        var total = 0;
        for (var i = 0; i < entries.Count; i++)
            total += entries[i].Value;

        return total;
    }
}
