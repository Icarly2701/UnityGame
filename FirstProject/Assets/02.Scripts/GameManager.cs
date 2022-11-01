using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public int totalScore;
    public int stagePoint;
    public int stageIndex;
    public int starPoint;
    public int health;
    public PlayerMove player;

    public Image[] UIHealth;
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    
   
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(manager == null)
            manager = this;
        
    }

    void Update()
    {
        UIPoint.text = "POINT   " + stagePoint.ToString();
        UIStage.text = "STAGE " + stageIndex.ToString();
    }
    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIHealth[health].color = new Color(0, 0, 0, 0.4f);
        }
        else
        {
            player.OnDie();
        }
    }

    public void HealthUp()
    {
        if (health < 5)
        {
            UIHealth[health].color = new Color(1, 1, 1, 1f);
            health++;
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           HealthDown();

            if (health > 0)
            {
                player.transform.position = new Vector3(-8.5f, -1.5f, 0);
            }else if(health == 0)
            {
                player.OnDie();
            }
        }
    }

    public void reStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void DieManager()
    {
        Destroy(gameObject);
    }
}
