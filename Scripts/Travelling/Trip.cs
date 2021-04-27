using System.Collections.Generic;
using System.Linq;

public class Trip : IResetable, IObserver<bool>
{
    public List<Transport> transportHistory { get; private set; }
    public Transport CurrentTransport { get; set; }
    public City CurrentCity { get; set; }
    public bool arrivedInTime;

    private List<CityStay> route;

    public Trip()
    {
        this.route = new List<CityStay>();
        this.transportHistory = new List<Transport>();
    }

    public void Start()
    {
        CurrentTransport?.Start();
        transportHistory.Add(CurrentTransport);
    }

    public void AddStay(CityStay stay)
    {
        route.Add(stay);
    }

    public void ArrivedNotInTime()
    {
        CurrentCity = CurrentTransport.To.City;
        arrivedInTime = false;
    }

    public void ArrivedInTime()
    {
        CurrentCity = CurrentTransport.To.City;
        arrivedInTime = true;
    }

    public bool IsCompleted()
    {
        if (CurrentCity != GraphGenerator.GetCity("Salzburg"))
            return false;
        
        // skipping Salzburg, then check if count matches
        return GraphGenerator.cities.Count(k => k.Key != "Salzburg") == route.Count;
    }

    public bool HasAlreadyVistedCurrentCity()
    {
        if (CurrentCity == GraphGenerator.GetCity("Salzburg"))
            return true;

        return route.Exists(cityStay => cityStay.City == CurrentCity);
    }

    public void Reset()
    {
        route.Clear();
        transportHistory.Clear();
        CurrentCity = GraphGenerator.GetCity("Salzburg");
    }

    public void ObserverUpdate(bool shouldReset)
    {
        Reset();
    }
}
