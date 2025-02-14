using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Vector3 offset = new Vector3(0, 10, 0); // Camera ở trên đầu player

    private void LateUpdate()
    {
        if (player != null)
        {
            // Camera theo player, giữ offset cố định
            transform.position = player.position + offset;
        }
    }
}
