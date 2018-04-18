using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{

    public GameOverManager go;
    private GameObject[] Obstical;
    GameObject[] pauseObjects;
    public bool alive = true;
    Animator anim;

    
    void Start()
    {
        
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
        alive = true;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            alive = false;
            go.GetComponent<GameOverManager>().GameOver();
        }
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
