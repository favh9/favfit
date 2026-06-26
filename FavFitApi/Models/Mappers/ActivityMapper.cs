
namespace FavFitApi.Models;

public class ActivityMapper
{

    public Activity ToActivity(CreateActivityDto createActivityDto)
    {
        
        var newActivity = new Activity
        {
            UserId = createActivityDto.UserId,
            Title = createActivityDto.Title,
            Type = createActivityDto.Type,
            Date = createActivityDto.Date,
            ElapsedTime = createActivityDto.ElapsedTime,
            Distance = createActivityDto.Distance,
            AverageSpeed = createActivityDto.AverageSpeed,
            AverageCadence = createActivityDto.AverageCadence,
            AveragePace = createActivityDto.AveragePace,
            AverageHeartRate = createActivityDto.AverageHeartRate,
            ElevationGain = createActivityDto.ElevationGain,
            Calories = createActivityDto.Calories
        };

        return newActivity;
    }

    public ActivityDto ToActivityDto(Activity activity)
    {
        
        var activityDto = new ActivityDto
        {
            UserId = activity.UserId,
            Title = activity.Title,
            Type = activity.Type,
            Date = activity.Date,
            ElapsedTime = activity.ElapsedTime,
            Distance = activity.Distance,
            AverageSpeed = activity.AverageSpeed,
            AverageCadence = activity.AverageCadence,
            AveragePace = activity.AveragePace,
            AverageHeartRate = activity.AverageHeartRate,
            ElevationGain = activity.ElevationGain,
            Calories = activity.Calories,
            CreatedAt = activity.CreatedAt
        };

        return activityDto;
    }

    
}