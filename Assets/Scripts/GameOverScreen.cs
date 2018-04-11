using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    
    GameObject[] gameoverObjects;
    private GameObject[] Obstical;
    GameObject[] pauseObjects;
    private bool alive;
    
    
    void Start()
    {
       
        Time.timeScale = 1;
        gameoverObjects = GameObject.FindGameObjectsWithTag("ShowOnGameOver");
        pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
        alive = true;
        hideGameOver();
        hidePause();

    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (Time.timeScale == 1 && alive == true)
            {
                
                Time.timeScale = 0;
                showPause();
            }
            else if (Time.timeScale == 0 && alive == true)
            {
                Time.timeScale = 1;
                hidePause();
            }

        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag=="Obstacle")
        {
            alive = false;
            Time.timeScale = 0;
            showGameOver();
        }
    }
   
    public void hideGameOver()
    {
        foreach (GameObject g in gameoverObjects)
            g.SetActive(false);
    }
    public void showGameOver()
    {
        foreach (GameObject g in gameoverObjects)
            g.SetActive(true);
    }
    public void hidePause()
    {
        foreach (GameObject g in pauseObjects)
            g.SetActive(false);
    }
    public void showPause()
    {
        foreach (GameObject g in pauseObjects)
            g.SetActive(true);
    }
}
