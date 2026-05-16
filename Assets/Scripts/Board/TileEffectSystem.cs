using System;
using UnityEngine;

public sealed class TileEffectSystem : MonoBehaviour
{
    public event Action<BoardTile> TileResolving;
    public event Action<BoardTile> TileResolved;

    public void ResolveTile(BoardTile tile, Action onResolved)
    {
        TileResolving?.Invoke(tile);

        if (tile != null)
            tile.Enter();

        TileResolved?.Invoke(tile);
        onResolved?.Invoke();
    }
}
