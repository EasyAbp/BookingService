# Concurrency control of DefaultAssetOccupancyProvider

1. cate/asset vs cate/asset: using `AssetOccupancyCount`'s `ConcurrencyToken` to ensure `at least one succeed`.
1. bulk vs cate/asset: using `AssetOccupancyCount`'s `ConcurrencyToken` to ensure `at least one succeed`.
1. bulk vs bulk -> using [WaitDie](http://www.mathcs.emory.edu/~cheung/Courses/554/Syllabus/8-recv+serial/deadlock-waitdie.html) method to avoid `all failed` scenario.