using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;


    private void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;

        if (videoPlayer == null)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene("MainMenu");
    }
}

