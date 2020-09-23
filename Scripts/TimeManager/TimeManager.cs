using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour, IResetable, IObserver<bool>
{
    public static TimeManager Instance { get; private set; }

    public static readonly int defaultMinsToPlay = 15;

    public const double RealTimeToGameTime = 0.00208333333f;
    public const int CityDayHourCount = 10;
    private List<TimeEventGroup> eventGroups;

    public float updateTimeSec = 0.1f;
    private float t;

    public DateTime GameStart { get; private set; }
    private DateTime GameEnd;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        eventGroups = new List<TimeEventGroup>();
    }

    void Update()
    {
        t += Time.deltaTime;

        if(t >= updateTimeSec)
        {
            t = 0;
            CheckEvents();
        }
    }

    public void StartGameNow()
    {
        GameStart = DateTime.UtcNow;
        float minutesToPlay = GameManager.Instance.Player.Avatar.MinutesToPlay;
        GameTimeDisplayParent.Instance.Initiate(minutesToPlay);
        
        //copy and add game time
        GameEnd = DateTime.UtcNow.AddMinutes(minutesToPlay);
    }

    public void StartGameForViewer()
    {
        GameStart = DateTime.UtcNow;
    }

    public float GetRemainingDays()
    {
        return (float)GameMinsToCityDays(GetRemainingGameTimeMins());
    }

    public float GetRemainingDays(float otherMinsToPlay)
    {
        return (float)GameMinsToCityDays(RemainMinsInTimeFrame(GameStart, otherMinsToPlay));
    }

    public float GetRemainingHours()
    {
        return GetRemainingDays() * CityDayHourCount;
    }

    public float GetRemainingGameTimeMins()
    {
        return (float)RemainMinsInTimeFrame(DateTime.UtcNow, GameEnd);
    }

    private void CheckEvents()
    {
        DateTime timestamp = DateTime.UtcNow;

        eventGroups.ForEach(g => g.CheckEvents(timestamp));
    }
    
    public static double RealHrsToGameSec(double hours)
    {
        // ordering to keep number in ~ double range
        return hours * 60 * RealTimeToGameTime * 60;
    }

    public static double CityDaysToGameSec(int days)
    {
        return days * CityDayHourCount * 60 * RealTimeToGameTime * 60;
    }

    public static double GameMinsToCityDays(double mins)
    {
        // ordering to keep number in ~ double range
        return mins / 60 / RealTimeToGameTime / CityDayHourCount;
    }

    public static double RemainMinsInTimeFrame(DateTime start, double durationMins)
    {
        DateTime startCopy = start;
        return RemainMinsInTimeFrame(DateTime.UtcNow, startCopy.AddMinutes(durationMins));
    }
    public static double RemainMinsInTimeFrame(DateTime current, DateTime end)
    {
        return (end - current).TotalMinutes;
    }

    public static (double passedMins, double remainMins) MinsInTimeFrame(DateTime start, double durationMins)
    {
        DateTime startCopy = start;
        return MinsInTimeFrame(start, DateTime.UtcNow, startCopy.AddMinutes(durationMins));
    }

    public static (double passedMins, double remainMins) MinsInTimeFrame(DateTime start, DateTime current, DateTime end)
    {
        double passedMins = (current - start).TotalMinutes;
        double remainMins = (end - current).TotalMinutes;

        return (passedMins, remainMins);
    }

    public void RegisterEventGroup(TimeEventGroup group)
    {
        if(group != null)
            eventGroups.Add(group);
    }

    public void CancelEventGroup(string name)
    {
        TimeEventGroup foundGroup = eventGroups.Find(g => g.name.Equals(name));
        if (foundGroup != null)
            eventGroups.Remove(foundGroup);
    }

    public TimeEventGroup Find(string name)
    {
        return eventGroups.Find(g => g.name.Equals(name));
    }

    public void Reset()
    {
        if(eventGroups != null)
        {
            foreach (TimeEventGroup group in eventGroups)
            {
                eventGroups.Remove(group);
            }
            eventGroups.Clear();
        }    
    }

    public void ObserverUpdate(bool shouldReset)
    {
        Reset();
    }
}
