using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Phương thức Restart để khởi động lại cảnh
    public void RestartGame()
    {
        Debug.Log("Khởi động lại trò chơi!");
        Time.timeScale = 1f; // Khôi phục thời gian trò chơi
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Tải lại Scene hiện tại
    }

    // Phương thức Quit để thoát ứng dụng
    public void QuitGame()
    {
        Debug.Log("Thoát trò chơi!");
        Application.Quit(); // Thoát trò chơi (chỉ hoạt động khi đã build)
    }
}