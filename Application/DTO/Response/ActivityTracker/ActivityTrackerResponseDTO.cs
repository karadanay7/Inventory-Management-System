using System;
using System.ComponentModel.DataAnnotations;
using Application.DTO.Response.ActivityTracker;

namespace Application.DTO.Response;

public class ActivityTrackerResponseDTO: BaseActivityTracker
{
  [Required]
  public string UserName { get; set; }
}
