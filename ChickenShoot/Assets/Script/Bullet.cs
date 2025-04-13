using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * speed;
    }

    void Update()
    {
        if (transform.position.y > 10f)
            Destroy(gameObject);
    }

}
