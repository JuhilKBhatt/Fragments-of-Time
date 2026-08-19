using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private PrefabSceneManager manager;
    public bool DontActivateYet = false;
    public bool canGoBack = false;
    public bool entered = false;
    public bool allowGoingBack;
    public bool changeTimePeriod = false;

    void Awake()
    {
        manager = GameObject.Find("SceneManagerObject").GetComponent<PrefabSceneManager>();
    }
    public bool loadNextRoom;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !DontActivateYet)
        {
            if (loadNextRoom && !canGoBack)
            {
                Debug.Log("Loading next room");
                manager.GoForwardOneLevel(this.gameObject);
                if (allowGoingBack)
                {
                    canGoBack = true;
                }
            }
            else if (canGoBack && entered == false)
            {
                Debug.Log("Loading last room");
                //manager.MoveCamera(-15);
                canGoBack = false;
                manager.GoBackOneLevel(this.gameObject);
            }
            if (changeTimePeriod)
            {
                GameObject.FindGameObjectWithTag("SceneManagerObject").GetComponent<PrefabSceneManager>().epilougeTimeTravel();
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && DontActivateYet)
        {
            DontActivateYet = false;
        }
        

    }
}
