using System.Text.Json;
using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Infrastructure;

public class CollisionDataProvider : ICollisionDataProvider
{
    private readonly Dictionary<(string, string), bool> _collisionMatrix = new();

    private static (string, string) GetOrderedKey(string type1, string type2)
    {
        return string.Compare(type1, type2, StringComparison.Ordinal) <= 0
            ? (type1, type2)
            : (type2, type1);
    }

    public void AddCollisionPair(string objectType1, string objectType2, bool collides)
    {
        var key = GetOrderedKey(objectType1, objectType2);
        _collisionMatrix[key] = collides;
    }

    public bool CheckCollision(string objectType1, string objectType2)
    {
        var key1 = (objectType1, objectType2);
        var key2 = (objectType2, objectType1);

        if (_collisionMatrix.TryGetValue(key1, out var result))
        {
            return result;
        }

        if (_collisionMatrix.TryGetValue(key2, out result))
        {
            return result;
        }

        return false;
    }

    public void SaveToFile(string filePath)
    {
        var serializableData = new List<CollisionDataEntry>();

        foreach (var kvp in _collisionMatrix)
        {
            serializableData.Add(new CollisionDataEntry
            {
                ObjectType1 = kvp.Key.Item1,
                ObjectType2 = kvp.Key.Item2,
                Collides = kvp.Value
            });
        }

        var json = JsonSerializer.Serialize(serializableData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Collision data file not found: {filePath}");
        }

        var json = File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<List<CollisionDataEntry>>(json);

        if (data == null)
        {
            throw new InvalidOperationException("Failed to deserialize collision data.");
        }

        _collisionMatrix.Clear();

        foreach (var entry in data)
        {
            AddCollisionPair(entry.ObjectType1, entry.ObjectType2, entry.Collides);
        }
    }

    private class CollisionDataEntry
    {
        public string ObjectType1 { get; set; } = string.Empty;
        public string ObjectType2 { get; set; } = string.Empty;
        public bool Collides { get; set; }
    }
}
