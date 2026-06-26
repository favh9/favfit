using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FavFitApi.Models;
using FavFitApi.Data;

namespace FavFitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly FavFitdbContext _context;
    private readonly ActivityMapper _activityMapper;

    public ActivitiesController(FavFitdbContext context, ActivityMapper activityMapper)
    {
        _context = context;
        _activityMapper = activityMapper;
    }

    [HttpPost]
    [EndpointSummary("Creates an activity")]
    [EndpointDescription("Activity Type Options: Bike = 1, Run = 2, Hike = 3, Walk = 4, Swim = 5, WeightLifting = 6")]
    public async Task<ActionResult<Activity>> CreateActivity(CreateActivityDto createActivityDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FindAsync(createActivityDto.UserId);

        if (user == null)
            return NotFound($"User ID {createActivityDto.UserId} does not exist");

        var newActivity = _activityMapper.ToActivity(createActivityDto);
        var activityDto = _activityMapper.ToActivityDto(newActivity);

        var res = CreatedAtAction(nameof(GetActivityById), new {id = newActivity.Id}, activityDto); 
        
        _context.Activities.Add(newActivity);

        await _context.SaveChangesAsync();

        return res;
    }

    [HttpGet]
    [Route("{id}")]
    [EndpointSummary("Returns an activity")]
    public async Task<ActionResult<ActivityDto>> GetActivityById(long id)
    {
        var activity = await _context.Activities.FindAsync(id);

        if (activity == null)
            return NotFound($"Activity ID {id} not found");
        
        var activityDto = _activityMapper.ToActivityDto(activity);
        
        return activityDto;
    }

    [HttpPut]
    [EndpointSummary("Updates an activity")]
    public async Task<ActionResult<UpdateActivityDto>> UpdateActivity(UpdateActivityDto updateActivityDto)
    {   
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var activity = await _context.Activities.FindAsync(updateActivityDto.Id);

        if (activity == null)
            return NotFound($"Activity ID {updateActivityDto.Id} not found");
        
        if (updateActivityDto.NewTitle != null)
            activity.Title = updateActivityDto.NewTitle;

        activity.Type = updateActivityDto.NewType;
        activity.Date = updateActivityDto.NewDate;
        activity.ElapsedTime = updateActivityDto.NewElapsedTime;
        activity.Distance = updateActivityDto.NewDistance;
        activity.AverageSpeed = updateActivityDto.NewAverageSpeed;
        activity.AverageCadence = updateActivityDto.NewAverageCadence;
        activity.AveragePace = updateActivityDto.NewAveragePace;
        activity.AverageHeartRate = updateActivityDto.NewAverageHeartRate;
        activity.ElevationGain = updateActivityDto.NewElevationGain;
        activity.Calories = updateActivityDto.NewCalories;
        
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    [EndpointSummary("Deletes an activity")]
    public async Task<ActionResult> DeleteActivity(long id)
    {
        var activity = await _context.Activities.FindAsync(id);

        if (activity == null)
            return NotFound($"Activity ID {id} not found");
        
        _context.Activities.Remove(activity);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
}