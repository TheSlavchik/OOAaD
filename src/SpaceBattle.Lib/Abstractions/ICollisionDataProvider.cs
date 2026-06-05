namespace SpaceBattle.Lib.Abstractions;

public interface ICollisionDataProvider
{
    bool CheckCollision(string objectType1, string objectType2);
    void SaveToFile(string filePath);
    void LoadFromFile(string filePath);
}
