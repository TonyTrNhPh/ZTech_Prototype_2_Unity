using UnityEngine;

public class Egg : MonoBehaviour
{
    private float speed = 5f; // Tốc độ rơi của trứng

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Trứng trúng tàu! Game Over!");
            GameManager.Instance.GameOver();// Gọi màn hình game over
            Destroy(gameObject);

        }
    }
}
