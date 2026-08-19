using UnityEngine;

public class FixHourglass : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //gameObject.GetComponent<Animator>().SetTrigger("FixHourGlass");
            //this.GetComponent<PlaySoundEffect>().PlaySFX(0);
        }
    }

    public void FixHourGlass()
    {
        gameObject.GetComponent<Animator>().SetTrigger("FixHourGlass");
        this.GetComponent<PlaySoundEffect>().PlaySFX(0);
    }
}
