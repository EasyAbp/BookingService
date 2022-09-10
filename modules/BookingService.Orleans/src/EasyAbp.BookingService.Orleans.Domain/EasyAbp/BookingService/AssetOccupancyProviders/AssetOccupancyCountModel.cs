using System;

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

    public bool TryChangeVolume(int changedVolume)
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