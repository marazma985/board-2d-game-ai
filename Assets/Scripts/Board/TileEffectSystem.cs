using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TileEffectSystem : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;

    private readonly HealTileEffect healTileEffect = new HealTileEffect();
    private readonly DebuffTileEffect debuffTileEffect = new DebuffTileEffect();
    private readonly BattleTileEffect battleTileEffect = new BattleTileEffect();
    private readonly EventTileEffect eventTileEffect = new EventTileEffect();
    private readonly RareTileEffect rareTileEffect = new RareTileEffect();
    private readonly BuffTileEffect buffTileEffect = new BuffTileEffect();
    private readonly Dictionary<TileType, ITileEffect> effectsByTileType = new Dictionary<TileType, ITileEffect>();

    public event Action<BoardTile> TileResolving;
    public event Action<BoardTile> TileResolved;

    public void RegisterEffect(TileType tileType, ITileEffect effect)
    {
        if (effect == null)
            return;

        effectsByTileType[tileType] = effect;
    }

    public void ResolveTile(BoardTile tile)
    {
        ResolveTile(tile, null);
    }

    public void ResolveTile(BoardTile tile, Action onResolved)
    {
        TileResolving?.Invoke(tile);

        if (tile != null)
        {
            tile.Enter();
            var effect = GetEffect(tile.TileType);
            if (effect is IDeferredTileEffect deferredTileEffect)
            {
                deferredTileEffect.Resolve(tile, () => CompleteTileResolution(tile, onResolved));
                return;
            }

            effect.Resolve(tile);
        }
        else
        {
            Debug.LogWarning("TileEffectSystem received null tile.");
        }

        CompleteTileResolution(tile, onResolved);
    }

    private void CompleteTileResolution(BoardTile tile, Action onResolved)
    {
        TileResolved?.Invoke(tile);
        onResolved?.Invoke();
    }

    private void Awake()
    {
        InitializeEffects();
    }

    private void OnValidate()
    {
        InitializeEffects();
    }

    private ITileEffect GetEffect(TileType tileType)
    {
        if (effectsByTileType.Count == 0)
            InitializeEffects();

        return effectsByTileType.TryGetValue(tileType, out var effect) ? effect : eventTileEffect;
    }

    private void InitializeEffects()
    {
        effectsByTileType.Clear();
        battleTileEffect.SetBattleSystem(battleSystem);
        RegisterEffect(TileType.RandomEvent, eventTileEffect);
        RegisterEffect(TileType.RareEvent, rareTileEffect);
        RegisterEffect(TileType.Battle, battleTileEffect);
        RegisterEffect(TileType.Buff, buffTileEffect);
        RegisterEffect(TileType.Debuff, debuffTileEffect);

        _ = healTileEffect;
    }
}
