using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class WebOrWindows : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject windowsClip;
    public GameObject webGLClip;
    public int mainMenuScene = 0;
    public bool load = false;
    [SerializeField] string videoFileName; 
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            webGLClip.SetActive(true);
            VideoPlayer player = webGLClip.GetComponent<VideoPlayer>();
            if (player)
            {
                string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
                player.url = videoPath;
                player.Play();            }

            

        }
        else 
        {
            webGLClip.SetActive(false);
            windowsClip.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !load)
        {
            load = true;
            SceneManager.LoadSceneAsync(mainMenuScene);
        }
        
    }
}
