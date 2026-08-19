using NUnit.Framework;
using UnityEngine;

public class BreakHourglass : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
          /* // Debug.Log("Oh Noes, The artifact has been broken");
            gameObject.GetComponent<Animator>().SetTrigger("BreakHourGlass");
            this.GetComponent<PlaySoundEffect>().PlaySFX(0);
            Invoke("DestroyHourGlassAndOtherStuff", 0.4f);*/
        }
    }
    public void destroyGlassOther()
    {
        gameObject.GetComponent<Animator>().SetTrigger("BreakHourGlass");
        this.GetComponent<PlaySoundEffect>().PlaySFX(0);
        Invoke("DestroyHourGlassAndOtherStuff", 0.4f);
    }

    public void DestroyHourGlassAndOtherStuff()
    {
        FindAnyObjectByType<PrefabSceneManager>().HourGlassBroken();
        //FindAnyObjectByType<EchoAbility>().canNowEcho();
        //Contact the manager script or something here;
        Destroy(gameObject, 0.2f);

    }
}
