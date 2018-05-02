using UnityEngine;

//Upgrade behaviour
public class Upgrades : MonoBehaviour {

  [Tooltip("Food currency. Public ONLY for testing purposes.")]
    public int _currency;
  public GameOverScreen _gos;
  public Score _sc;


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

  public void Update() {
    //IF kill, then add current runs currency collection to the main currency
    if (! _gos.GetComponent<GameOverScreen>().alive){
      _currency += _sc.GetComponent<Score>().RunsFood();
      PlayerPrefs.SetInt("currency", (_currency + PlayerPrefs.GetInt("currency")));
    }
  }
}
