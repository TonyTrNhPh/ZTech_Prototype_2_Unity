using UnityEngine;

public class Chicken : MonoBehaviour
{
    [HideInInspector] public float verticalSpeed ; // Tốc độ đi xuống
    [HideInInspector] public float horizontalSpeed; // Tốc độ di chuyển ngang
    [HideInInspector] public float amplitude; // Biên độ zigzag
    [HideInInspector] public float verticalAmplitude; // Biên độ dao động dọc

    private float startX;// Vị trí x ban đầu
    private float startY;// Vị trí y ban đầu
    private float minY; // Giới hạn dưới của nửa trên khung hình
    private ChickenSpawner spawner;
    private Animator animator; // Tham chiếu đến Animator
    private Camera mainCamera;

    public float eggDropRate = 0.1f;
    public GameObject eggPrefab;

    public AudioClip chickenDieSound;

    void Start()
    {
        spawner = FindObjectOfType<ChickenSpawner>();
        animator = GetComponent<Animator>(); 

        startX = transform.position.x;
        startY = transform.position.y;

        // Tính giới hạn nửa trên khung hình
        float camHeight = 2f * mainCamera.orthographicSize;
        minY = (camHeight / 2) / 2f ;
        

        if (animator != null)
        {
            animator.Play("Chicken_Animation - FLY"); 
        }

        if (eggPrefab == null)
        {
            Debug.LogError("Egg Prefab chưa được gán cho Chicken!");

        }
    }

    void Update()
    {       
        float newX = startX + Mathf.Sin(Time.time * horizontalSpeed) * amplitude;

        
        float newY = startY + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;
        newY = Mathf.Clamp(newY, minY, startY);

        transform.position = new Vector3(newX, newY, 0);

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }

        // Thả trứng ngẫu nhiên
        if (Random.value < eggDropRate * Time.deltaTime)
        {
            Instantiate(eggPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Va chạm với: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Gà trúng tàu! Game Over!");
            GameManager.Instance.GameOver();// Gọi màn hình game over
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Va chạm với: " + collision.gameObject.name + ", Tag: " + collision.tag);
        if (collision.CompareTag("Bullet"))
        {
            AudioSource.PlayClipAtPoint(chickenDieSound, transform.position);
            Debug.Log("Đạn trúng gà!");
            GameManager.Instance.UpdateScore();
            Destroy(collision.gameObject); 
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnChickenDestroyed();
        }
    }
}