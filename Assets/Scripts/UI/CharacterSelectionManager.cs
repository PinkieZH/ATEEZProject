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
                Debug.Log($"UI cachée pour la scène: {scene.name}");
        }
        else
        {
            ShowUI();
            if (debugMode)
                Debug.Log($"UI cachée pour la scène: {scene.name}");
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
            Debug.Log("UI initialisée");
    }

    void CreateCharacterButtons()
    {
        if (characters == null || characters.Length == 0)
        {
            Debug.LogError("Aucun personnage assigné dans le CharacterSelectionManager!");
            return;
        }

        if (characterButtonPrefab == null)
        {
            Debug.LogError("CharacterButtonPrefab non assigné!");
            return;
        }

        if (gridPanel == null)
        {
            Debug.LogError("GridPanel non assigné!");
            return;
        }

        // Nettoyer les anciens boutons si ils existent
        foreach (Transform child in gridPanel)
        {
            Destroy(child.gameObject);
        }

        // Créer les boutons pour chaque personnage
        for (int i = 0; i < characters.Length; i++)
        {
            CharacterData character = characters[i];

            if (character == null)
            {
                Debug.LogWarning($"Personnage à l'index {i} est null!");
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
                // Capture de la variable locale pour éviter les problèmes de closure
                CharacterData selectedChar = character;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnCharacterSelected(selectedChar));
            }

            if (debugMode)
                Debug.Log($"Bouton créé pour: {character.characterName}");
        }
    }

    void OnCharacterSelected(CharacterData character)
    {
        if (character == null)
        {
            Debug.LogError("Personnage sélectionné est null!");
            return;
        }

        selectedCharacter = character;

        if (debugMode)
            Debug.Log($"Personnage sélectionné: {character.characterName}");

        // Activer le panneau d'info
        if (infoPanel != null)
            infoPanel.SetActive(true);
        if (namePanel != null)
            namePanel.SetActive(true);

        // Mettre à jour les informations
        if (nameText != null)
            nameText.text = character.characterName;

        if (descriptionText != null)
            descriptionText.text = character.description;

        if (portraitImage != null && character.portrait != null)
            portraitImage.sprite = character.portrait;

        // Activer le bouton de démarrage
        if (startButton != null)
            startButton.interactable = true;
    }

    public void StartGame()
    {
        if (selectedCharacter == null)
        {
            Debug.LogError("Aucun personnage sélectionné!");
            return;
        }

        if (debugMode)
            Debug.Log($"Démarrage du jeu avec: {selectedCharacter.characterName}");

        // S'assurer que le GameManager existe et est configuré
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance est null! Assurez-vous qu'il existe dans la scène.");
            return;
        }

        // Assigner le personnage sélectionné
        GameManager.Instance.selectedCharacter = selectedCharacter;

        // Charger la scène de jeu
        try
        {
            SceneManager.LoadScene("SceneUne");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erreur lors du chargement de la scène 'SceneUne': {e.Message}");
        }
    }

    // Méthodes utilitaires pour debug
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

