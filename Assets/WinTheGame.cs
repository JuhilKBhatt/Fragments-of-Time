using UnityEngine;

public class WinTheGame : MonoBehaviour
{
    public GameObject winObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WIN()
    {
        Time.timeScale = 0;
        winObject.SetActive(true);
    }
}
