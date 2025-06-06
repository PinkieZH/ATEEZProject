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
        // V�rifier que le GameManager existe
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance est null! Le personnage ne peut pas �tre charg�.");
            return;
        }

        // V�rifier qu'un personnage a �t� s�lectionn�
        if (GameManager.Instance.selectedCharacter == null)
        {
            Debug.LogError("Aucun personnage s�lectionn� dans le GameManager!");
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
                Debug.Log($"Sprite appliqu�: {data.portrait.name}");
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
                Debug.Log($"Animator Controller appliqu�: {data.animatorController.name}");
        }
        else if (debugMode)
        {
            Debug.LogWarning("Animator ou AnimatorController manquant");
        }

        // T�l�porter le joueur
        if ((Vector3)data.spawnPosition != Vector3.zero)
        {
            transform.position = data.spawnPosition;
            if (debugMode)
                Debug.Log($"Joueur t�l�port� �: {data.spawnPosition}");
        }
        else
        {
            if (debugMode)
                Debug.LogWarning("Position de spawn non d�finie, utilisation de la position actuelle");
        }

        // Configurer la cam�ra pour suivre le joueur
        SetupCameraTarget();

        // Mettre � jour le ItemInteractionManager avec les donn�es du personnage
        UpdateItemInteractionManager(data);
    }

    void SetupCameraTarget()
    {
        if (cameraController != null)
        {
            // Assigner le joueur comme target de la cam�ra
            cameraController.target = transform;

            // T�l�porter la cam�ra instantan�ment (si la m�thode existe)
            if (cameraController.GetComponent<CameraController>() != null)
            {
                // Utiliser la m�thode TeleportToTarget si vous l'avez ajout�e
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
                Debug.Log("Cam�ra t�l�port�e instantan�ment au joueur");
        }
        else if (debugMode)
        {
            Debug.LogWarning("CameraController non assign�");
        }
    }

    void UpdateItemInteractionManager(CharacterData data)
    {
        // Mettre � jour le ItemInteractionManager avec les donn�es du personnage
        if (ItemInteractionManager.Instance != null)
        {
            ItemInteractionManager.Instance.currentCharacterData = data;
            if (debugMode)
                Debug.Log($"ItemInteractionManager mis � jour avec les donn�es de {data.characterName}");
        }
        else if (debugMode)
        {
            Debug.LogWarning("ItemInteractionManager.Instance non trouv�");
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

        // Mettre � jour le GameManager
        GameManager.Instance.selectedCharacter = newCharacter;

        // Appliquer le nouveau personnage
        ApplyCharacter(newCharacter);

        if (debugMode)
            Debug.Log($"Personnage chang� pour: {newCharacter.characterName}");
    }

    // M�thodes de debug
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
            Debug.Log("Aucun personnage s�lectionn�");
        }
    }
}
