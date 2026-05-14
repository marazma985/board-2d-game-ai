using System;
using UnityEngine;
using UnityEngine.Events;

public class BoardTile : MonoBehaviour
{
    [Serializable]
    public sealed class BoardTileEvent : UnityEvent<BoardTile>
    {
    }

    [SerializeField, Min(0)] private int index;
    [SerializeField] private TileType tileType;
    [SerializeField] private BoardTileEvent entered = new BoardTileEvent();

    public int Index => index;
    public TileType TileType => tileType;
    public BoardTileEvent Entered => entered;

    public void Configure(int newIndex, TileType newTileType)
    {
        index = Mathf.Max(0, newIndex);
        tileType = newTileType;
        ApplyName();
    }

    public void Enter()
    {
        OnEnter();
        entered.Invoke(this);
    }

    protected virtual void OnEnter()
    {
    }

    private void Reset()
    {
        ApplyName();
    }

    private void OnValidate()
    {
        index = Mathf.Max(0, index);
        ApplyName();
    }

    private void ApplyName()
    {
        gameObject.name = $"Tile_{index:00}";
    }

    private void OnDrawGizmos()
    {
        var color = GetGizmoColor(tileType);
        color.a = 0.35f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(1.8f, 0.8f, 0.05f));

        color.a = 1f;
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.8f, 0.8f, 0.05f));
    }

    private static Color GetGizmoColor(TileType type)
    {
        switch (type)
        {
            case TileType.RandomEvent:
                return new Color(1f, 0.75f, 0.2f);
            case TileType.Debuff:
                return new Color(0.55f, 0.2f, 0.9f);
            case TileType.Battle:
                return new Color(1f, 0.25f, 0.2f);
            case TileType.RareEvent:
                return new Color(0.2f, 0.75f, 1f);
            case TileType.Buff:
                return new Color(0.25f, 0.85f, 0.35f);
            default:
                return Color.white;
        }
    }
}
