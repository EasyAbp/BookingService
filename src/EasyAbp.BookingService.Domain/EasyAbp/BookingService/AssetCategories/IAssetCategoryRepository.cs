using System;
using EasyAbp.Abp.Trees;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetCategories;

public interface IAssetCategoryRepository : ITreeRepository<AssetCategory>
{
}