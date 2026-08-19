using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ButtonTriggerMultiple : MonoBehaviour
{
    [SerializeField] public List<GameObject> doors = new List<GameObject>(); // List to store multiple doors
    [SerializeField] public List<bool> initialDoorStates = new List<bool>(); // List to track initial states (open/closed) for each door
    public Sprite buttonOff;
    public Sprite buttonOn;
    private bool canPress = true;

    private bool doorsAreOpen = false; // Track if the doors are open or closed
    public SpriteRenderer indicator;
    private bool fIsPressed;
    private bool isPLayerOverSwitch;
    public AudioSource doorsSource;

    // Method to initialize doors to their specified states when the level starts
    private void Start()
    {
        InitializeDoorStates();
        this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOff;
        doorsSource = this.GetComponent<AudioSource>();
        indicator.enabled = false;
    }

    // Set the initial states of doors based on the `initialDoorStates` list

    void Update()
    {
        if (isPLayerOverSwitch && canPress)
        {
            if (!fIsPressed && Keyboard.current.fKey.isPressed)
            {
                changeDoors();
            }
            if (!Keyboard.current.fKey.isPressed)
            {
                fIsPressed = false;
            }
        }

    }
    private void InitializeDoorStates(bool isReset = false)
    {
        this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOff;
        bool playOnce = true;
        for (int i = 0; i < doors.Count; i++)
        {
            // Open doors i
            // initially based on the initialDoorStates list
            if (isReset && playOnce && initialDoorStates[i] != doors[i].GetComponent<Animator>().GetBool("OpenDoor"))
            {
                playOnce = false;
                doorsSource.Play();
            }
            if (initialDoorStates[i])
                {
                    OpenSpecificDoor(i);
                }
                else
                {
                    CloseSpecificDoor(i);
                }
        }
    }
    public void initalDoor()
    {
        InitializeDoorStates(true);
    }

    public void changeDoors()
    {
        if (this.GetComponentInChildren<SpriteRenderer>().sprite == buttonOff)
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOn;
        }
        else if (this.GetComponentInChildren<SpriteRenderer>().sprite == buttonOn)
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOff;
        }
        else
        {
        }
        for (int i = 0; i < doors.Count; i++)
        {
            bool isDoorOpen = doors[i].GetComponent<Animator>().GetBool("OpenDoor");

            // Open doors initially based on the initialDoorStates list
            if (!isDoorOpen)
            {
                OpenSpecificDoor(i);
            }
            else
            {
                CloseSpecificDoor(i);
            }
        }
        fIsPressed = true;
        if (!doorsSource.isPlaying)
        {
            //doorsSource.Play();
        }
        doorsSource.Play();
    }
    public void activateDoors(bool value)
    {
        canPress = value;
        if (value == false)
        {
            // isPLayerOverSwitch = false;
            indicator.enabled = false;
        }
    }
    public void resetDoors()
    {

    }

    // Open a specific door
    private void OpenSpecificDoor(int doorIndex)
    {
        doors[doorIndex].GetComponent<BoxCollider2D>().isTrigger = true;
        doors[doorIndex].GetComponent<Animator>().SetBool("OpenDoor", true);
    }

    // Close a specific door
    private void CloseSpecificDoor(int doorIndex)
    {
        doors[doorIndex].GetComponent<BoxCollider2D>().isTrigger = false;
        doors[doorIndex].GetComponent<Animator>().SetBool("OpenDoor", false);
    }

    // Detect when something enters the button trigger area
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if (canPress)
            {
                isPLayerOverSwitch = true;
                indicator.enabled = true;
            }
        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CutscenePlayer"))
        {
            other.GetComponent<ShadowPlayerMovement>().assignCutsceneLever(this.gameObject);
        }
    }

    // Detect when something exits the button trigger area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (canPress)
        {
            if (other.CompareTag("Player"))
            {
                //  this.GetComponentInChildren<SpriteRenderer>().sprite = buttonOff;
                isPLayerOverSwitch = false;
                indicator.enabled = false;
            }
        }

    }
    
}
