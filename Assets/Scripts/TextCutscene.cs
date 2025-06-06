using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TextCutscene : MonoBehaviour
{
    public TextMeshProUGUI cutsceneText;
    public string[] lines;
    public float typingSpeed = 0.03f;
    private int index = 0;

    void Start()
    {
        StartCoroutine(TypeLine());
    }



    IEnumerator TypeLine()
    {
        cutsceneText.text = "";
        foreach (char c in lines[index].ToCharArray())
        {
            cutsceneText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (cutsceneText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                cutsceneText.text = lines[index];
            }
        }
    }

    void NextLine()
    {
        index++;
        if (index < lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            SceneManager.LoadScene("CharacterSelection");
        }
    }
}
