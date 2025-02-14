using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TetriminoController : MonoBehaviour
{
   
    private float previousTime;   // Lưu thời gian khối rơi lần cuối để tính thời gian rơi

    void Start()
    {
        previousTime = Time.time;
    }

    void Update()
    {
   

        // Rơi tự động
        if (Time.time - previousTime >= GameManager.instance.fallSpeed)
        {
            if (!Move(Vector3.down)) // Nếu không thể rơi xuống thêm (chạm đáy hoặc khối khác)
            {
                // add block to grid
                AddToGrid();

                // check and destroy rows full
                FindObjectOfType<GridManager>().clearFullRow();

                // stop controlling this block
                enabled = false;

                // call new biomass function form gamemanager
                FindObjectOfType<GameManager>().OnTetriminoLanded();
            }

            previousTime = Time.time; // update time last time falling block
        }
    }


    //public void HandleMovement()
    //{
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        MoveLeft();

    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //        MoveRight();

    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //        Rotate();

    //    if (Input.GetKey(KeyCode.DownArrow))
    //        MoveDown();
    //}

    public void MoveLeft()
    {
        
        transform.position += new Vector3(-1, 0, 0);
        if (!IsValidPosition())
        {
            transform.position += new Vector3(1, 0, 0); // Quay lại nếu không hợp lệ
        }
    }

    public void MoveRight()
    {
        transform.position += new Vector3(1, 0, 0);
        if (!IsValidPosition())
        {
            transform.position += new Vector3(-1, 0, 0);
        }
    }

    public void Rotate()
    {
        if (transform.position.y >= GridManager.height - 1)
        {
            return;
        }
        transform.Rotate(0, 0, 90); 

        if (!Move(Vector3.down))
        {
            transform.Rotate(0, 0, -90); 
        }
    }
    public void MoveDown()
    {
        
            if (Time.time - previousTime >= GameManager.instance.fallSpeed / 10)
           {
               Move(new Vector3(0, -1, 0)); 
               previousTime = Time.time; 
            }
        
    }
    public void test()
    {
        Debug.Log("anhthube");
    }

    public bool Move(Vector3 direction)
    {
        // Thay đổi vị trí
        transform.position += direction;

        // Kiểm tra nếu vị trí không hợp lệ
        if (!IsValidPosition())
        {
            // Hoàn tác di chuyển nếu không hợp lệ
            transform.position -= direction;
            return false;
        }

        return true;
    }


    private bool IsValidPosition() // kiem tra trog luoi 
    {
        foreach (Transform child in transform)
        {
            
            // Làm tròn vị trí của từng khối nhỏ
            Vector2 position = GridManager.Round(child.position);

            // Kiểm tra nếu vị trí ngoài lưới
            if (!GridManager.IsinsideGird(position))
            {
                return false;
            }

            // Kiểm tra nếu có khối khác đã tồn tại ở vị trí này
            if (GridManager.grid[(int)position.x, (int)position.y] != null)
            {
                GameManager.instance.gameOver();
                return false;
            }
        }

        return true;
    }

    private void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            // Làm tròn vị trí của từng khối nhỏ
            Vector2 position = GridManager.Round(child.position);

            if (position.y >= GridManager.height)
            {
                GameManager.instance.gameOver();
                Debug.LogError("Khối nằm ngoài lưới! Game Over");
                return;
            }
            // Kiểm tra xem vị trí có hợp lệ trong phạm vi của grid không
            if (position.x >= 0 && position.x < GridManager.width && position.y >= 0 && position.y < GridManager.height)//
            {
                GridManager.grid[(int)position.x, (int)position.y] = child;
            }
            else
            {
                Debug.LogError("Vị trí ngoài phạm vi grid: " + position);
            }
        }
    }

}
