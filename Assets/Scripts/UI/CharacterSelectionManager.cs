using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    [Header("Character Data")]
    public CharacterData[] characters;
    public CharacterData selectedCharacter;

    [Header("UI Elements")]
    public GameObject characterButtonPrefab;
    public Transform gridPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image portraitImage;
    public Button startButton;
    public GameObject infoPanel;
    public GameObject namePanel;


    [Header("Scene Management")]
    public string[] scenesToHideUI = { "SceneUne", "IntroScene", "MainMenu", "CharacterSelection", "CutsceneStart" };

    [Header("Debug")]
    public bool debugMode = true;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool shouldHideUI = false;

        foreach (string sceneName in scenesToHideUI)
        {
            if (scene.name == sceneName)
            {
                shouldHideUI = true;
                break;
            }
        }

        if (shouldHideUI)
        {
            HideUI();
            if (debugMode)
                Debug.Log($"UI cach�e pour la sc�ne: {scene.name}");
        }
        else
        {
            ShowUI();
            if (debugMode)
                Debug.Log($"UI cach�e pour la sc�ne: {scene.name}");
        }
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        gameObject.SetActive(true);
    }


    void Start()
    {
        InitializeUI();
        CreateCharacterButtons();
    }
    void InitializeUI()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
        if (namePanel != null)
            namePanel.SetActive(false);

        if (startButton != null)
            startButton.interactable = false;

        if (debugMode)
            Debug.Log("UI initialis�e");
    }

    void CreateCharacterButtons()
    {
        if (characters == null || characters.Length == 0)
        {
            Debug.LogError("Aucun personnage assign� dans le CharacterSelectionManager!");
            return;
        }

        if (characterButtonPrefab == null)
        {
            Debug.LogError("CharacterButtonPrefab non assign�!");
            return;
        }

        if (gridPanel == null)
        {
            Debug.LogError("GridPanel non assign�!");
            return;
        }

        // Nettoyer les anciens boutons si ils existent
        foreach (Transform child in gridPanel)
        {
            Destroy(child.gameObject);
        }

        // Cr�er les boutons pour chaque personnage
        for (int i = 0; i < characters.Length; i++)
        {
            CharacterData character = characters[i];

            if (character == null)
            {
                Debug.LogWarning($"Personnage � l'index {i} est null!");
                continue;
            }

            GameObject btn = Instantiate(characterButtonPrefab, gridPanel);

            // Configurer l'image du portrait
            Image portraitImg = btn.transform.Find("PortraitImage")?.GetComponent<Image>();
            if (portraitImg != null && character.portrait != null)
            {
                portraitImg.sprite = character.portrait;
            }
            else if (debugMode)
            {
                Debug.LogWarning($"Portrait manquant pour {character.characterName}");
            }

            // Configurer le nom si il y a un texte
            TextMeshProUGUI nameLabel = btn.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            if (nameLabel != null)
            {
                nameLabel.text = character.characterName;
            }

            // Configurer le bouton
            Button button = btn.GetComponent<Button>();
            if (button != null)
            {
                // Capture de la variable locale pour �viter les probl�mes de closure
                CharacterData selectedChar = character;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnCharacterSelected(selectedChar));
            }

            if (debugMode)
                Debug.Log($"Bouton cr�� pour: {character.characterName}");
        }
    }

    void OnCharacterSelected(CharacterData character)
    {
        if (character == null)
        {
            Debug.LogError("Personnage s�lectionn� est null!");
            return;
        }

        selectedCharacter = character;

        if (debugMode)
            Debug.Log($"Personnage s�lectionn�: {character.characterName}");

        // Activer le panneau d'info
        if (infoPanel != null)
            infoPanel.SetActive(true);
        if (namePanel != null)
            namePanel.SetActive(true);

        // Mettre � jour les informations
        if (nameText != null)
            nameText.text = character.characterName;

        if (descriptionText != null)
            descriptionText.text = character.description;

        if (portraitImage != null && character.portrait != null)
            portraitImage.sprite = character.portrait;

        // Activer le bouton de d�marrage
        if (startButton != null)
            startButton.interactable = true;
    }

    public void StartGame()
    {
        if (selectedCharacter == null)
        {
            Debug.LogError("Aucun personnage s�lectionn�!");
            return;
        }

        if (debugMode)
            Debug.Log($"D�marrage du jeu avec: {selectedCharacter.characterName}");

        // S'assurer que le GameManager existe et est configur�
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance est null! Assurez-vous qu'il existe dans la sc�ne.");
            return;
        }

        // Assigner le personnage s�lectionn�
        GameManager.Instance.selectedCharacter = selectedCharacter;

        // Charger la sc�ne de jeu
        try
        {
            SceneManager.LoadScene("SceneUne");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erreur lors du chargement de la sc�ne 'SceneUne': {e.Message}");
        }
    }

    // M�thodes utilitaires pour debug
    [ContextMenu("Debug Character Count")]
    public void DebugCharacterCount()
    {
        Debug.Log($"Nombre de personnages: {characters?.Length ?? 0}");
    }

    [ContextMenu("List All Characters")]
    public void ListAllCharacters()
    {
        if (characters == null) return;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != null)
                Debug.Log($"{i}: {characters[i].characterName}");
            else
                Debug.Log($"{i}: NULL");
        }
    }
}

