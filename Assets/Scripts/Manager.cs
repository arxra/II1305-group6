using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour {

    public void changeScene(int Scene)
    {
        if (Scene == 0)
            Application.LoadLevel(Application.loadedLevel);
        else
            Application.LoadLevel(Scene);

    }

}
