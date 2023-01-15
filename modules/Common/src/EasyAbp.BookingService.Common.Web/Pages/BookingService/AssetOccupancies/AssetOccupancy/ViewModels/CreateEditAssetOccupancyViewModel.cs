using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetOccupancies.AssetOccupancy.ViewModels;

public class CreateAssetOccupancyViewModel : ExtensibleObject
{
    [Display(Name = "AssetOccupancyAssetId")]
    public Guid AssetId { get; set; }

    [Display(Name = "AssetOccupancyVolume")]
    public int Volume { get; set; }

    [Display(Name = "AssetOccupancyDate")]
    public DateTime Date { get; set; }

    [Display(Name = "AssetOccupancyStartingTime")]
    public TimeSpan StartingTime { get; set; }

    [Display(Name = "AssetOccupancyDuration")]
    public TimeSpan Duration { get; set; }

    [Display(Name = "AssetOccupancyOccupierUserId")]
    public Guid? OccupierUserId { get; set; }
}