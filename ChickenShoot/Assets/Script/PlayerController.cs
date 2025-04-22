using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    private float minX, minY, maxX, maxY;
    private float spriteWìdth, spriteHeight;

    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    private float nextFire = 0f;

    public AudioClip shootSound;
    private AudioSource audioSource;

    void Start()
    {
        // Lấy tham số camera chính
        mainCamera = Camera.main;

        // Tính toán ranh giới màn hình dựa trên camera
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;

        // Lấy kích thước của sprite để không bị cắt khi chạm biên
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteWìdth = spriteRenderer.bounds.size.x / 2; // Nửa chiều rộng
            spriteHeight = spriteRenderer.bounds.size.y / 2; // Nửa chiều cao
        }
        else
        {
            spriteWìdth = 0.5f; // Giá trị mặc định nếu không có SpriteRenderer
            spriteHeight = 0.5f;
        }

        // Điều chỉnh ranh giới để sprite không vượt quá biên
        minX += spriteWìdth;
        maxX -= spriteWìdth;
        minY += spriteHeight;
        maxY -= spriteHeight;

        // Thêm và cấu hình AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = shootSound;
    }
    void Update()
    {
        // Lấy vị trí chuột trên màn hình và chuyển sang tọa độ trong game
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Giữ ở mặt phẳng 2D

        // Giới hạn vị trí trong ranh giới màn hình
        mousePosition.x = Mathf.Clamp(mousePosition.x, minX, maxX);
        mousePosition.y = Mathf.Clamp(mousePosition.y, minY, maxY);

        // Di chuyển tàu đến vị trí chuột
        transform.position = mousePosition;

        // Bắn khi nhấn chuột trái
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            audioSource.Play();
            nextFire = Time.time + fireRate;
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bulletPrefab.tag = "Bullet";
        }
    }

}
