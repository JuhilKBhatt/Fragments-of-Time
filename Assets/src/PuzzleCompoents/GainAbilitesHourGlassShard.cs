using UnityEngine;

public class GainAbilitesHourGlassShard : MonoBehaviour
{
    public GameObject[] stuffToActivate;
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
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Oh Noes, The artifact has been broken");
            FindAnyObjectByType<EchoAbility>().canNowEcho();
            FindAnyObjectByType<EchoAbility>().enableStuffAgain();
            FindAnyObjectByType<PrefabSceneManager>().CanUseAbility();
            for (int i = 0; i < stuffToActivate.Length; i++)
            {
                stuffToActivate[i].SetActive(true);
            }
            Destroy(gameObject);

        }
    }
}
