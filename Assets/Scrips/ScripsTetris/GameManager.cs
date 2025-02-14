using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject[] tetriminoPrefabs; // Mảng chứa các khối Tetrimino
    public Transform spawnPoint;         // Vị trí sinh khối mới


    private GameObject currenBlock, nextBlock; // Tetrimino đang được kiểm soát
    private bool startPlay = false;
    private Vector2 HinhTiepTheo = new Vector2(22.41f, 14.96f);
    private Vector2 HoldPosition = new Vector2(-8.92f, 15.3f);
    private bool hasUseHold = false;
    public static GameManager instance;  // Singleton để dễ truy cập


    public TMP_Text scoreText;
    public TMP_Text LinesText;
    public TMP_Text LevelText;
    private int score;
    private int lines;
    public float fallSpeed = 1f;
    private float previousTime = 0f;
    private float fastFallSpeed = 0.1f; // Tốc độ rơi nhanh khi giữ button
    private float defaultFallSpeed = 1f; // Tốc độ rơi bình thường

    private bool isFastFalling = false; // Trạng thái giữ nút

    public int level;
    int newLevel;
    private int LinelevelUp = 2; // condition levelup
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
        level = GameSetting.SelectedLevel;
        lines = 0;
        score = 0;
        LevelUp(level);
        UpdateUI();
        SpawnNewTetrimino();
    }

    void Update()
    {
        gameOver();
        pressbutton();
    }
    public void gameOver()
    {
        if (!isGameOver && CheckGameOver())
        {

            isGameOver = true;
            //FindObjectOfType<SoundManager>().audioSource.Stop();
            //FindObjectOfType<SoundManager>().PlaySoundForDuration(gameOverSound, 3.0f);
            SceneManager.LoadScene("GameOver");

            //Time.timeScale = 0; // Dừng thời gian (tạm dừng game)



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
        LinesText.text = ": " + lines;


        newLevel = lines / LinelevelUp;
        if (newLevel > level)
        {
            LevelUp(newLevel);
        }
    }
    public void UpdateUI()
    {
        scoreText.text = ": " + score;
        LinesText.text = ": " + lines;
        LevelText.text = ": " + level;
    }
    //private void LevelUp(int newLevel)
    //{
    //    level = newLevel;
    //    fallSpeed = 1.0f - level * 0.2f;
    //    fallSpeed = Mathf.Max(fallSpeed, 0.1f);
    //    LevelText.text = ": " + level;
    //    FindObjectOfType<SoundManager>().PlaySoundForDuration(gameLevelUpSound, 3.0f);
    //}
    private void LevelUp(int newLevel)
    {
        level = newLevel;

        // Tính toán tốc độ rơi mới
        defaultFallSpeed = 1.0f - level * 0.2f;
        defaultFallSpeed = Mathf.Max(defaultFallSpeed, 0.1f); // Đảm bảo tốc độ không nhỏ hơn 0.1f

        // Cập nhật tốc độ rơi hiện tại
        fallSpeed = defaultFallSpeed;

        // Cập nhật UI
        LevelText.text = ": " + level;

        // Phát âm thanh khi lên cấp
        FindObjectOfType<SoundManager>().PlaySoundForDuration(gameLevelUpSound, 3.0f);
    }

    public void holdTetrimino()
    {
        if (currenBlock == null || isGameOver) return;

        if (!hasUseHold)
        {
            hasUseHold = true;
            if(HoldPosition == null)
            {

            }
        }




    } 
    // Sinh khối Tetrimino mới
    public void SpawnNewTetrimino()
    {
        if (isGameOver) return;
        if (tetriminoPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, tetriminoPrefabs.Length);
            int randomNext = Random.Range(0, tetriminoPrefabs.Length);

            if (!startPlay)
            {
                startPlay = true;
                currenBlock = Instantiate(tetriminoPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
                nextBlock = Instantiate(tetriminoPrefabs[randomNext], HinhTiepTheo, Quaternion.identity);
                nextBlock.GetComponent<TetriminoController>().enabled = false;
            }
            else
            {
                if (nextBlock != null)
                {
                    nextBlock.transform.localPosition = spawnPoint.position;
                    currenBlock = nextBlock;
                    nextBlock.GetComponent<TetriminoController>().enabled = true;
                }
                else
                {
                    Debug.LogWarning("next block destroy before use.");
                }

                nextBlock = Instantiate(tetriminoPrefabs[randomIndex], HinhTiepTheo, Quaternion.identity);
                nextBlock.GetComponent<TetriminoController>().enabled = false;
            }

            if (currenBlock != null)
            {
                // Làm tròn vị trí Tetrimino
                Vector3 roundedPosition = new Vector3(
                    Mathf.Round(currenBlock.transform.position.x),
                    Mathf.Round(currenBlock.transform.position.y),
                    currenBlock.transform.position.z
                );
                currenBlock.transform.position = roundedPosition;

                // Kiểm tra tất cả các ô của khối hiện tại
                //Nếu bất kỳ ô nào của khối mới sinh ra bị chiếm, game over ngay
                foreach (Transform child in currenBlock.transform)
                {
                    Vector2 pos = GridManager.Round(child.position);
                    //Kiểm tra xem ô đã bị chiếm chưa
                    if (!GridManager.IsinsideGird(pos) || GridManager.grid[(int)pos.x, (int)pos.y] != null)

                    {
                        Debug.Log("Game Over - Block overlapped");
                        SceneManager.LoadScene("GameOver");
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("current block destroyed before use.");
            }
        }
        else
        {
            Debug.LogWarning("not attached Tetrimino prefabs.");
        }
    }
    public void MoveLeft()
    {
        if (currenBlock != null)
        {
            currenBlock.GetComponent<TetriminoController>().MoveLeft();
        }
    }

    public void MoveRight()
    {
        if (currenBlock != null)
        {
            currenBlock.GetComponent<TetriminoController>().MoveRight();
        }
    }

    public void Rotate()
    {
        if (currenBlock != null)
        {
            currenBlock.GetComponent<TetriminoController>().Rotate();
        }
    }

    public void MoveDown()
    {
        if (currenBlock != null)
        {
            currenBlock.GetComponent<TetriminoController>().MoveDown();
        }
    }

    public void pressbutton()
    {
        // Kiểm tra xem có đang rơi nhanh không (nếu đang giữ button hoặc đang nhấn)
        if (isFastFalling)
        {
            fallSpeed = fastFallSpeed; // Tăng tốc độ rơi khi giữ button
        }
        else
        {
            fallSpeed = defaultFallSpeed; // Quay lại tốc độ rơi bình thường
        }

        // Kiểm tra thời gian để thực hiện hành động rơi
        if (Time.time - previousTime >= fallSpeed)
        {
            // Di chuyển vật thể xuống
            //transform.position += new Vector3(1, 0, 0);
            //if (!FindObjectOfType<TetriminoController>().IsValidPosition())
            //{
            //    transform.position += new Vector3(-1, 0, 0);
            //}
            FindObjectOfType<TetriminoController>().Move(new Vector3(0, -1, 0));
            // Cập nhật thời gian cho lần kiểm tra tiếp theo
            previousTime = Time.time;
        }
    }

    // Sự kiện khi nhấn nút (OnPointerDown)
    //public void OnPointerDown1(PointerEventData eventData)
    //{
    //    isFastFalling = true; // Bắt đầu tăng tốc độ rơi khi nhấn vào nút
    //}

    // Sự kiện khi thả nút (OnPointerUp)
    public void teset()
    {
        isFastFalling = true;
    }
    public void teset1()
    {
        isFastFalling = false;
    }
    //public void OnPointerUp1(PointerEventData eventData)
    //{
    //    isFastFalling = false; // Dừng tăng tốc độ rơi khi thả nút
    //}
    private bool CheckGameOver()
    {
        // Kiểm tra nếu bất kỳ ô nào ở hàng trên cùng bị chiếm
        for (int x = 0; x < GridManager.width; x++)
        {
            if (GridManager.grid[x, GridManager.height - 1] != null)
            {
                Debug.Log("Game Over - Top row filled");
                return true;
            }
        }



        return false; // Không có điều kiện nào thỏa game over
    }


    public void OnTetriminoLanded()
    {


        if (CheckGameOver())
        {
            Debug.Log("Game Over after landing");
            gameOver();

            return;
        }


        SpawnNewTetrimino();
    }

}
