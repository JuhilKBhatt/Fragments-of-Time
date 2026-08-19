using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaterDetect : MonoBehaviour
{
    public bool Drainable = false;
    private List<Rigidbody2D> boxes;
    private bool canPlaySound = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxes = new List<Rigidbody2D>();
        Invoke("waitBeforeplaySound", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PushableBox"))
        {
            Debug.Log("This is where we would start pushing up the box");
            other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            if(!boxes.Contains(other.gameObject.GetComponent<Rigidbody2D>()))
            {
                boxes.Add(other.GetComponent<Rigidbody2D>());
            }
            if (canPlaySound == true && this.GetComponent<PlaySoundEffect>() != null)
            {
                if (other.GetComponent<PushableBox>().startsInWater)
                {
                    other.GetComponent<PushableBox>().startsInWater = false;
                }
                else
                {
                    this.GetComponent<PlaySoundEffect>().PlaySFX(0);
                }
            }
        }
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            other.GetComponent<PlayerMovement>().inWater = true;
            if (canPlaySound == true && this.GetComponent<PlaySoundEffect>() != null)
            {
                this.GetComponent<PlaySoundEffect>().PlaySFX(0);
            }
        }
    }
     void  OnTriggerStay2D(Collider2D other)
     {
        if (other.CompareTag("Player"))
        {
          
            other.GetComponent<PlayerMovement>().inWater = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            other.GetComponent<PlayerMovement>().inWater = false;
        }
    }
    void OnDisable()
    {
        canPlaySound = false;
        PlayerMovement player = null;
        try
        {
            if (FindAnyObjectByType<PlayerMovement>().inWater == true)
            {
                FindAnyObjectByType<PlayerMovement>().inWater = false;
                player = FindAnyObjectByType<PlayerMovement>();


            }
        }
        catch (Exception e)
        {
            print ("WHOOPSY " + e);
        }
            
        if (player != null && player.inWater)
        {
            player.inWater = false;
        }
    }
    void OnEnable()
    {
        Invoke("waitBeforeplaySound", 0.2f);
    }
    public void waitBeforeplaySound()
    {
        canPlaySound = true;
    }
    public void freeBoxes()
    {

    }

    void OnDestroy()
    {
        if (boxes == null) return;
        foreach (Rigidbody2D box in boxes)
        {
            if (box != null)
            {
                box.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}
