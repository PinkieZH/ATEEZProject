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

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        infoPanel.SetActive(false);
        // Créer les boutons pour chaque perso
        foreach (var character in characters)
        {
            GameObject btn = Instantiate(characterButtonPrefab, gridPanel);
            btn.transform.Find("PortraitImage").GetComponent<Image>().sprite = character.portrait;

            btn.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(character));
        }

        startButton.interactable = false;
    }

    void OnCharacterSelected(CharacterData character)
    {
        selectedCharacter = character;

        infoPanel.SetActive(true);

        nameText.text = character.characterName;
        descriptionText.text = character.description;
        portraitImage.sprite = character.portrait;

        startButton.interactable = true;
    }

    public void StartGame()
    {
        if (selectedCharacter != null)
        {
            SceneManager.LoadScene("SceneUne");
        }
    }
}
