using UnityEngine;

public class GameOverScreen : MonoBehaviour
{

  public GameOverManager go;
  private GameObject[] Obstical;
  GameObject[] pauseObjects;
  public bool alive = true;
  public bool godMode;
  Animator anim;
  private float immortalityTimer;
  public Upgrades ups;

  private WorldMover worldMover;

  void Start()
  {
    Time.timeScale = 1;
    pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
    alive = true;
    godMode = false;
    hidePause();
    worldMover = GameObject.Find("WorldMover").GetComponent<WorldMover>();
    immortalityTimer = 3.0f;

    foreach(Upgrades.Upgrade t in ups.CurrentUpgrades())
      if(t.IsMyName("godmode_timer"))
        immortalityTimer += t.Level();
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

      worldMover.IsKill = false;
    }
  }
  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag.Equals("Obstacle"))
    {
      if (!godMode) {
        alive = false;
        go.GetComponent<GameOverManager> ().GameOver ();
        worldMover.IsKill = true;
      } else {
        unlimitedPower((collision.transform.parent.gameObject));
      }
    }
  }

  public void unlimitedPower(GameObject toDestroy) {
    Destroy (toDestroy);
  }

  public void activateGodMode() {
    godMode = true;
    Invoke ("deactivateGodMode", immortalityTimer);
  }
  public void deactivateGodMode() {
    godMode = false;
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
