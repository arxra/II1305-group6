using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

  [Tooltip("The current players score")]
    public int score;
  public Text text;
  public GameOverScreen go;
  public bool alive;
  // Use this for initialization
  void Start () {
    alive = true;
    score = 0;
      updateText();
  }

  // Update is called once per frame
  void Update () {
    alive = go.GetComponent<GameOverScreen>().alive;
    int multiplier = 1; // Change this one later to get it's variable from the multiplier
    float scoreUpdate = 0;
    if(Time.timeScale == 1)
      scoreUpdate += Time.deltaTime * GameObject.Find("WorldMover").GetComponent<WorldMover>().GetCurrentSpeed();

    score += Mathf.RoundToInt(scoreUpdate * multiplier);
    updateText();
  }

  void updateText()
  {
    text.text = "Score : "+ score ;
      if (alive.Equals(false))
      {
        text.rectTransform.anchoredPosition = new Vector2(0f, 30f);
      }
  }

  void OnCollisionEnter(Collision col){
    if(col.gameObject.tag == "Pick Up"){
      GameObject pckup = col.gameObject;
      score += pckup.GetComponent<Collectables>().value;
      Destroy(pckup);
    }
  }
}
