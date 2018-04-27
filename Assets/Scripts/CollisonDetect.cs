using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisonDetect : MonoBehaviour {

  public GameObject _player;

  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.Equals(_player))
    {
      SceneManager.LoadScene(1);
    }
  }
}
