using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameSelector : MonoBehaviour
{
    // Start is called before the first frame update
    

    public void loadTetris()
    {
        SceneManager.LoadScene("menuPlayGame");
    }
    public void loadMissile()
    {
        SceneManager.LoadScene("sceneMissile");
    }
}
