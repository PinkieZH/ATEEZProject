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
    public float loadingDuration = 2f;
    public string characterSelectionSceneName = "CharacterSelection";

    [Header("Debug")]
    public bool debugMode = true;

    public static ItemInteractionManager Instance;
    private int compteur = 0;


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

        if (debugMode)
        {
            Debug.Log($"Personnage: {currentCharacterData?.characterName}");
            Debug.Log($"Items requis: {GetItemsSpeciauxRequis()}");
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
            StartCoroutine(SequenceFinDeJeu());
        }
    }

    IEnumerator SequenceFinDeJeu()
    {
        fadeCanvas.SetActive(true);
        yield return StartCoroutine(FadeToBlack());
        yield return StartCoroutine(TextSlow());
        yield return new WaitForSeconds(textDisplayDuration);
        yield return StartCoroutine(LoadingScreen());
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

    IEnumerator LoadingScreen()
    {
        // Vous pouvez ajouter ici une barre de chargement ou des points qui s'ajoutent
        string baseText = transitionText.text;

        for (int i = 0; i < 3; i++)
        {
            transitionText.text = baseText + "\n\nChargement" + new string('.', i + 1);
            yield return new WaitForSeconds(loadingDuration / 3f);
        }
    }

    void CharaSelectorScene()
    {
        if (debugMode)
            Debug.Log($"Chargement de la sc�ne: {characterSelectionSceneName}");

        SceneManager.LoadScene(characterSelectionSceneName);
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
