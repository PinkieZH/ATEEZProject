using UnityEngine;
using UnityEngine.SceneManagement;

public class VisibilityController : MonoBehaviour
{
    [Header("Scene Management")]
    public string[] scenesToHideIn = { "CharacterSelection", "MainMenu", "CutsceneStart", "IntroScene" }; // Scènes où cacher ce GameObject

    [Header("Debug")]
    public bool debugMode = true;

    void Awake()
    {
        // S'abonner aux événements de changement de scène
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Vérifier la scène actuelle au démarrage
        CheckCurrentScene();
    }

    void OnDestroy()
    {
        // Se désabonner des événements pour éviter les memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Gérer la visibilité selon la scène
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneVisibility(scene.name);
    }

    // Vérifier la scène actuelle
    private void CheckCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        CheckSceneVisibility(currentSceneName);
    }

    // Vérifier si l'objet doit être visible dans cette scène
    private void CheckSceneVisibility(string sceneName)
    {
        bool shouldHide = false;

        // Vérifier si la scène actuelle est dans la liste des scènes où cacher l'objet
        foreach (string sceneToHide in scenesToHideIn)
        {
            if (sceneName == sceneToHide)
            {
                shouldHide = true;
                break;
            }
        }

        // Cacher ou afficher l'objet selon la scène
        if (shouldHide)
        {
            HideManagers();
            if (debugMode)
                Debug.Log($"Managers cachés dans la scène: {sceneName}");
        }
        else
        {
            ShowManagers();
            if (debugMode)
                Debug.Log($"Managers affichés dans la scène: {sceneName}");
        }
    }

    // Cacher les managers
    private void HideManagers()
    {
        gameObject.SetActive(false);
    }

    // Afficher les managers
    private void ShowManagers()
    {
        gameObject.SetActive(true);
    }
}
