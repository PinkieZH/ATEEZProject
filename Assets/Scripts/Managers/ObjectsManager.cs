using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEditor.Rendering;

public class ObjectsManager : MonoBehaviour
{
    [Header("Infos References")]
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI itemName;
    public GameObject continueTextIndicator;

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;

    [Header("Input Settings")]
    public InputActionReference continueTextAction;

    public string[] currentLines;
    public int currentLineIndex;
    private bool isTyping;
    private Coroutine typeCoroutine;
    private bool isTextActive;

    public static ObjectsManager Instance { get; private set; }

    private void Awake()
    {
        //Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        infoPanel.SetActive(false);
        if (continueTextIndicator != null)
            continueTextIndicator.SetActive(false);

        if (continueTextAction != null)
        {
            continueTextAction.action.started += OnContinueTextInput;
        }
    }

    private void OnEnable()
    {
        if (continueTextAction != null)
        {
            continueTextAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (continueTextAction != null)
        {
            continueTextAction.action.Disable();
        }
    }

    private void OnDestroy()
    {
        if (continueTextAction != null)
        {
            continueTextAction.action.started -= OnContinueTextInput;
        }
    }

    private void OnContinueTextInput(InputAction.CallbackContext context)
    {
        if (!isTextActive)
        {
            return;
        }

        if (isTyping)
        {
            CompleteTyping();
        }
        else
        {
            DisplayNextLine();
        }
    }

    public void StartText(string objectName, string[] lines)
    {
        currentLines = lines;
        currentLineIndex = 0;
        isTextActive = true;

        infoPanel.SetActive(true);
        itemName.text = objectName;

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }

        //si on est à la fin du dialogue: terminer
        if (currentLineIndex >= currentLines.Length)
        {
            EndText();

            return;
        }

        //desac l'indicateur de continuation
        if (continueTextIndicator)
        {
            continueTextIndicator.SetActive(false);
        }

        //commencer à taper la line
        typeCoroutine = StartCoroutine(TypeLine(currentLines[currentLineIndex]));
        currentLineIndex++;
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        infoText.text = "";

        foreach (char c in line.ToCharArray())
        {
            infoText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        //activer l'indicateur de continuation
        if (continueTextIndicator)
        {
            continueTextIndicator.SetActive(true);
        }
    }

    private void CompleteTyping()
    {
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }

        infoText.text = currentLines[currentLineIndex - 1];
        isTyping = false;

        //activer l'indicateur de continuation
        if (continueTextIndicator)
        {
            continueTextIndicator.SetActive(true);
        }
    }

    public void EndText()
    {
        isTextActive = false;
        infoPanel.SetActive(false);
    }

    public bool IsTextActive()
    {
        return isTextActive;
    }
}