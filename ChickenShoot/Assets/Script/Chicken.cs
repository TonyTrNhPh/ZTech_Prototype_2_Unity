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

    void Start()
    {
        spawner = FindObjectOfType<ChickenSpawner>();
        animator = GetComponent<Animator>(); // Lấy Animator

        startX = transform.position.x;
        startY = transform.position.y;

        // Tính giới hạn nửa trên khung hình
        float camHeight = 2f * mainCamera.orthographicSize;
        minY = (camHeight / 2) / 2f ;
        

        // Đảm bảo animation "FLY" được chạy
        if (animator != null)
        {
            animator.Play("Chicken_Animation - FLY"); // Tên animation có thể khác, kiểm tra trong Animator
        }
    }

    void Update()
    {       
       // Di chuyển ngang theo sóng sin (zigzag)
        float newX = startX + Mathf.Sin(Time.time * horizontalSpeed) * amplitude;

        // Dao động dọc trong nửa trên khung hình
        float newY = startY + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;
        newY = Mathf.Clamp(newY, minY, startY);

        transform.position = new Vector3(newX, newY, 0);

        // Xóa nếu ra khỏi màn hình
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Va chạm với: " + collision.gameObject.name + ", Tag: " + collision.tag);
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("Đạn trúng gà!");
            Destroy(collision.gameObject); // Xóa đạn
            Destroy(gameObject); // Xóa gà
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