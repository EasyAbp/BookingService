using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public interface IHasOccupyingTimeInfo : IHasPeriodInfo
{
    DateTime Date { get; }
}