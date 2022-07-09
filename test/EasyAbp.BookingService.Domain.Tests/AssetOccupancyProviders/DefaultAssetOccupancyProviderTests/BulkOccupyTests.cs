using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancyCounts;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.PeriodSchemes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Xunit;
using Xunit.Sdk;

namespace EasyAbp.BookingService.AssetOccupancyProviders.DefaultAssetOccupancyProviderTests;

public class BulkOccupyTests : DefaultAssetOccupancyProviderTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        services.Replace(ServiceDescriptor.Transient(s => Substitute.ForPartsOf<DefaultAssetOccupancyProvider>(
            s.GetRequiredService<IAssetOccupancyCountRepository>(),
            s.GetRequiredService<IUnitOfWorkManager>(),
            s
        )));
    }

    #region CanBulkOccupy

    private static object[] CanBulkOccupy_OccupyingAsset_Baseline() =>
        new object[]
        {
            new BulkOccupyTestModel(true,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(0, 1, 0, 1, 0)
                            })
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingCategory_Baseline() =>
        new object[]
        {
            new BulkOccupyTestModel(true,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1)
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingAssetAndCategory_Baseline() =>
        new object[]
        {
            new BulkOccupyTestModel(true,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 2, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(0, 1, 0, 1, 0)
                            })
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingAsset_Category_Disabled() =>
        new object[]
        {
            new BulkOccupyTestModel(false,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(true,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(0, 1, 0, 1, 0)
                            })
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingAsset_Asset_Disabled() =>
        new object[]
        {
            new BulkOccupyTestModel(false,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(true, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(0, 1, 0, 1, 0)
                            })
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingCategory_Category_Disabled() =>
        new object[]
        {
            new BulkOccupyTestModel(false,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(true,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1)
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingCategory_Asset_Disabled() =>
        new object[]
        {
            new BulkOccupyTestModel(false,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(true, 1)
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingAsset_MultiAssets(bool canOccupy) =>
        new object[]
        {
            new BulkOccupyTestModel(canOccupy,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 1, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0)
                            })
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingAsset_MultiDates(bool canOccupy) =>
        new object[]
        {
            new BulkOccupyTestModel(canOccupy,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 1, 0, 1, 0),
                                new(1, 0, 0, 1, 1)
                            }),
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingAssets_MultiAssets(bool canOccupy) =>
        new object[]
        {
            new BulkOccupyTestModel(canOccupy,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 1, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 1, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0)
                            }),
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingAssets_MultiAssetsAndMultiDates(bool canOccupy) =>
        new object[]
        {
            new BulkOccupyTestModel(canOccupy,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 1, 0, 1, 0),
                                new(canOccupy ? 0 : 1, 1, 0, 1, 1),
                                new(canOccupy ? 0 : 1, 0, 0, 1, 2)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 1, 0, 1, 0),
                                new(canOccupy ? 0 : 1, 1, 0, 1, 1),
                                new(canOccupy ? 0 : 1, 0, 0, 1, 2)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0),
                                new(0, 0, 0, 1, 1),
                                new(0, 0, 0, 1, 2)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0),
                                new(0, 0, 0, 1, 1),
                                new(0, 0, 0, 1, 2)
                            }),
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingCategory_MultiAssets(bool canOccupy) =>
        new object[]
        {
            new BulkOccupyTestModel(canOccupy,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 0, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0)
                            })
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingCategory_MultiDates(bool canOccupy) =>
        new object[]
        {
            new BulkOccupyTestModel(canOccupy,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 0, 0, 1, 0),
                                new(1, 0, 0, 1, 1)
                            }),
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        }),
                })
        };

    private static object[] CanBulkOccupy_OccupyingCategories_MultiAssets(bool canOccupy) =>
        new object[]
        {
            new BulkOccupyTestModel(canOccupy,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 0, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0)
                            }),
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        }),
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 0, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0)
                            }),
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        })
                })
        };

    private static object[] CanBulkOccupy_OccupyingCategories_MultiAssetsAndMultiDates(bool canOccupy) =>
        new object[]
        {
            new BulkOccupyTestModel(canOccupy,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(1, 0, 0, 1, 0),
                                new(canOccupy ? 0 : 1, 0, 0, 1, 1),
                                new(canOccupy ? 0 : 1, 0, 0, 1, 2)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(canOccupy ? 0 : 1, 0, 0, 1, 0),
                                new(1, 0, 0, 1, 1),
                                new(1, 0, 0, 1, 2)
                            }),
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0),
                            new(1, 0, 1, 1),
                            new(1, 0, 1, 2),
                        })
                })
        };

    public static IEnumerable<object[]> CanBulkOccupyTestData => new List<object[]>
    {
        CanBulkOccupy_OccupyingAsset_Baseline(),
        CanBulkOccupy_OccupyingAsset_MultiAssets(true),
        CanBulkOccupy_OccupyingAsset_MultiAssets(false),
        CanBulkOccupy_OccupyingAsset_MultiDates(true),
        CanBulkOccupy_OccupyingAsset_MultiDates(false),
        CanBulkOccupy_OccupyingAssets_MultiAssets(true),
        CanBulkOccupy_OccupyingAssets_MultiAssets(false),
        CanBulkOccupy_OccupyingAssets_MultiAssetsAndMultiDates(true),
        CanBulkOccupy_OccupyingAssets_MultiAssetsAndMultiDates(false),

        CanBulkOccupy_OccupyingCategory_Baseline(),
        CanBulkOccupy_OccupyingCategory_MultiAssets(true),
        CanBulkOccupy_OccupyingCategory_MultiAssets(false),
        CanBulkOccupy_OccupyingCategory_MultiDates(true),
        CanBulkOccupy_OccupyingCategory_MultiDates(false),
        CanBulkOccupy_OccupyingCategories_MultiAssets(true),
        CanBulkOccupy_OccupyingCategories_MultiAssets(false),
        CanBulkOccupy_OccupyingCategories_MultiAssetsAndMultiDates(true),
        CanBulkOccupy_OccupyingCategories_MultiAssetsAndMultiDates(false),

        CanBulkOccupy_OccupyingAssetAndCategory_Baseline(),

        CanBulkOccupy_OccupyingCategory_Asset_Disabled(),
    };

    public static IEnumerable<object[]> CanBulkOccupyDisabledTestData => new List<object[]>
    {
        CanBulkOccupy_OccupyingAsset_Category_Disabled(),
        CanBulkOccupy_OccupyingAsset_Asset_Disabled(),
        CanBulkOccupy_OccupyingCategory_Category_Disabled(),
    };

    [Theory]
    [MemberData(nameof(CanBulkOccupyTestData))]
    public async Task CanBulkOccupy_Test(object obj)
    {
        // Arrange
        if (obj is not BulkOccupyTestModel testModel)
        {
            throw new XunitException();
        }

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        var (assets, categories) = await CreateEntitiesAsync(testModel.Categories, targetDate);

        // Act & Assert
        var result = await AssetOccupancyProvider.CanBulkOccupyAsync(assets, categories);
        result.CanOccupy.ShouldBe(testModel.CanOccupy);
        if (!testModel.CanOccupy)
        {
            result.ErrorCode.ShouldBe(BookingServiceErrorCodes.InsufficientAssetVolume);
        }
    }

    [Theory]
    [MemberData(nameof(CanBulkOccupyDisabledTestData))]
    public async Task CanBulkOccupy_ShouldThrow_DisabledAssetOrCategoryException_Test(object obj)
    {
        // Arrange
        if (obj is not BulkOccupyTestModel testModel)
        {
            throw new XunitException();
        }

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        var (assets, categories) = await CreateEntitiesAsync(testModel.Categories, targetDate);

        // Act & Assert
        var result = await AssetOccupancyProvider.CanBulkOccupyAsync(assets, categories);
        result.CanOccupy.ShouldBeFalse();
        result.ErrorCode.ShouldBe(BookingServiceErrorCodes.DisabledAssetOrCategory);
    }

    #endregion

    #region BulkOccupy

    private static object[] BulkOccupy_Rollback() =>
        new object[]
        {
            new BulkOccupyTestModel(true,
                new List<BulkOccupyCategoryTestModel>
                {
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(0, 1, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(0, 1, 0, 1, 0)
                            }),
                        }),
                    new(false,
                        new List<BulkOccupyAssetTestModel>
                        {
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(0, 0, 0, 1, 0)
                            }),
                            new(false, 1, new List<BulkOccupyAssetOccupancyTestModel>
                            {
                                new(0, 0, 0, 1, 0)
                            }),
                        },
                        new List<BulkOccupyCategoryOccupancyTestModel>
                        {
                            new(1, 0, 1, 0)
                        })
                })
        };

    public static IEnumerable<object[]> BulkOccupyShouldThrowInsufficientAssetVolumeExceptionData => new List<object[]>
    {
        CanBulkOccupy_OccupyingAsset_MultiAssets(false),
        CanBulkOccupy_OccupyingAsset_MultiDates(false),
        CanBulkOccupy_OccupyingAssets_MultiAssets(false),
        CanBulkOccupy_OccupyingAssets_MultiAssetsAndMultiDates(false),
        CanBulkOccupy_OccupyingCategory_MultiAssets(false),
        CanBulkOccupy_OccupyingCategory_MultiDates(false),
        CanBulkOccupy_OccupyingCategories_MultiAssets(false),
        CanBulkOccupy_OccupyingCategories_MultiAssetsAndMultiDates(false),

        CanBulkOccupy_OccupyingCategory_Asset_Disabled(),
    };

    public static IEnumerable<object[]> BulkOccupyShouldThrowDisabledAssetOrCategoryExceptionData => new List<object[]>
    {
        CanBulkOccupy_OccupyingCategory_Category_Disabled(),
        CanBulkOccupy_OccupyingAsset_Category_Disabled(),
        CanBulkOccupy_OccupyingAsset_Asset_Disabled(),
    };

    public static IEnumerable<object[]> BulkOccupyData => new List<object[]>
    {
        CanBulkOccupy_OccupyingAssetAndCategory_Baseline(),
    };

    public static IEnumerable<object[]> BulkOccupyRollbackData => new List<object[]>
    {
        BulkOccupy_Rollback(),
    };

    [Theory]
    [MemberData(nameof(BulkOccupyShouldThrowInsufficientAssetVolumeExceptionData))]
    public async Task BulkOccupy_ShouldThrow_InsufficientAssetVolumeException_Test(object obj)
    {
        // Arrange
        if (obj is not BulkOccupyTestModel testModel)
        {
            throw new XunitException();
        }

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        var (assets, categories) = await CreateEntitiesAsync(testModel.Categories, targetDate);

        // Act & Assert
        await Should.ThrowAsync<InsufficientAssetVolumeException>(() =>
            AssetOccupancyProvider.BulkOccupyAsync(assets, categories, default));
    }

    [Theory]
    [MemberData(nameof(BulkOccupyShouldThrowDisabledAssetOrCategoryExceptionData))]
    public async Task BulkOccupy_ShouldThrow_DisabledAssetOrCategoryException_Test(object obj)
    {
        // Arrange
        if (obj is not BulkOccupyTestModel testModel)
        {
            throw new XunitException();
        }

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        var (assets, categories) = await CreateEntitiesAsync(testModel.Categories, targetDate);

        // Act & Assert
        await Should.ThrowAsync<DisabledAssetOrCategoryException>(() =>
            AssetOccupancyProvider.BulkOccupyAsync(assets, categories, default));
    }

    [Theory]
    [MemberData(nameof(BulkOccupyData))]
    public async Task BulkOccupy_OccupierUserId_Test(object obj)
    {
        // Arrange
        if (obj is not BulkOccupyTestModel testModel)
        {
            throw new XunitException();
        }

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);
        var userId = GetRequiredService<IGuidGenerator>().Create();
        const string userName = nameof(userName);
        var userData = Substitute.For<IUserData>();
        userData.UserName.Returns(userName);
        ExternalUserLookupServiceProvider.FindByIdAsync(userId).Returns(Task.FromResult(userData));

        var (assets, categories) = await CreateEntitiesAsync(testModel.Categories, targetDate);

        // Act
        var result = await AssetOccupancyProvider.BulkOccupyAsync(assets, categories, userId);

        // Assert
        var assetOccupancies = result.Select(x => x.Item2).ToList();
        assetOccupancies.ShouldNotBeEmpty();
        assetOccupancies.All(x => x.OccupierUserId == userId && x.OccupierName == userName).ShouldBeTrue();
    }

    [Theory]
    [MemberData(nameof(BulkOccupyRollbackData))]
    public async Task BulkOccupy_Rollback_Test(object obj)
    {
        // Arrange
        if (obj is not BulkOccupyTestModel testModel)
        {
            throw new XunitException();
        }

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        var (assets, categories) = await CreateEntitiesAsync(testModel.Categories, targetDate);

        AssetOccupancyProvider.OccupyByCategoryAsync(Arg.Any<OccupyAssetByCategoryInfoModel>(), Arg.Any<Guid?>())
            .ThrowsForAnyArgs(new DisabledAssetOrCategoryException());

        // Act
        await Should.ThrowAsync<DisabledAssetOrCategoryException>(() =>
            AssetOccupancyProvider.BulkOccupyAsync(assets, categories, default));
        var assetOccupancyCounts = await AssetOccupancyCountRepository.GetListAsync();

        // Assert
        assetOccupancyCounts.All(x => x.Volume == 0).ShouldBeTrue();
    }

    #endregion

    #region private

    private async Task<(List<OccupyAssetInfoModel>, List<OccupyAssetByCategoryInfoModel>)> CreateEntitiesAsync(
        List<BulkOccupyCategoryTestModel> bulkOccupyCategories,
        DateTime targetDate)
    {
        var assets = new List<Asset>();
        var categories = new List<AssetCategory>();
        var counts = new List<AssetOccupancyCount>();
        var occupyingAssets = new List<OccupyAssetInfoModel>();
        var occupyingCategories = new List<OccupyAssetByCategoryInfoModel>();
        var index = 0;

        var distinctPeriods = bulkOccupyCategories
            .SelectMany(x => x.Assets)
            .SelectMany(x => x.OccupancyTestModels)
            .Select(x => new { x.PeriodStartingHour, x.PeriodDurationHour })
            .Concat(bulkOccupyCategories
                .SelectMany(x => x.OccupancyTestModels)
                .Select(x => new { x.PeriodStartingHour, x.PeriodDurationHour }))
            .GroupBy(x => x)
            .Select(x => new { x.Key.PeriodStartingHour, x.Key.PeriodDurationHour })
            .ToList();

        var periods = new List<Period>();
        foreach (var period in distinctPeriods)
        {
            periods.Add(await PeriodSchemeManager.CreatePeriodAsync(TimeSpan.FromHours(period.PeriodStartingHour),
                TimeSpan.FromHours(period.PeriodDurationHour)));
        }

        var periodScheme = await PeriodSchemeManager.CreateAsync(nameof(PeriodScheme), periods);

        await PeriodSchemeManager.SetAsDefaultAsync(periodScheme);

        foreach (var categoryTestModel in bulkOccupyCategories)
        {
            var category = await AssetCategoryManager.CreateAsync(default, nameof(AssetCategory) + index++,
                AssetDefinition.Name,
                default,
                default,
                new TimeInAdvance
                {
                    MaxDaysInAdvance = 365
                },
                categoryTestModel.Disabled);

            categories.Add(category);

            foreach (var model in categoryTestModel.OccupancyTestModels)
            {
                if (model.OccupyingVolume > 0)
                {
                    occupyingCategories.Add(new OccupyAssetByCategoryInfoModel(category.Id,
                        default,
                        model.OccupyingVolume,
                        targetDate.AddDays(model.DayOffset),
                        TimeSpan.FromHours(model.PeriodStartingHour),
                        TimeSpan.FromHours(model.PeriodDurationHour)));
                }
            }

            foreach (var assetTestModel in categoryTestModel.Assets)
            {
                var asset = await AssetManager.CreateAsync(nameof(Asset) + index++,
                    AssetDefinition.Name,
                    category,
                    default,
                    default,
                    assetTestModel.InitialVolume,
                    default,
                    default,
                    assetTestModel.Disabled);

                assets.Add(asset);

                foreach (var model in assetTestModel.OccupancyTestModels)
                {
                    if (model.OccupyingVolume > 0)
                    {
                        occupyingAssets.Add(new OccupyAssetInfoModel(asset.Id, model.OccupyingVolume,
                            targetDate.AddDays(model.DayOffset),
                            TimeSpan.FromHours(model.PeriodStartingHour),
                            TimeSpan.FromHours(model.PeriodDurationHour)));
                    }

                    if (model.OccupiedVolume > 0)
                    {
                        counts.Add(new AssetOccupancyCount(default, asset.Id, asset.Name,
                            targetDate.AddDays(model.DayOffset),
                            TimeSpan.FromHours(model.PeriodStartingHour),
                            TimeSpan.FromHours(model.PeriodDurationHour),
                            model.OccupiedVolume));
                    }
                }
            }
        }

        await WithUnitOfWorkAsync(async () =>
        {
            await PeriodSchemeRepository.InsertAsync(periodScheme);
            await AssetCategoryRepository.InsertManyAsync(categories);
            await AssetRepository.InsertManyAsync(assets);
            await AssetOccupancyCountRepository.InsertManyAsync(counts);
        });

        return (occupyingAssets, occupyingCategories);
    }

    private class BulkOccupyTestModel
    {
        public BulkOccupyTestModel(bool canOccupy, List<BulkOccupyCategoryTestModel> categories,
            [CallerMemberName] string name = "")
        {
            CanOccupy = canOccupy;
            Categories = categories;
            Name = name;
        }

        public List<BulkOccupyCategoryTestModel> Categories { get; }

        public bool CanOccupy { get; }

        public string Name { get; }

        public override string ToString()
        {
            return $"{Name}-{CanOccupy}";
        }
    }

    private class BulkOccupyCategoryTestModel
    {
        public BulkOccupyCategoryTestModel(bool disabled, List<BulkOccupyAssetTestModel> assets,
            List<BulkOccupyCategoryOccupancyTestModel> occupancyTestModels = default)
        {
            Disabled = disabled;
            Assets = assets;
            OccupancyTestModels = occupancyTestModels ?? new List<BulkOccupyCategoryOccupancyTestModel>();
        }

        public List<BulkOccupyAssetTestModel> Assets { get; }

        public List<BulkOccupyCategoryOccupancyTestModel> OccupancyTestModels { get; }

        public bool Disabled { get; }
    }

    private class BulkOccupyAssetTestModel
    {
        public BulkOccupyAssetTestModel(bool disabled, int initialVolume,
            List<BulkOccupyAssetOccupancyTestModel> occupancyTestModels = default)
        {
            Disabled = disabled;
            InitialVolume = initialVolume;
            OccupancyTestModels = occupancyTestModels ?? new List<BulkOccupyAssetOccupancyTestModel>();
        }

        public bool Disabled { get; }

        public int InitialVolume { get; }

        public List<BulkOccupyAssetOccupancyTestModel> OccupancyTestModels { get; }
    }

    private class BulkOccupyAssetOccupancyTestModel
    {
        public BulkOccupyAssetOccupancyTestModel(int occupiedVolume, int occupyingVolume, int periodStartingHour,
            int periodDurationHour, int dayOffset)
        {
            OccupiedVolume = occupiedVolume;
            PeriodStartingHour = periodStartingHour;
            PeriodDurationHour = periodDurationHour;
            DayOffset = dayOffset;
            OccupyingVolume = occupyingVolume;
        }

        public int DayOffset { get; }

        public int OccupiedVolume { get; }

        public int OccupyingVolume { get; }

        public int PeriodStartingHour { get; }

        public int PeriodDurationHour { get; }
    }

    private class BulkOccupyCategoryOccupancyTestModel
    {
        public BulkOccupyCategoryOccupancyTestModel(int occupyingVolume, int periodStartingHour,
            int periodDurationHour, int dayOffset)
        {
            PeriodStartingHour = periodStartingHour;
            PeriodDurationHour = periodDurationHour;
            DayOffset = dayOffset;
            OccupyingVolume = occupyingVolume;
        }

        public int DayOffset { get; }

        public int OccupyingVolume { get; }

        public int PeriodStartingHour { get; }

        public int PeriodDurationHour { get; }
    }

    #endregion
}