using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject UiHero;
    [SerializeField] private GameObject UiShopiteam;
    [SerializeField] private GameObject UiSetting;
    public TextMeshProUGUI StartText;
    
    public void Start()
    {
        CloseAllPanels();

    }
    public void start()
    {
        UpdateStatsUI();
    }
    public void OnUpgradeButtonClicked()
    {
        
       
        playerControllerkillzombie.Instance.upgrade();
        UpdateStatsUI();
    }
   public void UpdateStatsUI()
    {
        StartText.text =
                         $"fireRate: {playerControllerkillzombie.Instance.getfireRate()}\n" +
                         $"bulletSpeed: {playerControllerkillzombie.Instance.getbulletSpeed()}\n" +
                         $"bulletDamage: {playerControllerkillzombie.Instance.getbulletDamage()} ";
    }



    public void btnHero()
    {
        UiHero.SetActive(true);
        Time.timeScale = 0f;
    }
    public void OpenUiShopiteam()
    {
        UiShopiteam.SetActive(true);
        Time.timeScale = 0f;
    }
    public void OpenUiSetting()
    {
        UiSetting.SetActive(true);
        Time.timeScale = 0f;
    }

    public void btnClone()
    {
        UiHero.SetActive(false);
        Time.timeScale = 1f;
    }
    private void CloseAllPanels()
    {
        UiHero.SetActive(false);
        UiShopiteam.SetActive(false);
        UiSetting.SetActive(false);
    }
}
