using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    GameObject[] gameoverObjects;
    private GameObject[] Obstical;
    
    
    void Start()
    {
       
        Time.timeScale = 1;
        gameoverObjects = GameObject.FindGameObjectsWithTag("ShowOnGameOver");
        hideGameOver();
    }
   
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag=="Obstacle")
        {
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
}
