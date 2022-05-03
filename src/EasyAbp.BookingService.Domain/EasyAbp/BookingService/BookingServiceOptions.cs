using System.Collections.Generic;
using EasyAbp.BookingService.AssetDefinitions;

namespace EasyAbp.BookingService;

public class BookingServiceOptions
{
    public const string ConfigurationKey = "BookingService";
    
    public List<AssetDefinition> AssetDefinitions { get; set; } = new();
}