using UnityEngine;

public class PushableBox : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isFloating = false;
    public bool startsInWater = false;
    //public bool 

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //rb.constraints = RigidbodyConstraints2D.None;
            //gameObject.GetComponent<Animator>().SetBool(I)

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.GetComponent<PlayerMovement>().canPlayerPullBox(true, rb);


            //rb.constraints = RigidbodyConstraints2D.None;
            //gameObject.GetComponent<Animator>().SetBool(I)

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (rb.bodyType != RigidbodyType2D.Static)
            {
                collision.GetComponent<PlayerMovement>().canPlayerPullBox(false);
            }


            //rb.constraints = RigidbodyConstraints2D.None;
                //gameObject.GetComponent<Animator>().SetBool(I)

            }
    }
}
