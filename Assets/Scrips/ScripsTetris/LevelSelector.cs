using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public void SelectLevel(int level)
    {
        // Gán giá trị level vào GameSettings
        GameSetting.SelectedLevel = level;
        Debug.Log("Level được chọn: " + GameSetting.SelectedLevel);
    }

    public void StartGame()
    {
        Debug.Log("da chuyen level");
        // Chuyển sang màn chơi
        UnityEngine.SceneManagement.SceneManager.LoadScene("GiaoDienChinh");
    }
}
