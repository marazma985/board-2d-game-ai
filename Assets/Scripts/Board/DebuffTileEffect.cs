using UnityEngine;

public sealed class DebuffTileEffect : ITileEffect
{
    public void Resolve(BoardTile tile)
    {
        Debug.Log("Debuff tile resolved");
    }
}
