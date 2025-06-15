using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class BubbleText : Items
{
    [Header("Références")]
    public ObjectsManager objectsManager;
    

    public override void Interact()
    {
        Read();
    }
}
