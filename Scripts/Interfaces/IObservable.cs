using System.Collections.Generic;

public interface IObservable<T>
{
    List<IObserver<T>> observers { get; }
    void AddObserver(IObserver<T> observer);
    void RemoveObserver(IObserver<T> observer);
    void NotifyObservers(T obj);
}