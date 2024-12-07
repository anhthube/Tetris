using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject[] tetriminoPrefabs; // Mảng chứa các khối Tetrimino
    public Transform spawnPoint;         // Vị trí sinh khối mới
    

    private GameObject KhoiHienTai, KhoiTiepTheo; // Tetrimino đang được kiểm soát
    private bool BatDauChoi = false;
    private Vector2 HinhTiepTheo = new Vector2(16.3f, 16f);

    public static GameManager instance;  // Singleton để dễ truy cập
    

    public TMP_Text scoreText;
    public TMP_Text LinesText;
    public TMP_Text LevelText;
    private int score;
    private int lines;
        
    public float fallSpeed = 1.0f;
    public int level;
    int newLevel;
    private int LinelevelUp= 2; // so dong can de tang cap 
    private bool isGameOver = false;
    //audio 
    public AudioClip gameOverSound;
    public AudioClip gameLevelUpSound;
    public ParticleSystem explosionEffect;
    void Awake()
    {
        // Singleton setup
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        FindObjectOfType<SoundManager>().PlayBackgroundMusic();
        level = 0;
        lines = 0;
        score = 0;
        
        SpawnNewTetrimino();
    }

    void Update()
    {
        // Kiểm tra game over
        if (!isGameOver && CheckGameOver())
        {
            
            isGameOver = true;
            //FindObjectOfType<SoundManager>().audioSource.Stop();
            //FindObjectOfType<SoundManager>().PlaySoundForDuration(gameOverSound, 3.0f);
            SceneManager.LoadScene("GameOver");
            
            Time.timeScale = 0; // Dừng thời gian (tạm dừng game)
           


        }
    }
    public void TriggerExplosion(Vector2 position)
    {
       
       ParticleSystem explosion = Instantiate(explosionEffect, position, Quaternion.identity);

       
        Destroy(explosion.gameObject, explosion.main.duration); 
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = ": " + score;
    }
    public void AddLines(int points)
    {
        lines += points;
        LinesText.text= ": "+lines;


        newLevel = lines / LinelevelUp;
        if (newLevel > level)
        {
            LevelUp(newLevel);
        }
    }
    private void LevelUp(int newLevel)
    {
        level = newLevel;
        fallSpeed = 1.0f - level * 0.2f; 
        fallSpeed = Mathf.Max(fallSpeed, 0.1f);
        LevelText.text = ": " + level;
        FindObjectOfType<SoundManager>().PlaySoundForDuration(gameLevelUpSound, 3.0f);
    }
    // Sinh khối Tetrimino mới
    public void SpawnNewTetrimino()
    {
        if (tetriminoPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, tetriminoPrefabs.Length); 
            int khoithuhai = Random.Range(0, tetriminoPrefabs.Length); 

            if (!BatDauChoi)
            {
                BatDauChoi = true;
                KhoiHienTai = Instantiate(tetriminoPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

                KhoiTiepTheo = Instantiate(tetriminoPrefabs[khoithuhai], HinhTiepTheo, Quaternion.identity);
                KhoiTiepTheo.GetComponent<TetriminoController>().enabled = false;
            }
            else
            {
                // Kiểm tra nếu KhoiTiepTheo đã bị hủy trước khi sử dụng
                if (KhoiTiepTheo != null)
                {
                    KhoiTiepTheo.transform.localPosition = spawnPoint.position;
                    KhoiHienTai = KhoiTiepTheo;
                    KhoiTiepTheo.GetComponent<TetriminoController>().enabled = true;
                }
                else
                {
                    Debug.LogWarning("KhoiTiepTheo bị hủy trước khi sử dụng.");
                }

                KhoiTiepTheo = Instantiate(tetriminoPrefabs[randomIndex], HinhTiepTheo, Quaternion.identity);
                KhoiTiepTheo.GetComponent<TetriminoController>().enabled = false;
            }

            // Kiểm tra nếu KhoiHienTai còn tồn tại trước khi thao tác
            if (KhoiHienTai != null)
            {
                // Làm tròn vị trí của Tetrimino
                Vector3 roundedPosition = new Vector3(
                    Mathf.Round(KhoiHienTai.transform.position.x),
                    Mathf.Round(KhoiHienTai.transform.position.y),
                    KhoiHienTai.transform.position.z 
                );
                KhoiHienTai.transform.position = roundedPosition;
            }
            else
            {
                Debug.LogWarning("KhoiHienTai bị hủy trước khi sử dụng.");
            }
        }
        else
        {
            Debug.LogWarning("Chưa gán Tetrimino prefabs.");
        }
    }




    // Kiểm tra game over (nếu một khối không thể xuất hiện)
    private bool CheckGameOver()
    {
        
        foreach (Transform child in GridManager.grid)
        {
            if (child != null && child.position.y >= GridManager.height-1 )
            {

                return true; // Phát hiện khối ở đỉnh lưới, game over
            }
        }
        return false;
    }

    // Gọi khi một khối dừng lại (khi va chạm với đáy hoặc các khối khác)
    public void OnTetriminoLanded()
    {
        // Dọn sạch hàng đầy và di chuyển hàng xuống
        FindObjectOfType<GridManager>().clearFullRow();

        // Sinh khối mới
        SpawnNewTetrimino();
    }
}
