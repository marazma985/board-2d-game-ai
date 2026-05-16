using System;

public interface IDeferredTileEffect : ITileEffect
{
    void Resolve(BoardTile tile, Action onResolved);
}
