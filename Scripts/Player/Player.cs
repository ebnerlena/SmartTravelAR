using System.Collections.Generic;
using System;
using System.Linq;

public class Player : EnemyPlayer, IObservable<Dictionary<Type, Resource>>
{
    public Trip Trip { get; }
    public bool isViewer;

    public List<IObserver<Dictionary<Type, Resource>>> observers { get; private set; }

    public Player(string playerId) : base(playerId)
    {
        this.playerId = playerId;
        this.Trip = new Trip();
        observers = new List<IObserver<Dictionary<Type, Resource>>>();
        isViewer = false;
    }

    public override void CreateResources()
    {
        base.CreateResources();
        NotifyObservers(Resources);
    }

    public void AddCityStayWithPkg(City city, SightseeingPackage package)
    {
        CityStay newStay = new CityStay(city, package.days);
        Trip.AddStay(newStay);

        if (Trip.arrivedInTime)
            CulturePoints += package.culturePoints;

        UsePackageResources(package);
    }

    private void UsePackageResources(SightseeingPackage package)
    {
        foreach(KeyValuePair<Type,float> entry in package.resources)
        {
            if (Resources.ContainsKey(entry.Key))
                Resources[entry.Key].Use(entry.Value);
        }
        OnResourceUpdate();        
    }
    public void UseTransportResources()
    {
        TransportType transportType = Trip.CurrentTransport.Option.TransportType;
        foreach (KeyValuePair<Type,Resource> resource in Resources)
        {
            if (transportType.ResourceCostsPerDistance.ContainsKey(resource.Key))
            {
                resource.Value.Use(transportType.ResourceCostsPerDistance[resource.Key]);
            }     
        }
        OnResourceUpdate();
    }

    private void OnResourceUpdate()
    {
        NotifyObservers(Resources);
        DictionaryStripper.ExtractResourcesDic(Resources, out string[] resNames, out float[] rawValues);
        GameManager.Instance.NetworkPlayer?.SendOnly(new ScoreUpdateRequestMessageable(playerId, TimeManager.Instance.GetRemainingDays(), CulturePoints, resNames, rawValues));
    }

    public void ValidTripFinish(float points)
    {
        CulturePoints += points;
    }

    public void AnsweredQuestionCorrect()
    {
        CulturePoints += 1;
    }

    public void AddObserver(IObserver<Dictionary<Type, Resource>> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(IObserver<Dictionary<Type, Resource>> observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    public void NotifyObservers(Dictionary<Type, Resource> obj)
    {
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            observers[i].ObserverUpdate(obj);
        }
    }
    private void UseCityStayResources(CityStay stay)
    {
        float days = stay.DaysStaytime;
        float costs;

        CulturePoints += Trip.CurrentCity.CulturePointsPer12Hrs * 2 * days;
        CulturePoints += UnityEngine.Random.Range(-2, 3); // + or - 2 points randomly

        if (days > 1)
            costs = days * stay.City.CostsPerNight[Avatar.AvatarType];
        else
            costs = stay.City.CostsPerDay[Avatar.AvatarType];

        Resources[typeof(MoneyResource)].Use(costs);
        NotifyObservers(Resources);
    }
}
