using UnityEngine;

public sealed class EventTileEffect : ITileEffect
{
    public void Resolve(BoardTile tile)
    {
        Debug.Log("Event tile resolved");
    }
}
