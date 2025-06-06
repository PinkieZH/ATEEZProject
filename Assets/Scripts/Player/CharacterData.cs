using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    public RuntimeAnimatorController animatorController;
    public Vector2 spawnPosition;
    public string description;
    public int itemsSpeciauxRequis;
    [TextArea(3, 5)]
    public string finText;
}