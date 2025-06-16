using UnityEngine;
using UnityEngine.SceneManagement;

public class VisibilityController : MonoBehaviour
{
    [Header("Scene Management")]
    public string[] scenesToHideIn = { "CharacterSelection", "MainMenu", "CutsceneStart", "IntroScene" }; // Sc�nes o� cacher ce GameObject

    [Header("Debug")]
    public bool debugMode = true;

    void Awake()
    {
        // S'abonner aux �v�nements de changement de sc�ne
        SceneManager.sceneLoaded += OnSceneLoaded;

        // V�rifier la sc�ne actuelle au d�marrage
        CheckCurrentScene();
    }

    void OnDestroy()
    {
        // Se d�sabonner des �v�nements pour �viter les memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // G�rer la visibilit� selon la sc�ne
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneVisibility(scene.name);
    }

    // V�rifier la sc�ne actuelle
    private void CheckCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        CheckSceneVisibility(currentSceneName);
    }

    // V�rifier si l'objet doit �tre visible dans cette sc�ne
    private void CheckSceneVisibility(string sceneName)
    {
        bool shouldHide = false;

        // V�rifier si la sc�ne actuelle est dans la liste des sc�nes o� cacher l'objet
        foreach (string sceneToHide in scenesToHideIn)
        {
            if (sceneName == sceneToHide)
            {
                shouldHide = true;
                break;
            }
        }

        // Cacher ou afficher l'objet selon la sc�ne
        if (shouldHide)
        {
            HideManagers();
            if (debugMode)
                Debug.Log($"Managers cach�s dans la sc�ne: {sceneName}");
        }
        else
        {
            ShowManagers();
            if (debugMode)
                Debug.Log($"Managers affich�s dans la sc�ne: {sceneName}");
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
