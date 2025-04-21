using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton pattern để đảm bảo chỉ có 1 instance
    public static GameManager Instance { get; private set; }

    // Các trạng thái của game
    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; }
    private int score = 0;

    // Tham chiếu đến các panel UI
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        // Xử lý singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ qua các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentState = GameState.MainMenu;
        ShowStartMenu();
    }

    // Khởi tạo game
    public void StartGame()
    {
        CurrentState = GameState.Playing;
        startPanel.SetActive(false);
        // Thêm logic khởi tạo game ở đây
    }

    // Tạm dừng game
    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            CurrentState = GameState.Paused;
            Time.timeScale = 0f; // Dừng thời gian game
        }
    }

    // Tiếp tục game
    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f; // Khôi phục thời gian
        }
    }

    // Kết thúc game
    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        ShowGameOver();
        // Thêm logic kết thúc game
        GameObject[] chickens = GameObject.FindGameObjectsWithTag("Chicken");
        foreach (GameObject chicken in chickens)
        {
            Destroy(chicken);
        }
        // Xóa trứng
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("Egg");
        foreach (GameObject egg in eggs)
        {
            Destroy(egg);
        }
    }

    // Restart game
    public void GameRestart()
    {
        CurrentState = GameState.Playing;
        gameOverPanel.SetActive(false);
        // Thêm logic restart game
        scoreText.text = score.ToString();
        ChickenSpawner spawner = FindObjectOfType<ChickenSpawner>();
        if (spawner != null)
        {
            spawner.ResetSpawner();
        }
    }

    // Exit game
    public void GameExit()
    {
        CurrentState = GameState.GameOver;
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        // Thêm logic kết thúc game
    }

    // Quay về menu chính
    public void ReturnToMenu()
    {
        CurrentState = GameState.MainMenu;
        ShowGameOver();
        // Thêm logic load scene menu nếu cần
    }

    // Hiển thị menu bắt đầu
    public void ShowStartMenu()
    {
        scoreText.text = score.ToString();
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    // Hiển thị màn hình game over
    public void ShowGameOver()
    {
        score = 0;
        gameOverPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    // Tăng score lên 1
    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
