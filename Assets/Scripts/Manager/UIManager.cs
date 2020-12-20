using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject healthBar;
    public GameObject gameOver;

    [Header("UI Elements")]
    public GameObject pauseMenu;
    public Slider bossHealthBar;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // 仅保留一个UIManager
            Destroy(gameObject);
        }
    }

    public void UpdateHealth(float currentHealth)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }
        if (currentHealth > 60)
        {
            healthBar.transform.GetChild(0).gameObject.SetActive(true);
            healthBar.transform.GetChild(1).gameObject.SetActive(true);
            healthBar.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (currentHealth > 30 && currentHealth <= 60)
        {
            healthBar.transform.GetChild(0).gameObject.SetActive(true);
            healthBar.transform.GetChild(1).gameObject.SetActive(true);
            healthBar.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (currentHealth <= 30 && currentHealth > 1)
        {
            healthBar.transform.GetChild(0).gameObject.SetActive(true);
            healthBar.transform.GetChild(1).gameObject.SetActive(false);
            healthBar.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            healthBar.transform.GetChild(0).gameObject.SetActive(false);
            healthBar.transform.GetChild(1).gameObject.SetActive(false);
            healthBar.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        // 游戏暂停
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        // 游戏恢复
        Time.timeScale = 1;
    }

    // 设置boss血量
    public void SetBossHealth(float health)
    {
        bossHealthBar.maxValue = health;
    }

    public void UpdateBossHealth(float health)
    {
        bossHealthBar.value = health;
    }

    public void GameOverUI(bool playerDead)
    {

        gameOver.SetActive(playerDead);
        
    }
}
