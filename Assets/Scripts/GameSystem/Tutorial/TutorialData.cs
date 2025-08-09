using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public string tutorialTitle;
    public Sprite tutorialImage;
    [TextArea(3, 10)]
    public string tutorialDescription;
}

[CreateAssetMenu(fileName = "NewTutorialData", menuName = "Tutorial/Tutorial Data")]
public class TutorialData : ScriptableObject
{
    [Header("Pasos del Tutorial")]
    public TutorialStep[] steps;
}
