using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void Reset()
    {
       // Time.timeScale = 1; // Đặt lại tốc độ thời gian
        SceneManager.LoadScene("GiaoDienChinh");
    }
    public void endgame()
    {
        Application.Quit();
    }   
    public void Home()
    {
        SceneManager.LoadScene("menuPlayGame");
    }

}
