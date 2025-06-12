using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class BubbleText : Items
{
    [Header("Références Bulles")]
    public Animator anim;
    public GameObject textBubble;
    public ObjectsManager objectsManager;
    private bool isTextClosed;
    private Transform parentPosition;
    private GameObject objectsManagerInstantiated;

    private void Awake()
    {
        parentPosition = GetComponentInParent<Transform>();
        GameObject newBubble = Instantiate(textBubble,
            new Vector3(parentPosition.position.x, parentPosition.position.y, 0),
            Quaternion.identity);

        newBubble.transform.SetParent(parentPosition);
        anim = newBubble.GetComponent<Animator>();
        objectsManagerInstantiated = newBubble;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        objectsManagerInstantiated.transform.localPosition = new Vector3(0.5f, 3.4f, 0);
        objectsManagerInstantiated.SetActive(true);
        anim.SetBool("isShowing", true);
        isTextClosed = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isTextClosed = false;
        anim.SetBool("isShowing", false);
    }

    public override void Interact()
    {
        base.Interact();
        anim.SetBool("isShowing", false);
        Read();
    }

    private void Update()
    {
        bool dialogueState = objectsManager.IsTextActive();
        if (isTextClosed)
        {
            anim.SetBool("isShowing", !dialogueState);
        }
    }
}
