using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class BubbleText : Items
{
    [Header("R�f�rences")]
    public ObjectsManager objectsManager;
    

    public override void Interact()
    {
        Read();
    }
}
