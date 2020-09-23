public class CityStay
{
    public City City { get; }

    public float DaysStaytime { get; set; }

    public CityStay(City city, float daysStaytime = 1)
    {
        this.City = city;
        this.DaysStaytime = daysStaytime;
    }
}
