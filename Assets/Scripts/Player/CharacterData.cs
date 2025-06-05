using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;
    public RuntimeAnimatorController animatorController;
    public Vector2 spawnPosition;
}