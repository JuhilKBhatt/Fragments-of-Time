using UnityEngine;

public class RevealButtons : MonoBehaviour
{
    [SerializeField] private GameObject[] hiddenButtons; // Array to hold references to the hidden buttons
    private bool canPress = true; // Whether the button can be pressed
    public Sprite buttonOff; // Sprite for the button when not pressed
    public Sprite buttonOn; // Sprite for the button when pressed
    public AudioSource revealButtonSound;
    
    private void RevealAllButtons()
    {
        foreach (GameObject button in hiddenButtons)
        {
            button.SetActive(true);  // Activate the hidden buttons
        }
    }

    // Detect when an object enters the button's trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PushableBox") || other.CompareTag("RollableObject"))
        {
            if (this.GetComponentInChildren<SpriteRenderer>().color.a != 0)
            {
                if (canPress && hiddenButtons[0].activeInHierarchy == false)
                {
                    this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOn;
                    RevealAllButtons();
                    revealButtonSound.Play();
                }
            }
        }
    }

    // Detect when something exits the button trigger area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (canPress)
        {
            if (other.CompareTag("Player") || other.CompareTag("PushableBox") || other.CompareTag("RollableObject"))
            {
                this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOff;
            }
        }
    }
}