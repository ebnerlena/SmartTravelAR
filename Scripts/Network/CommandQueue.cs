using System;

public class CommandQueue
{
    private static object gameLock = new object();

    public static void Queue(ICommand command, string name)
    {
        //UnityEngine.Debug.Log("executing " + name);
        lock (gameLock)
        {
            command.Execute();
        }
        //UnityEngine.Debug.Log("done with " + name);
    }

    public static void Queue(Action action)
    {
        lock (gameLock)
        {
            action?.Invoke();
        }
    }
}