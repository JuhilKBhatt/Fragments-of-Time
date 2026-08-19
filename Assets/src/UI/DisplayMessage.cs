using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // TextMeshPro

public class MessageDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    [SerializeField] private string[] messages; // Array to hold the messages
    private int currentMessageIndex = 0;
    private bool isDisplayingMessage = false;

    [SerializeField] private float messageDisplayTime = 2f; // Time to live

    private float timer = 0f;

    void Start()
    {
        // Initially hide the message text
        messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        // handle the progression
        if (currentMessageIndex < messages.Length)
        {
            if (!isDisplayingMessage)
            {
                ShowNextMessage();
            }
            else
            {
                // start the time to live timer to move to the next message
                timer += Time.deltaTime;

                if (timer >= messageDisplayTime)
                {
                    HideCurrentMessage();
                }
            }
        }
    }

    // show the next message
    private void ShowNextMessage()
    {
        if (currentMessageIndex < messages.Length)
        {
            // Display the message
            messageText.gameObject.SetActive(true);
            messageText.text = messages[currentMessageIndex];

            // Mark the message as being displayed
            isDisplayingMessage = true;

            // Reset the time to live timer
            timer = 0f;
        }
    }

    // hide the current message and move to the next one
    private void HideCurrentMessage()
    {
        messageText.gameObject.SetActive(false);
        isDisplayingMessage = false;

        currentMessageIndex++; // Move to the next message
        if(currentMessageIndex >= messages.Length)
        {
            currentMessageIndex = 0;
        }

        // Reset the time to live timer
        timer = 0f;
    }
}