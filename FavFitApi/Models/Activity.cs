using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FavFitApi.Enums;

namespace FavFitApi.Models;

[Table("activities")]
public class Activity
{
    [Key]
    [Column("id")]
    public long Id {get; set;}

    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public long UserId {get; set;}

    [Column("title")]
    public string? Title {get; set;}

    [Column("type")]
    public ActivityType Type {get; set;}

    [Column("date")]
    public DateTime Date {get; set;}

    [Column("elapsed_time")]
    public TimeSpan ElapsedTime {get; set;}

    [Column("distance")]
    public double Distance {get; set;}

    [Column("average_speed")]
    public double AverageSpeed {get; set;}

    [Column("average_cadence")]
    public int AverageCadence {get; set;}

    [Column("average_pace")]
    public TimeSpan AveragePace {get; set;}

    [Column("average_heart_rate")]
    public int AverageHeartRate {get; set;}

    [Column("elevation_gain")]
    public double ElevationGain {get; set;}

    [Column("calories")]
    public int Calories {get; set;}

    [Column("created_at")]
    public DateTime CreatedAt {get; set;}
}



