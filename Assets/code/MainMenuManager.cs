using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Menu chính
    public GameObject gameplayCanvas; // Giao diện chơi game

    public void StartGame()
    {
        Debug.Log("Bắt đầu trò chơi!");
        mainMenuCanvas.SetActive(false); // Ẩn menu chính
        gameplayCanvas.SetActive(true); // Hiển thị giao diện chơi game
        // Có thể thêm logic khác để khởi động gameplay
    }

    public void OpenSettings()
    {
        Debug.Log("Mở cài đặt!");
        // Thêm logic mở giao diện cài đặt nếu cần
    }

    public void QuitGame()
    {
        Debug.Log("Thoát trò chơi!");
        Application.Quit(); // Thoát trò chơi (chỉ hoạt động sau khi Build)
    }
}