using System;

public class TimeEvent
{
    public string name;
    public Action action;
    public float time;
    public bool isRemainingTime;

    private bool wasInvoked;

    public TimeEvent(string name, Action action, float time, bool isRemainingTime)
    {
        this.name = name;
        this.action = action;
        this.time = time;
        this.isRemainingTime = isRemainingTime;
    }

    public void TryInvoke() 
    {
        if (!wasInvoked && action != null)
        {
            UnityEngine.Debug.Log("timeEvent " + name + " popped");
            action.Invoke();
            wasInvoked = true;
        }
    }

    public void Reset()
    {
        wasInvoked = false;
    }
}