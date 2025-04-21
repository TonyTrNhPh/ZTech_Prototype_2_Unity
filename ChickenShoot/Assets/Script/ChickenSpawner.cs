using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    public GameObject chickenPrefab; // Prefab của Chicken
    public float spawnRate = 1f;     // Tần suất sinh (giây)
    public float spawnXMin = -8f;    // Giới hạn X nhỏ nhất
    public float spawnXMax = 8f;     // Giới hạn X lớn nhất
    public float spawnY = 6f;        // Vị trí Y cố định
    public int maxChickens = 10;     // Số lượng gà tối đa trên màn hình

    private float nextSpawn = 0f;
    private int currentChickens = 0; // Đếm số gà hiện tại

    private Camera mainCamera;

    void Start()
    {
        // Lấy camera chính
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Không tìm thấy Main Camera!");
            enabled = false;
            return;
        }

        // Tính toán giới hạn dựa trên camera
        float camHeight = 2f * mainCamera.orthographicSize; // Chiều cao khung hình
        float camWidth = camHeight * mainCamera.aspect;     // Chiều rộng khung hình

        // Cập nhật giới hạn X và Y
        spawnXMin = -camWidth / 2f + 0.5f;  // Thêm khoảng đệm 0.5 để không sát biên
        spawnXMax = camWidth / 2f - 0.5f;
        spawnY = camHeight / 2f - 0.5f;    // Đặt Y ở gần mép trên khung hình

        // Kiểm tra xem chickenPrefab đã được gán chưa
        if (chickenPrefab == null)
        {
            Debug.LogError("Chicken Prefab chưa được gán trong Inspector!");
            enabled = false; // Tắt script để tránh lỗi
        }

        
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing) 
        {
            // Chỉ sinh gà nếu chưa vượt quá số lượng tối đa
            if (currentChickens < maxChickens && Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnRate;
                Vector3 spawnPosition = new Vector3(Random.Range(spawnXMin, spawnXMax), spawnY, 0);
                GameObject chicken = Instantiate(chickenPrefab, spawnPosition, Quaternion.identity);
                currentChickens++; // Tăng số đếm

                // Đảm bảo scale của gà hợp lý
                chicken.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // Điều chỉnh nếu cần

                // Gán các thông số chuyển động ngẫu nhiên
                Chicken chickenScript = chicken.GetComponent<Chicken>();
                if (chickenScript != null)
                {
                    chickenScript.horizontalSpeed = Random.Range(1f, 3f); // Tốc độ ngang ngẫu nhiên
                    chickenScript.amplitude = Random.Range(0.5f, 2f); // Biên độ ngang ngẫu nhiên
                    chickenScript.verticalSpeed = Random.Range(0.5f, 2f); // Tốc độ dọc ngẫu nhiên
                    chickenScript.verticalAmplitude = Random.Range(0.3f, 1f); // Biên độ dọc ngẫu nhiên
                }
            }
        }
    }

    // Hàm để giảm số đếm khi gà bị tiêu diệt
    public void OnChickenDestroyed()
    {
        currentChickens--;
    }

    public void ResetSpawner()
    {
        currentChickens = 0;
        nextSpawn = 0f;
    }
}