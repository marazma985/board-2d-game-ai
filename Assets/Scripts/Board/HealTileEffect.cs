using UnityEngine;

public sealed class HealTileEffect : ITileEffect
{
    public void Resolve(BoardTile tile)
    {
        Debug.Log("Heal tile resolved");
    }
}
