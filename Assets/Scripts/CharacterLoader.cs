using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public CharacterData[] characterOptions;

    void Start()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacter", 0);
        ApplyCharacter(characterOptions[index]);
    }

    public void ApplyCharacter(CharacterData data)
    {
        spriteRenderer.sprite = data.portrait;
        animator.runtimeAnimatorController = data.animatorController;
        transform.position = data.spawnPosition;
    }

    // Pour un changement de personnage en cours de jeu
    public void SwitchCharacter(int index)
    {
        PlayerPrefs.SetInt("SelectedCharacter", index);
        ApplyCharacter(characterOptions[index]);
    }
}
