using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] public GameObject door; //door object
    public Sprite buttonOff;
    public Sprite buttonOn;
    private UnityEvent onSpriteRendererEnabled;
    private bool canPress = true;
    private bool justEnabled = false;
    //public bool activateToClose = false;

    private int objectsDetected = 0; //is there an object detected on the button? can add more things that interact with the buttons later maybe?
    public void ReTriggerDoor()
    {
        if(this.GetComponentInChildren<SpriteRenderer>().sprite.Equals( buttonOn)&& objectsDetected != 0)
        {
            //door.GetComponent<SpriteRenderer>().enabled = false;
            door.GetComponent<BoxCollider2D>().isTrigger = true;
            /*if (!door.GetComponent<Animator>().GetBool("OpenDoor") == false)
            {
                this.GetComponent<PlaySoundEffect>().PlaySFX(0);
            }*/
            door.GetComponent<Animator>().SetBool("OpenDoor", true);
            
        }
        else
        {
            if(objectsDetected == 0)
            {
                this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOff;
            }
           // door.GetComponent<SpriteRenderer>().enabled = true;
            door.GetComponent<BoxCollider2D>().isTrigger = false;
            door.GetComponent<Animator>().SetBool("OpenDoor", false);


        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") || other.CompareTag("PushableBox") || other.CompareTag("RollableObject") || other.CompareTag("CutscenePlayer"))
        {
            if(canPress)
            {
                if(other.gameObject.tag == "CutscenePlayer" || other.gameObject.tag == "Player" || other.gameObject.transform.parent.tag == this.gameObject.transform.parent.tag)
                {

            objectsDetected++; //count if object is on the button
            //door.SetActive(false); //activate door (make it disapear for now)
            if(this.GetComponentInChildren<SpriteRenderer>().enabled == true)
            {
              //  door.GetComponent<SpriteRenderer>().enabled = false;
                door.GetComponent<BoxCollider2D>().isTrigger = true;
                    if (door.GetComponent<Animator>().GetBool("OpenDoor") == false && !justEnabled)
                    {
                        this.GetComponent<PlaySoundEffect>().PlaySFX(0);
                    }
                    door.GetComponent<Animator>().SetBool("OpenDoor", true); 
                        }
            this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOn;
            //this.GetComponent<
            }
            }

        }
        
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if(canPress)
        {
            if (other.CompareTag("Player") || other.CompareTag("PushableBox") || other.CompareTag("RollableObject") || other.CompareTag("CutscenePlayer"))
        {
            objectsDetected--; //same thing but reduce if no object detected

            if (objectsDetected <= 0) 
            {

                //door.SetActive(true); //make door visable again
                if (this.GetComponentInChildren<SpriteRenderer>().enabled == true)
                {
                 //   door.GetComponent<SpriteRenderer>().enabled = true;
                    door.GetComponent<BoxCollider2D>().isTrigger = false;
                    door.GetComponent<Animator>().SetBool("OpenDoor", false);
                }
                this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOff;

            }
        
        }
        }
    }

    public void getInput(bool value)
    {
        justEnabled = value;
        canPress = value;
        objectsDetected = 0;
        gameObject.GetComponent<BoxCollider2D>().enabled = value;
        Invoke("CanMakeSound", 0.3f);
    }

    private void CanMakeSound()
    {
        justEnabled = false;
    }
}