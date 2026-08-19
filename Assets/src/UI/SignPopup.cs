
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SignPopup : MonoBehaviour
{
    public string Tip;
    private TMP_Text text;
    private Image panel;
    private GameObject[] extraSign;
    public SpriteRenderer inputPrompt;
    public bool playerIsOverSign;
    public bool isSignActivated;
    public bool fPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel = GameObject.FindGameObjectWithTag("SignPannel").GetComponent<Image>();
        text = GameObject.FindGameObjectWithTag("SignInfo").GetComponent<TMP_Text>();
        extraSign = GameObject.FindGameObjectsWithTag("ExtraSign");

        text.text = Tip;
        text.enabled = false;
        panel.enabled = false;
       /* foreach (GameObject obj in extraSign)
        {
            obj.SetActive(false);
        }*/
       

    }
    

    void Update()
    {
        if (playerIsOverSign == true && Keyboard.current.fKey.isPressed && !isSignActivated && !fPressed)
        {
            text.enabled = true;
            text.text = Tip;
            panel.enabled = true;
            isSignActivated = true;
            foreach (GameObject obj in extraSign)
            {
                obj.SetActive(true);
            }

        }
        else if (playerIsOverSign == true && Keyboard.current.fKey.isPressed && isSignActivated && !fPressed)
        {
            isSignActivated = false;
            text.enabled = false;
            panel.enabled = false;
            foreach (GameObject obj in extraSign)
            {
                obj.SetActive(false);
            }
        }

        if(Keyboard.current.fKey.isPressed)
        {
            fPressed = true;
        }
        else
        {
            fPressed = false;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //make something pop up
            Debug.Log("pop up with info");
            Debug.Log(Tip);
            //text.enabled = true;
           // panel.enabled = true;
            playerIsOverSign = true;
            inputPrompt.enabled = true;
        }
        
    }
    void  OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //make something disapear up
            Debug.Log("pop up with info");
            Debug.Log(Tip);
            text.enabled = false;
            panel.enabled = false;
            playerIsOverSign = false;
            isSignActivated = false;
            inputPrompt.enabled = false;
            foreach (GameObject obj in extraSign)
            {
                obj.SetActive(false);
            }
        }
    }
}
