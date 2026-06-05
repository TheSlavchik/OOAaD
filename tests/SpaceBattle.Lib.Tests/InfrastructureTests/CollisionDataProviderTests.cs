using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.InfrastructureTests;

public class CollisionDataProviderTests
{
    [Fact]
    public void CheckCollision_RegisteredPair_ReturnsTrue()
    {
        var provider = new CollisionDataProvider();
        provider.AddCollisionPair("Ship", "Torpedo", true);

        var result = provider.CheckCollision("Ship", "Torpedo");

        Assert.True(result);
    }

    [Fact]
    public void CheckCollision_RegisteredInReverseOrder_ReturnsTrue()
    {
        var provider = new CollisionDataProvider();
        provider.AddCollisionPair("Ship", "Torpedo", true);

        var result = provider.CheckCollision("Torpedo", "Ship");

        Assert.True(result);
    }

    [Fact]
    public void CheckCollision_UnregisteredPair_ReturnsFalse()
    {
        var provider = new CollisionDataProvider();

        var result = provider.CheckCollision("Ship", "Asteroid");

        Assert.False(result);
    }

    [Fact]
    public void CheckCollision_NonCollidingPair_ReturnsFalse()
    {
        var provider = new CollisionDataProvider();
        provider.AddCollisionPair("Ship", "Ship", false);

        var result = provider.CheckCollision("Ship", "Ship");

        Assert.False(result);
    }

    [Fact]
    public void SaveAndLoadToFile_RoundTrip_PreservesData()
    {
        var filePath = Path.GetTempFileName();
        try
        {
            var provider = new CollisionDataProvider();
            provider.AddCollisionPair("Ship", "Torpedo", true);
            provider.AddCollisionPair("Ship", "Asteroid", false);

            provider.SaveToFile(filePath);

            var loadedProvider = new CollisionDataProvider();
            loadedProvider.LoadFromFile(filePath);

            Assert.True(loadedProvider.CheckCollision("Ship", "Torpedo"));
            Assert.True(loadedProvider.CheckCollision("Torpedo", "Ship"));
            Assert.False(loadedProvider.CheckCollision("Ship", "Asteroid"));
            Assert.False(loadedProvider.CheckCollision("Asteroid", "Ship"));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void LoadFromFile_FileNotFound_ThrowsFileNotFoundException()
    {
        var provider = new CollisionDataProvider();

        Assert.Throws<FileNotFoundException>(() => provider.LoadFromFile("nonexistent_file.json"));
    }

    [Fact]
    public void AddCollisionPair_SamePairTwice_OverridesValue()
    {
        var provider = new CollisionDataProvider();
        provider.AddCollisionPair("Ship", "Torpedo", true);
        provider.AddCollisionPair("Ship", "Torpedo", false);

        var result = provider.CheckCollision("Ship", "Torpedo");

        Assert.False(result);
    }

    [Fact]
    public void SaveToFile_CreatesValidJson()
    {
        var filePath = Path.GetTempFileName();
        try
        {
            var provider = new CollisionDataProvider();
            provider.AddCollisionPair("A", "B", true);

            provider.SaveToFile(filePath);

            var json = File.ReadAllText(filePath);
            Assert.Contains("A", json);
            Assert.Contains("B", json);
            Assert.Contains("true", json.ToLower());
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void CheckCollision_OnlyOneDirectionRegistered_ReturnsTrue()
    {
        var provider = new CollisionDataProvider();
        provider.AddCollisionPair("A", "B", true);

        var result = provider.CheckCollision("A", "B");

        Assert.True(result);
    }

    [Fact]
    public void CheckCollision_SecondTryGetValueCatches()
    {
        var provider = new CollisionDataProvider();
        provider.AddCollisionPair("A", "B", true);

        var result = provider.CheckCollision("B", "A");

        Assert.True(result);
    }

    [Fact]
    public void LoadFromFile_InvalidJson_ThrowsJsonException()
    {
        var filePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(filePath, "not valid json");

            var provider = new CollisionDataProvider();

            Assert.Throws<System.Text.Json.JsonException>(() => provider.LoadFromFile(filePath));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void LoadFromFile_NullJsonData_ThrowsInvalidOperationException()
    {
        var filePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(filePath, "null");

            var provider = new CollisionDataProvider();

            Assert.Throws<InvalidOperationException>(() => provider.LoadFromFile(filePath));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void CheckCollision_O1_Complexity_Test()
    {
        var provider = new CollisionDataProvider();
        provider.AddCollisionPair("ShipType1", "ShipType2", true);

        var startTime = System.Diagnostics.Stopwatch.GetTimestamp();
        for (int i = 0; i < 1000; i++)
        {
            provider.CheckCollision("ShipType1", "ShipType2");
        }
        var elapsed = System.Diagnostics.Stopwatch.GetElapsedTime(startTime);

        Assert.True(elapsed.TotalMilliseconds < 100, $"Checking collision 1000 times took too long: {elapsed.TotalMilliseconds}ms");
    }
}
