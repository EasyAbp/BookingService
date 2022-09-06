using System;
using EasyAbp.BookingService.AssetOccupancies;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

[Serializable]
public class AssetOccupancyCountModel
{
    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public int Volume { get; set; }

    protected AssetOccupancyCountModel()
    {
    }

    public AssetOccupancyCountModel(TimeSpan startingTime, TimeSpan duration, int volume)
    {
        StartingTime = startingTime;
        Duration = duration;
        Volume = volume;
    }

    public bool ChangeVolume(int changedVolume)
    {
        if (Volume + changedVolume < 0)
        {
            return false;
        }

        checked
        {
            Volume += changedVolume;
        }

        return true;
    }
}