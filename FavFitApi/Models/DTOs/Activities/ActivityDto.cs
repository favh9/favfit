using FavFitApi.Enums;

namespace FavFitApi.Models;

public class ActivityDto
{
    public long Id {get; set;}

    public long UserId {get; set;}

    public string? Title {get; set;}

    public ActivityType Type {get; set;}

    public DateTime Date {get; set;}

    public TimeSpan ElapsedTime {get; set;}

    public double Distance {get; set;}

    public double AverageSpeed {get; set;}

    public int AverageCadence {get; set;}

    public TimeSpan AveragePace {get; set;}

    public int AverageHeartRate {get; set;}

    public double ElevationGain {get; set;}

    public int Calories {get; set;}

    public DateTime CreatedAt {get; set;}
}