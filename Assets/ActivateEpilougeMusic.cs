using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class ActivateEpilougeMusic : MonoBehaviour
{
    public AudioSource aBitOfHope;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //aBitOfHope.Play();
            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().TurnOnEpilougeMusic();
        }
    }
}
