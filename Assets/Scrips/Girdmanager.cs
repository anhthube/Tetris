using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];
    

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    for (int x = 0; x <= width; x++)
    //    {
    //        for (int y = 0; y <= height; y++)
    //        {
    //            Vẽ ô lưới
    //            Gizmos.DrawWireCube(new Vector3(x, y, 0), Vector3.one);
    //        }
    //    }
    //    Vẽ gốc của lưới(0,0)
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(Vector3.zero, 0.2f);
    //}




   

    public static Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y)); // lam tron x, y

    }
    public static bool IsinsideGird(Vector2 position)
    {
        return (position.x >= 0 && position.x < width && position.y >= 0 && position.y <= height); // kiem tra co nam trong luoi khong
    }
    // kiem tra hang day
    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }
  //  IEnumerator
    private IEnumerator ClearRowWithDelay(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                
                 FindObjectOfType<GameManager>().TriggerExplosion(grid[x, y].transform.position);

                if (grid[x, y] != null)
                {
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                }

               
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
   
    public void clearFullRow()
    {
        StartCoroutine(ClearAndShiftRows());
    }

    private IEnumerator ClearAndShiftRows()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsRowFull(y))
            {
                // Xóa hàng đầy
                yield return StartCoroutine(ClearRowWithDelay(y));

                // Dời các hàng phía trên xuống
                yield return StartCoroutine(ShiftRowsAfterDelay(y, 0.05f));
                GameManager.instance.AddScore(100);
                GameManager.instance.AddLines(1);
                // Sau khi dời, kiểm tra lại hàng vừa dời xuống
                y--;
            }
        }
    }


    private IEnumerator ShiftRowsAfterDelay(int startingRow, float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = startingRow; i < height - 1; i++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, i + 1] != null && grid[x, i + 1].gameObject != null)
                {
                    grid[x, i] = grid[x, i + 1];
                    grid[x, i + 1] = null;
                    grid[x, i].position += new Vector3(0, -1, 0);
                }
            }
        }

    }
   

}
