using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class echoDetector : MonoBehaviour
{
    public bool allowEcho = true;
    public bool originalMapFuture = false;
    
    void Awake()
    {
        this.GetComponent<CapsuleCollider2D>().enabled = true;
        if(FindAnyObjectByType<PrefabSceneManager>().currentScene == SceneType.futureScene)
        {
            originalMapFuture =true;
        }
        else
        {
            originalMapFuture = false;
        }
    }


    public void CheckForCollisions(SceneType currentScene)
    {
        if(currentScene == SceneType.futureScene && originalMapFuture)
        {
            FindAnyObjectByType<EchoAbility>().isEchoOverlap(true);
            return;
        }
        else if(currentScene == SceneType.pastScene && !originalMapFuture)
        {
            FindAnyObjectByType<EchoAbility>().isEchoOverlap(true);
            return;
        }
      
        List<Collider2D> results = new List<Collider2D>();
        int hitCount = this.GetComponent<CapsuleCollider2D>().Overlap(results);
        for(int i = 0; i < hitCount; i++)
        {
            if(results[i].gameObject.CompareTag("PastTileMap") && originalMapFuture == true)
            {
                FindAnyObjectByType<EchoAbility>().isEchoOverlap(false);
                Debug.Log("We overlapping");
                return;
            }
            else if(results[i].gameObject.CompareTag("FutureTileMap") && originalMapFuture == false)
            {
                FindAnyObjectByType<EchoAbility>().isEchoOverlap(false);
                Debug.Log("We overlapping");
                return;
            }
        }
        FindAnyObjectByType<EchoAbility>().isEchoOverlap(true);
    }
}
