using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    [Header("Character Components")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    [Header("Camera")]
    public CameraController cameraController;

    [Header("Debug")]
    public bool debugMode = true;

    void Start()
    {
        LoadSelectedCharacter();
    }

    void LoadSelectedCharacter()
    {
        // Vérifier que le GameManager existe
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance est null! Le personnage ne peut pas être chargé.");
            return;
        }

        // Vérifier qu'un personnage a été sélectionné
        if (GameManager.Instance.selectedCharacter == null)
        {
            Debug.LogError("Aucun personnage sélectionné dans le GameManager!");
            return;
        }

        CharacterData selectedCharacter = GameManager.Instance.selectedCharacter;

        if (debugMode)
            Debug.Log($"Chargement du personnage: {selectedCharacter.characterName}");

        ApplyCharacter(selectedCharacter);
    }

    public void ApplyCharacter(CharacterData data)
    {
        if (data == null)
        {
            Debug.LogError("CharacterData est null!");
            return;
        }

        // Appliquer le sprite
        if (spriteRenderer != null && data.portrait != null)
        {
            spriteRenderer.sprite = data.portrait;
            if (debugMode)
                Debug.Log($"Sprite appliqué: {data.portrait.name}");
        }
        else if (debugMode)
        {
            Debug.LogWarning("SpriteRenderer ou portrait manquant");
        }

        // Appliquer l'animator controller
        if (animator != null && data.animatorController != null)
        {
            animator.runtimeAnimatorController = data.animatorController;
            if (debugMode)
                Debug.Log($"Animator Controller appliqué: {data.animatorController.name}");
        }
        else if (debugMode)
        {
            Debug.LogWarning("Animator ou AnimatorController manquant");
        }

        // Téléporter le joueur
        if ((Vector3)data.spawnPosition != Vector3.zero)
        {
            transform.position = data.spawnPosition;
            if (debugMode)
                Debug.Log($"Joueur téléporté à: {data.spawnPosition}");
        }
        else
        {
            if (debugMode)
                Debug.LogWarning("Position de spawn non définie, utilisation de la position actuelle");
        }

        // Configurer la caméra pour suivre le joueur
        SetupCameraTarget();

        // Mettre à jour le ItemInteractionManager avec les données du personnage
        UpdateItemInteractionManager(data);
    }

    void SetupCameraTarget()
    {
        if (cameraController != null)
        {
            // Assigner le joueur comme target de la caméra
            cameraController.target = transform;

            // Téléporter la caméra instantanément (si la méthode existe)
            if (cameraController.GetComponent<CameraController>() != null)
            {
                // Utiliser la méthode TeleportToTarget si vous l'avez ajoutée
                cameraController.SendMessage("TeleportToTarget", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                // Sinon, forcer la position manuellement
                Vector3 desiredCameraPosition = transform.position + cameraController.offset;

                if (cameraController.useBoundaries)
                {
                    desiredCameraPosition.x = Mathf.Clamp(desiredCameraPosition.x, cameraController.minX, cameraController.maxX);
                    desiredCameraPosition.y = Mathf.Clamp(desiredCameraPosition.y, cameraController.minY, cameraController.maxY);
                }

                cameraController.transform.position = desiredCameraPosition;
            }

            if (debugMode)
                Debug.Log("Caméra téléportée instantanément au joueur");
        }
        else if (debugMode)
        {
            Debug.LogWarning("CameraController non assigné");
        }
    }

    void UpdateItemInteractionManager(CharacterData data)
    {
        // Mettre à jour le ItemInteractionManager avec les données du personnage
        if (ItemInteractionManager.Instance != null)
        {
            ItemInteractionManager.Instance.currentCharacterData = data;
            if (debugMode)
                Debug.Log($"ItemInteractionManager mis à jour avec les données de {data.characterName}");
        }
        else if (debugMode)
        {
            Debug.LogWarning("ItemInteractionManager.Instance non trouvé");
        }
    }
    // Pour un changement de personnage en cours de jeu
    public void SwitchCharacter(CharacterData newCharacter)
    {
        if (newCharacter == null)
        {
            Debug.LogError("Nouveau personnage est null!");
            return;
        }

        // Mettre à jour le GameManager
        GameManager.Instance.selectedCharacter = newCharacter;

        // Appliquer le nouveau personnage
        ApplyCharacter(newCharacter);

        if (debugMode)
            Debug.Log($"Personnage changé pour: {newCharacter.characterName}");
    }

    // Méthodes de debug
    [ContextMenu("Reload Character")]
    public void ReloadCharacter()
    {
        LoadSelectedCharacter();
    }

    [ContextMenu("Debug Current Character")]
    public void DebugCurrentCharacter()
    {
        if (GameManager.Instance?.selectedCharacter != null)
        {
            CharacterData data = GameManager.Instance.selectedCharacter;
            Debug.Log($"Personnage actuel: {data.characterName}");
            Debug.Log($"Position de spawn: {data.spawnPosition}");
            Debug.Log($"Items requis: {data.itemsSpeciauxRequis}");
        }
        else
        {
            Debug.Log("Aucun personnage sélectionné");
        }
    }
}
