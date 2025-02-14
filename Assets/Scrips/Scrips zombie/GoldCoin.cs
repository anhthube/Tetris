using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    public int value = 1;
    private float collectDelay = 0.5f; // Thời gian tự động cộng vàng

    void Start()
    {
        StartCoroutine(AutoCollect());
    }

    IEnumerator AutoCollect()
    {
        yield return new WaitForSeconds(collectDelay);
        CollectGold();
    }

    void CollectGold()
    {
        playerControllerkillzombie.Instance.AddGold(value); // Cộng vàng vào người chơi
        Destroy(gameObject); // Hủy đối tượng vàng
    }
}
