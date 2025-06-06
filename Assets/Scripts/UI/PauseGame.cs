using UnityEngine;

public class PauseGame : MonoBehaviour

{
    [SerializeField]
    private GameObject menuPause;

    //[SerializeField] AudioManager audioManager;

    private void Awake()
    {
        Resume();
    }

    private void Paused()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0f;
        //audioManager.LowPassMusic(true);

    }
    private void Resume()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1f;
        //audioManager.LowPassMusic(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                Paused();
            }
            else if (Time.timeScale == 0f)
            {
                Resume();
            }
        }
    }
}

