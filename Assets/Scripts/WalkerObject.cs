using UnityEngine;

public class WalkerObject
{
    public Vector2Int Position;
    public Vector2 Direction;
    public float ChanceToChange;

    public WalkerObject(Vector2Int position, Vector2 direction, float chanceToChange)
    {
        Position = position;
        Direction = direction;
        ChanceToChange = chanceToChange;
    }
}
