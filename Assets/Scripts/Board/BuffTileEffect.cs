using UnityEngine;

public sealed class BuffTileEffect : ITileEffect
{
    public void Resolve(BoardTile tile)
    {
        Debug.Log("Buff tile resolved");
    }
}
