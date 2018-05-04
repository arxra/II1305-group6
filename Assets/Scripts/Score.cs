using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Score : MonoBehaviour {

  [Tooltip("The current players score")]
<<<<<<< HEAD
    public int score;
      public Text highScore;
      public Text text;
=======
    public int score;

  [Tooltip("Insert upgrade module here")]
    public Upgrades _upgrades;
  public Animator anim;
  private bool TheBoolThatTellsIfTheAnimationForANewHighScoreHasBeenPlayed = false;
  public Text highScore;
  public Text text;
  public Text MultiText;
>>>>>>> 7ea580f... Basic Upgrades framework
  public GameOverScreen go;
  public bool alive;
  public float multiplier;
  private Dictionary<int, MulStruct> _multis = new Dictionary<int, MulStruct>();
  private WorldMover _mv;

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
<<<<<<< HEAD
  // Use this for initialization
=======

>>>>>>> 7ea580f... Basic Upgrades framework
  void Start () {
    _mv = GameObject.Find("WorldMover").GetComponent<WorldMover>();
    alive = true;
    score = 0;
    highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore");
    updateText();
  }

<<<<<<< HEAD
  // Update is called once per frame
  void Update () {
    alive = go.GetComponent<GameOverScreen>().alive;
    float scoreUpdate = 0;
    //Tick multiplier
=======

  void Update () {
    alive = go.GetComponent<GameOverScreen>().alive;
    float scoreUpdate = 0;

    //Tick multiplier  (Multiply by multipliers) 
>>>>>>> 7ea580f... Basic Upgrades framework
    multiplier = 1f;

    foreach(Upgrades.Upgrade id in _upgrades.CurrentUpgrades())
      if(id._name.Equals("multiplier")){ 
        multiplier += id._level;
      }


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
<<<<<<< HEAD
    int oldHighScore = PlayerPrefs.GetInt("highScore");
      if (score > oldHighScore)
        PlayerPrefs.SetInt("highScore", score);
          highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore");
          
          
          
          updateText();
=======

    //Update highscore
    int oldHighScore = PlayerPrefs.GetInt("highScore");
      if (score > oldHighScore)
      {
        PlayerPrefs.SetInt("highScore", score);
          if (!TheBoolThatTellsIfTheAnimationForANewHighScoreHasBeenPlayed)
          {
            anim.SetTrigger("high");
              TheBoolThatTellsIfTheAnimationForANewHighScoreHasBeenPlayed = true; 
          }
      }
    highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore");
      updateText();
>>>>>>> 7ea580f... Basic Upgrades framework
  }

  void updateText()
  {
<<<<<<< HEAD
=======
    MultiText.text = "X" + multiplier;
>>>>>>> 7ea580f... Basic Upgrades framework
    text.text = "Score : "+ score ;
  }

  public float SizeMultiplier(){
    float tmp = _foodFactor;
    _foodFactor = 0f;
    return tmp;
  }

<<<<<<< HEAD
=======
  //Use only when game's lost
  public int RunsFood() {
    int tmep = _totalFoodForRun;
    _totalFoodForRun = 0;
    return tmep;
  }

  //Pickup collectable, add value and destroy when done
>>>>>>> 7ea580f... Basic Upgrades framework
  void OnTriggerEnter(Collider col){
    if(ObjectFilter.EntityHasTags(col.gameObject ,ObjectFilter.Tag.Collectable)){
      GameObject pckup = col.gameObject;
      score += pckup.GetComponent<Collectables>().value;
      _multis.Add(Time.frameCount, new MulStruct(pckup.GetComponent<Collectables>()._time, pckup.GetComponent<Collectables>()._mult, Time.frameCount));
      _foodFactor += pckup.GetComponent<Collectables>()._sizeMultiplier;
      Destroy(pckup);
    }
  }
}
