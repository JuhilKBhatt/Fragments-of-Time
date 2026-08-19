using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public bool DrainsWater = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RollableObject"))//when colliding with a rollable object
        {
            if(DrainsWater == true)
            {
                FindAnyObjectByType<PrefabSceneManager>().DrainWaterFromScene();
            }
            this.GetComponent<PlaySoundEffect>().PlaySFX(0);
            Destroy(collision.gameObject);//destroys the sphere
            this.GetComponent<SpriteRenderer>().enabled = false;
            FindAnyObjectByType<CameraShakeManager>().CameraShake();
            this.GetComponent<BoxCollider2D>().enabled = false;
            Invoke("waitToDestroy", 5);
        }
    }

    private void waitToDestroy()
    {
        Destroy(gameObject);
    }
}