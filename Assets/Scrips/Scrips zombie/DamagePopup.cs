using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{

    private TextMeshPro textMesh;
    private float moveSpeed = 2f;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount)
    {
        if (textMesh == null)
        {
            textMesh =  GetComponent<TextMeshPro>(); 
        }
        // Thêm dấu trừ và đổi màu text
        Debug.Log($"Setting damage text: -{damageAmount}");
        textMesh.text = $"{damageAmount}"; // Dùng .text thay vì SetText
        textMesh.color = new Color(1, 0, 0, 1); // Red with full alpha
        textColor = textMesh.color;
        transform.localScale = Vector3.one; // Reset scale
        disappearTimer = 1f;
        moveVector = new Vector3(0.7f, 1f) * 3f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > 0.5f)
        {
            // First half of the popup lifetime
            transform.localScale += Vector3.one * 1f * Time.deltaTime;
        }
        else
        {
            // Second half of the popup lifetime
            transform.localScale -= Vector3.one * 1f * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= 3f * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
