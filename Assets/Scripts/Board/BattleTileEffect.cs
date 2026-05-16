using UnityEngine;

public sealed class BattleTileEffect : IDeferredTileEffect
{
    private BattleSystem battleSystem;

    public void SetBattleSystem(BattleSystem newBattleSystem)
    {
        battleSystem = newBattleSystem;
    }

    public void Resolve(BoardTile tile)
    {
        Resolve(tile, null);
    }

    public void Resolve(BoardTile tile, System.Action onResolved)
    {
        if (battleSystem == null)
        {
            Debug.LogWarning("Battle tile resolved, but BattleSystem is not assigned.");
            onResolved?.Invoke();
            return;
        }

        battleSystem.StartBattle(onResolved);
    }
}
