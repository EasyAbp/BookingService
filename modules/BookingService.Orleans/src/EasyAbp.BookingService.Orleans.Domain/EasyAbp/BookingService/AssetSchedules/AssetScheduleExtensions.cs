using System;
using System.Globalization;

namespace EasyAbp.BookingService.AssetSchedules;

public static class AssetScheduleExtensions
{
    public const string CompoundKeySeparator = "|";

    public static string CalculateCompoundKey(DateTime date, Guid? id)
    {
        return $"{date.ToString(CultureInfo.InvariantCulture)}{CompoundKeySeparator}{id:N}";
    }

    public static DateTime GetDateFromCompoundKey(this string compoundKey)
    {
        return DateTime.Parse(compoundKey.Split(CompoundKeySeparator)[0]);
    }

    public static AssetScheduleGrainStateModel ToAssetScheduleGrainStateModel(this AssetSchedule entity)
    {
        return new AssetScheduleGrainStateModel(
            entity.Id,
            entity.PeriodSchemeId,
            entity.PeriodId,
            entity.PeriodUsable,
            entity.TimeInAdvance);
    }
}