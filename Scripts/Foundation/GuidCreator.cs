using System;

public class GuidCreator
{
    public static string New()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
