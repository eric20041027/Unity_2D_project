using UnityEngine;

public class TurnManager
{
    public event System.Action OnTick;
    private int turnCount;
    public TurnManager()
    {
        turnCount = 1;
    }
    public void Tick()
    {
        turnCount += 1;
        Debug.Log("Turn Round : " + turnCount);
        OnTick?.Invoke(); // ? ==> implementing the function if not null
    }
}
