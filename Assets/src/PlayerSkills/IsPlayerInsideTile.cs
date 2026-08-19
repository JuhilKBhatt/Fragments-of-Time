using UnityEngine;

public class IsPlayerInsideTile : MonoBehaviour
{
    private PrefabSceneManager manager;

    void Awake()
    {
        manager = GameObject.Find("SceneManagerObject").GetComponent<PrefabSceneManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
        Debug.Log("something has entered this ollider, its name is " + other.name);
        //GameObject.Find("SceneManagerObject").GetComponent<ManageScenes>().StopPlayerFromTimeTravel();
        manager.StopPlayerFromTimeTravel();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
        Debug.Log("something has entered this ollider, its name is " + other.name);
        //GameObject.Find("SceneManagerObject").GetComponent<ManageScenes>().AllowPlayerToTimeTravel();
        manager.AllowPlayerToTimeTravel();
        }

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && manager.canWeCurrentlyTimeTravel())
        {
            manager.StopPlayerFromTimeTravel();

        }
    }
}
