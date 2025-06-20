using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemInteractionManager : MonoBehaviour
{

    [Header("Configuration")]
    public CharacterData currentCharacterData;

    [Header("UI Elements")]
    public GameObject fadeCanvas;
    public Image fadeImage;
    public TextMeshProUGUI transitionText;

    [Header("Timing")]
    public float fadeDuration = 1f;
    public float textDisplayDuration = 3f;
    public float textSpeed = 0.03f;
    public float textFadeOutDuration = 1.5f;
    public float loadingDuration = 2f;
    public string characterSelectionSceneName = "CharacterSelection";

    [Header("Debug")]
    public bool debugMode = true;

    public static ItemInteractionManager Instance;
    private int compteur = 0;
    private bool waitingForTextToClose = false; // Nouvelle variable pour savoir si on attend
    


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (fadeCanvas != null)
        {
            fadeCanvas.SetActive(false);
        }

        if (transitionText != null)
        {
            transitionText.text = "";
        }
    }

    void Start()
    {
        // V�rifier si les donn�es de personnage sont assign�es
        if (currentCharacterData == null)
        {
            Debug.LogError("CharacterData non assign� dans ItemInteractionManager !");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (debugMode)
        {
            Debug.Log($"Personnage: {currentCharacterData?.characterName}");
            Debug.Log($"Items requis: {GetItemsSpeciauxRequis()}");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SceneUne")
        {
            ResetFadeCanvas();
            if (debugMode)
                Debug.Log($"FadeCanvas r�initialis� pour la sc�ne: {scene.name}");
        }
    }

    private void ResetFadeCanvas()
    {
        // D�sactiver le fadeCanvas
        if (fadeCanvas != null)
        {
            fadeCanvas.SetActive(false);
        }

        // Remettre l'alpha du fadeImage � 0
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        // Vider le texte de transition et remettre son alpha � 1
        if (transitionText != null)
        {
            transitionText.text = "";
            Color textColor = transitionText.color;
            textColor.a = 1f;
            transitionText.color = textColor;
        }
    }

    void Update()
    {
        // V�rifier si on attend que le texte se ferme
        if (waitingForTextToClose)
        {
            // Si ObjectsManager existe et que le texte n'est plus actif
            if (ObjectsManager.Instance != null && !ObjectsManager.Instance.IsTextActive())
            {
                waitingForTextToClose = false;
                // Maintenant on peut vraiment lancer la s�quence de fin
                StartCoroutine(SequenceFinDeJeu());
            }
        }
    }

    public int GetItemsSpeciauxRequis()
    {
        return currentCharacterData != null ? currentCharacterData.itemsSpeciauxRequis : 4;
    }

    public void CompterItemSpecial()
    {
        compteur++;
        if (debugMode)
            Debug.Log($"Items sp�ciaux interact�s : {compteur}/{GetItemsSpeciauxRequis()}");

        if (compteur >= GetItemsSpeciauxRequis())
        {
            // Au lieu de lancer directement la s�quence, on attend que le texte se ferme
            if (ObjectsManager.Instance != null && ObjectsManager.Instance.IsTextActive())
            {
                waitingForTextToClose = true;
                if (debugMode)
                    Debug.Log("En attente de la fermeture du texte avant la fin de jeu...");
            }
            else
            {
                // Si aucun texte n'est actif, lancer directement
                StartCoroutine(SequenceFinDeJeu());
            }
        }
    }

    IEnumerator SequenceFinDeJeu()
    {
        fadeCanvas.SetActive(true);
        yield return StartCoroutine(FadeToBlack());
        yield return StartCoroutine(TextSlow());
        yield return new WaitForSeconds(textDisplayDuration);
        yield return StartCoroutine(FadeTextOut());
        CharaSelectorScene();
    }
    IEnumerator FadeToBlack()
    {
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;
    }

    IEnumerator TextSlow()
    {
        if (transitionText == null || currentCharacterData == null)
            yield break;

        string texteComplet = currentCharacterData.finText;
        transitionText.text = "";

        for (int i = 0; i <= texteComplet.Length; i++)
        {
            transitionText.text = texteComplet.Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator FadeTextOut()
    {
        if (transitionText == null)
            yield break;

        Color textColor = transitionText.color;
        float startAlpha = textColor.a;

        float t = 0f;
        while (t < textFadeOutDuration)
        {
            t += Time.deltaTime;
            textColor.a = Mathf.Lerp(startAlpha, 0f, t / textFadeOutDuration);
            transitionText.color = textColor;
            yield return null;
        }
    }

    void CharaSelectorScene()
    {
        if (debugMode)
            Debug.Log($"Chargement de la sc�ne: {characterSelectionSceneName}");
       
        SceneManager.LoadScene(characterSelectionSceneName);
        ResetCompteur();
    }

    // M�thodes de debug
    [ContextMenu("Tester Fin de Jeu")]
    public void TesterFinDeJeu()
    {
        compteur = GetItemsSpeciauxRequis();
        CompterItemSpecial();
    }

    [ContextMenu("Reset Compteur")]
    public void ResetCompteur()
    {
        compteur = 0;
    }
}