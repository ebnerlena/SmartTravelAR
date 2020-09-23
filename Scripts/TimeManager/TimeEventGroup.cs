using System;
using System.Collections.Generic;

public class TimeEventGroup
{
    public string name { get; }
    public float totalTime { get; private set; }
    public bool isRunning { get; private set; }

    private List<TimeEvent> events;
    private DateTime startTime;

    public TimeEventGroup(string name, float time)
    {
        this.name = name;
        this.totalTime = time;
        isRunning = false;
        events = new List<TimeEvent>();
    }

    public void CheckEvents(DateTime timeStamp)
    {
        if (!isRunning)
            return;

        double passedSeconds = GetPassedSeconds(timeStamp);
        double remainingSeconds = GetRemainingSeconds(timeStamp);

        foreach (TimeEvent timeEvent in events)
        {
            if (timeEvent.isRemainingTime && remainingSeconds <= timeEvent.time)
                timeEvent.TryInvoke();
            else if (!timeEvent.isRemainingTime && passedSeconds >= timeEvent.time)
                timeEvent.TryInvoke();
        }
    }

    public float GetRemainingSeconds()
    {
        return GetRemainingSeconds(DateTime.UtcNow);
    }

    public float GetPassedSeconds()
    {
        return GetPassedSeconds(DateTime.UtcNow);
    }

    private float GetRemainingSeconds(DateTime timeStamp)
    {
        if (!isRunning)
            return totalTime;

        double secondsPassed = (timeStamp - startTime).TotalSeconds;
        return (float)(totalTime - secondsPassed);
    }

    private float GetPassedSeconds(DateTime timeStamp)
    {
        if (!isRunning)
            return 0f;

        return (float)(timeStamp - startTime).TotalSeconds;
    }


    public void Start()
    {
        if(!isRunning)
        {
            startTime = DateTime.UtcNow;
            isRunning = true;
        }
    }

    public void RegisterEvent(TimeEvent timeEvent)
    {
        if(timeEvent != null)
            events.Add(timeEvent);
    }

    public void CancelEvent(string name)
    {
        TimeEvent foundEvent = events.Find(e => e.name.Equals(name));
        if (foundEvent != null)
            events.Remove(foundEvent);
    }

    public void AdaptTotalTime(float newTime)
    {
        if(newTime > 0)
            this.totalTime = newTime;
    }
}