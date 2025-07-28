using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public LevelType levelType = LevelType.Day;
    public string[] levelScenes;
    public int requiredClues = 3;

    public ObjectiveType objectiveType = ObjectiveType.Door;
}

public enum ObjectiveType
{
    Door,
    Puzzle
}
