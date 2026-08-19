using UnityEngine;

public class CompleteLevelScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
        Debug.Log("You completed the level");
        //do stuff to complete level, move to next level or bring up menu ect
        Debug.Log("Would also destroy this object here");
        this.GetComponent<PlaySoundEffect>().PlaySFX(0);
        FindAnyObjectByType<WinController>().Win();
        }
    }
}
