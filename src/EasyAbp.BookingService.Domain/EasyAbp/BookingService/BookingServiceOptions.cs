using System;
using System.Collections.Generic;
using EasyAbp.BookingService.AssetDefinitions;

namespace EasyAbp.BookingService;

[Serializable]
public class BookingServiceOptions
{
    public const string ConfigurationKey = "BookingService";

    public List<AssetDefinition> AssetDefinitionConfigurations { get; set; } = new();
}