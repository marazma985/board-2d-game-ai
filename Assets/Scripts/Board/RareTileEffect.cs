using UnityEngine;

public sealed class RareTileEffect : ITileEffect
{
    public void Resolve(BoardTile tile)
    {
        Debug.Log("Rare tile resolved");
    }
}
