using UnityEngine;

public class SaveRollingObjectVelocity : MonoBehaviour
{
    public Vector2 savedVelocity;
    void OnDisable()
    {
        Rigidbody2D tempBody = gameObject.GetComponent<Rigidbody2D>();
        savedVelocity = new Vector2(tempBody.linearVelocityX, tempBody.linearVelocityY);

    }
    void OnEnable()
    {
        Rigidbody2D tempBody = gameObject.GetComponent<Rigidbody2D>();
        tempBody.linearVelocity = new Vector2(savedVelocity.x, savedVelocity.y);

    }
}
