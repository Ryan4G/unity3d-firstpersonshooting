using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[AddComponentMenu("Game/GameManager")]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int m_score = 0;

    public int m_best = 0;

    public int m_ammo = 100;

    Player m_player;

    Text text_ammo;

    Text text_best;

    Text text_life;

    Text text_score;

    Button button_restart;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GameObject uicanvas = GameObject.Find("Canvas");

        foreach(Transform t in uicanvas.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name.CompareTo("Text_ammo") == 0)
            {
                text_ammo = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("Text_life") == 0)
            {
                text_life = t.GetComponent<Text>();

                text_life.text = $"{m_player.m_life}";
            }
            else if (t.name.CompareTo("Text_score") == 0)
            {
                text_score = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("Text_best") == 0)
            {
                text_best = t.GetComponent<Text>();
                text_best.text = $"Best Score {m_best}";
            }
            else if (t.name.CompareTo("Button") == 0)
            {
                button_restart = t.GetComponent<Button>();
                button_restart.onClick.AddListener(delegate() {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });

                button_restart.gameObject.SetActive(false);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score)
    { 
        m_score += score;

        if (m_score > m_best)
        {
            m_best = m_score;
        }

        text_score.text = $"Score <color=yellow>{m_score}</color>";
        text_best.text = $"Best Score {m_best}";
    }

    public void SetAmmo(int ammo)
    {
        m_ammo -= ammo;
        
        if (m_ammo <= 0)
        {
            m_ammo = 100;
        }

        text_ammo.text = $"{m_ammo}/100";
    }

    public void SetLife(int life)
    {
        text_life.text = $"{life}";

        if (life <= 0)
        {
            button_restart.gameObject.SetActive(true);
        }
    }
}
