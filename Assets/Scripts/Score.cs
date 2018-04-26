using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Score : MonoBehaviour {

  [Tooltip("The current players score")]
    public int score;
      public Text highScore;
      public Text text;
  public GameOverScreen go;
  public bool alive;
  public float multiplier;
  private Dictionary<int, MulStruct> _multis = new Dictionary<int, MulStruct>();
  private WorldMover _mv = new WorldMover();

  private float _foodFactor;

  public class MulStruct {
    public MulStruct (float t, float m, int location){
      _time = t;
      _mult = m;
      _location = location;
    }
    public float _time;
    public float _mult;
    public int _location;
  }
  // Use this for initialization
  void Start () {
    _mv = GameObject.Find("WorldMover").GetComponent<WorldMover>();
    alive = true;
    score = 0;
    highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore");
      updateText();
  }

  // Update is called once per frame
  void Update () {
    alive = go.GetComponent<GameOverScreen>().alive;
    float scoreUpdate = 0;
    //Tick multiplier
    multiplier = 1f;
    foreach (MulStruct mul in new Dictionary<int, MulStruct>(_multis).Values) {
      mul._time -= Time.deltaTime;
      if(mul._time <=0) {
        _multis.Remove(mul._location);}
      else
        multiplier += mul._mult;
    }
    
      
      
      if (alive)
      {
        scoreUpdate += _mv.GetCurrentSpeed() * Time.deltaTime * multiplier;
          score += Mathf.RoundToInt(scoreUpdate);
      }
    int oldHighScore = PlayerPrefs.GetInt("highScore");
      if (score > oldHighScore)
        PlayerPrefs.SetInt("highScore", score);
          highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore");
          
          
          
          updateText();
  }

  void updateText()
  {
    text.text = "Score : "+ score ;
  }

  public float SizeMultiplier(){
    float tmp = _foodFactor;
    _foodFactor = 0f;
    return tmp;
  }

  void OnTriggerEnter(Collider col){
    if(ObjectFilter.EntityHasTags(col.gameObject ,ObjectFilter.Tag.Collectable)){
      GameObject pckup = col.gameObject;
      score += pckup.GetComponent<Collectables>().value;
      _multis.Add(_multis.Count, new MulStruct(pckup.GetComponent<Collectables>()._time, pckup.GetComponent<Collectables>()._mult, Time.frameCount));
      _foodFactor += pckup.GetComponent<Collectables>()._sizeMultiplier;
      Destroy(pckup);
    }
  }
}
