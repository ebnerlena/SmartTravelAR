using System;
using System.Collections.Generic;

public class PlayerListManager : IObservable<int>, IResetable, IObserver<bool>
{
    public List<EnemyPlayer> PlayerList { get; private set; }

    public List<IObserver<int>> observers { get; }

    public PlayerListManager()
    {
        PlayerList = new List<EnemyPlayer>();
        observers = new List<IObserver<int>>();
    }

    public void AddEnemyPlayers(params PlayerInfo[] enemies)
    {
        foreach(PlayerInfo enemy in enemies)
        {
            EnemyPlayer newEnemy = new EnemyPlayer(enemy.id, enemy.name, enemy.avatarType);
            newEnemy.CreateResources();
            PlayerList.Add(newEnemy);
            UnityEngine.Debug.Log("Added player " + enemy.name+ ", now at count: "+PlayerList.Count);
        }
        NotifyObservers(PlayerList.Count);
    }

    public void AddLocalPlayer(EnemyPlayer enemy)
    {
        PlayerList.Add(enemy);
        UnityEngine.Debug.Log("Added player " + enemy.Name + ", now at count: " + PlayerList.Count);
        NotifyObservers(PlayerList.Count);
    }

    /// <summary>
    /// Returns the rank between 1 and playerCount
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetRankOfPlayer(EnemyPlayer player)
    {
        if (player == null)
            return -1;

        SortByScore();
        return PlayerList.IndexOf(player) + 1;
    }

    public int GetRankOfPlayer(string playerId)
    {
        // ignoring found = null since this gets handled in main func anyways
        return GetRankOfPlayer(PlayerList.Find(p => p.playerId.Equals(playerId)));
    }

    public List<EnemyPlayer> GetTop3()
    {
        //Just for testing without players
        if (PlayerList.Count< 3)
        {
            return PlayerList.GetRange(0, PlayerList.Count);
        }
        else
            return PlayerList.GetRange(0, 3);

    }

    public void SortByScore()
    {
        PlayerList.Sort((p1, p2) => p1.CurrentScore.CompareTo(p2.CurrentScore));
    }

    public void UpdatePlayer(ScoreUpdateMessageable msg)
    {
        EnemyPlayer p = PlayerList.Find(e => e.playerId.Equals(msg.playerId));
        if (p != null)
        {
            p.UpdateScore(msg.score);
            p.UpdateCulturePoints(msg.culturePoints);
            p.UpdateMoneyAndCo2(msg.resourceNames, msg.rawValues);

            UnityEngine.Debug.Log("updated score of " + p.Name + " to " + msg.score);
            NotifyObservers(PlayerList.Count);
        }
    }

    public void UpdateCityOfPlayer(string playerId, string cityName)
    {
        PlayerList.Find(e => e.playerId.Equals(playerId))?.UpdateCurCityName(cityName);
    }

    public void AddObserver(IObserver<int> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(IObserver<int> observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    public void NotifyObservers(int msg)
    {
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            observers[i].ObserverUpdate(msg);
        }
    }

    public void ObserverUpdate(bool shouldReset)
    {
        Reset();
    }

    public void Reset()
    {
        PlayerList.Clear();
    }
}