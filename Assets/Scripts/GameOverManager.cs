using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {
    public bool alive;
    Animator anim;
    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        alive = true;
    }

    // Update is called once per frame

    public void GameOver()
    {
       
        alive = false;
        anim.SetTrigger("GameOver");
            
     }
}
