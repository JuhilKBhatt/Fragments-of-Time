using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource EpilougeMusic;
    public AudioSource generalAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        TurnOnGenMusic();
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TurnOnEpilougeMusic()
    {
        EpilougeMusic.Play();
        generalAudio.Stop();
    }
    public void TurnOnGenMusic()
    {
        generalAudio.Play();

    }
    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicManager");
        foreach (GameObject obj in objs)
        {
            if (obj.gameObject != this.gameObject)
            {
                Destroy(obj);
            }

        }
        if (scene.buildIndex == 0)
        {
            SceneManager.sceneLoaded -= onSceneLoaded;
            Destroy(this.gameObject);
        }
           }
}
