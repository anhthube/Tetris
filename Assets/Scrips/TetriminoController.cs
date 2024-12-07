using System.Collections;
using System.Collections.Generic;
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
        // Kiểm tra thao tác từ người chơi
        HandleMovement();

        // Rơi tự động
        if (Time.time - previousTime >= GameManager.instance.fallSpeed)
        {
            if (!Move(Vector3.down)) // Nếu không thể rơi xuống thêm (chạm đáy hoặc khối khác)
            {
                // Gắn khối vào lưới
                AddToGrid();

                // Kiểm tra và xóa các hàng đầy
                FindObjectOfType<GridManager>().clearFullRow();

                // Ngừng điều khiển khối này
                enabled = false;

                // Gọi hàm sinh khối mới từ GameManager
                FindObjectOfType<GameManager>().SpawnNewTetrimino();
            }

            previousTime = Time.time; // Cập nhật thời gian lần cuối khối rơi
        }
    }

    private void HandleMovement()
    {
        // Di chuyển sang trái
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!IsValidPosition())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }

        // Di chuyển sang phải
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!IsValidPosition())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }

        // Xoay khối
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (transform.position.y >= GridManager.height - 1)
            {
                return;
            }
            transform.Rotate(0, 0, 90); // Xoay 90 độ theo trục Z

            if (!Move(Vector3.down))
            {
                transform.Rotate(0, 0, -90); // Hoàn tác xoay nếu không hợp lệ
            }
        }

        // Rơi nhanh xuống khi nhấn mũi tên xuống
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(Vector3.down);
        }
    }

    private bool Move(Vector3 direction)
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

            // Kiểm tra xem vị trí có hợp lệ trong phạm vi của grid không
            if (position.x >= 0 && position.x < GridManager.width && position.y >= 0 && position.y < GridManager.height)
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
