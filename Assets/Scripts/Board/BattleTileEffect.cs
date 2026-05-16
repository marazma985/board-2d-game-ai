using UnityEngine;

public sealed class BattleTileEffect : ITileEffect
{
    public void Resolve(BoardTile tile)
    {
        Debug.Log("Battle tile resolved");
    }
}
