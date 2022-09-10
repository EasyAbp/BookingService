using System;
using System.Globalization;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public static class AssetOccupancyGrainExtensions
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
}