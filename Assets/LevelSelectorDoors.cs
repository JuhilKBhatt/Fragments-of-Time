using UnityEngine;

public class LevelSelectorDoors : MonoBehaviour
{
    public int levelCompletionRequired = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("LevelsBeat") >= levelCompletionRequired)
        {
            this.gameObject.GetComponent<Animator>().SetBool("OpenDoor", true);
            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
