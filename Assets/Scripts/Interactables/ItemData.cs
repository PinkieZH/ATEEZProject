using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Interactable Item\"")]
public class ItemData : ScriptableObject
{
    [Header("Item Settings")]
    public string itemName;

    [Header("Infos")]
    [TextArea(3, 10)]
    public string[] itemInfo;
    public bool estSpecial;
}
