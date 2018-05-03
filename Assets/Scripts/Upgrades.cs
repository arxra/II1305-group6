using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

//Upgrade behaviour
public class Upgrades : MonoBehaviour {

  [Tooltip("Food currency. Public ONLY for testing purposes.")]
    public int _currency;
  public GameOverScreen _gos;
  public Score _sc;
  List<Upgrade> _ownedUpgrades = new List<Upgrade>();


  //Upgrades
  private static string[] _ups = {
    "multiplier",
    "maximum_speed",
    "boost_speed",
    "multiplier_speed",
    "more_valuable_foods"
  };


  public void Start() {
    foreach (string up in _ups){
      _ownedUpgrades.Add(new Upgrade(up));
    }
  }


  //How much monies?
  public int CurrentCurrency() {
    return _currency;
  }


  //make it rain! (Check if money can be spent)
  public bool SpendMoney(int toBeSpent) {
    if(toBeSpent <= _currency) {
      _currency -= toBeSpent;
      return true;
    }
    return false;
  }


  public List<Upgrade> CurrentUpgrades() {
    return _ownedUpgrades;
  }


  public void Update() {
    //IF kill, then add current runs currency collection to the main currency
    if (! _gos.GetComponent<GameOverScreen>().alive)
      _currency += _sc.GetComponent<Score>().RunsFood();
      PlayerPrefs.SetInt("currency", (_currency + PlayerPrefs.GetInt("currency")));
  }


  public class Upgrade {
    public Upgrade(string na){
      name = na;
    }

    private static string name;
    private static int level = PlayerPrefs.GetInt(name);

    //Increment the cost of stuff exponatially
    int cost = Mathf.RoundToInt(Mathf.Pow(100, (1 + 0.2f * level)));
  }
}
