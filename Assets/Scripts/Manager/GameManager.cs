using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public static GameManager instance;

    private PlayerController player;
    private Door doorExit;

    public List<Enemy> enemies = new List<Enemy>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //player = FindObjectOfType<PlayerController>();
        //doorExit = FindObjectOfType<Door>();
    }

    public void IsPlayer(PlayerController playerController)
    {
        player = playerController;
    }

    public void IsExitDoor(Door door)
    {
        doorExit = door;
    }


    private void Update()
    {
        if (player != null)
        {
            gameOver = player.isDead;
        }
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            UIManager.instance.GameOverUI(gameOver);
        }
    }

    public void BackToMainMenu()
    {
        // 回到主菜单
        SceneManager.LoadScene(0);
        // 游戏恢复
        Time.timeScale = 1;
    }

    // 重启场景
    public void RestartScene()
    {
        // 重启当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // 游戏恢复
        Time.timeScale = 1;
        PlayerPrefs.DeleteKey("playerHealth");
    }

    // 通往下一关
    public void NextLevel()
    {
        // 通往下一关
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // 开始新游戏
    public void NewGame()
    {
        // 清除所有存储数据
        PlayerPrefs.DeleteAll();
        // 新游戏
        SceneManager.LoadScene(1);

        // 游戏恢复
        Time.timeScale = 1;
    }

    // 加载游戏
    public void LoadGame()
    {
        // 加载场景
        if (PlayerPrefs.HasKey("sceneIndex"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("sceneIndex"));
        }
        else
        {
            // 没保存就直接新游戏
            NewGame();
        }
        // 游戏恢复
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // 死亡的敌人被移除列表
    public void EnemyDead(Enemy enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            doorExit.OpenDoor();
            SaveData();
        }
    }
    public void IsEnemy(Enemy enemy)
    {
        // 把所有敌人加到敌人列表中
        enemies.Add(enemy);
    }

    // 加载保存的血量
    public float LoadHealth()
    {
        // 如果存储数据中没有这个值
        if (!PlayerPrefs.HasKey("playerHealth"))
        {
            // 初始生命值100
            PlayerPrefs.SetFloat("playerHealth", 100.0f);
        }
        // 有存档，获得保存的血量
        float currentHealth = PlayerPrefs.GetFloat("playerHealth");

        return currentHealth;
    }

    public void SaveData()
    {
        // 保存当前血量
        PlayerPrefs.SetFloat("playerHealth", player.health);

        // 保存下一关
        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex + 1);

        // 在对应平台的对应位置来保存数据
        PlayerPrefs.Save();
    }


}
