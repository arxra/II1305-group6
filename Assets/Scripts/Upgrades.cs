using UnityEngine;
using System.Collections.Generic;

//Upgrade behaviour
public class Upgrades : MonoBehaviour {

  [Tooltip("Food currency. Public ONLY for testing purposes.")]
    public int _currency;
  public GameOverScreen _gos;
  public Score _sc;
  public List<Upgrade> _ownedUpgrades = new List<Upgrade>();


  //Upgrades
  private static string[] _ups = {
    "multiplier",
    "maximum_speed",
    "boost_speed",
    "multiplier_speed",
    "more_valuable_foods",
    "godmode_timer"
  };


  public void Start() {
    foreach (string up in _ups){
      _ownedUpgrades.Add(new Upgrade(up, this));
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
    if (_gos != null && ! _gos.GetComponent<GameOverScreen>().alive)

      _currency += _sc.GetComponent<Score>().RunsFood();
      PlayerPrefs.SetInt("currency", (_currency + PlayerPrefs.GetInt("currency")));
      PlayerPrefs.Save();
  }


  public class Upgrade {
    public Upgrade(string na, Upgrades parent){
      _name = na;
      if(PlayerPrefs.HasKey(na))
      _level = PlayerPrefs.GetInt(na);
      else
        PlayerPrefs.SetInt(na, 1);
    //Increment the cost of stuff exponatially
      costInc();
      _parent = parent;
    }
    private void costInc(){
      _cost = Mathf.RoundToInt(Mathf.Pow(100, (0.2f * _level + 1)));
    }

    private string _name;
    private int _level;
    private int _cost;
    private Upgrades _parent;


    public string Name () {return _name; }
    public int Cost ()    {return _cost; }
    public int Level()    {return _level;}

    public bool Imporve (){
      if (_parent.SpendMoney(_cost)){
        _level ++;
        PlayerPrefs.SetInt(_name, _level);
        PlayerPrefs.Save();
        costInc();
        return true;
      }
      //Imporvement failed
      return false;
    }

    public bool IsMyName(string pourque){
      return (_name.Equals(pourque));
    }
    //end of class Upgrade
  }
}
