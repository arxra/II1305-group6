using UnityEngine;

public class buying : MonoBehaviour {
  public Upgrades ups;
  public void Buy (string name) {
    Debug.Log(name);
    foreach(Upgrades.Upgrade t in ups.CurrentUpgrades())
      if(t.IsMyName(name)){
        if(t.Imporve())
          Debug.Log(t.Name() + "was improved");     
        else
          Debug.Log("imporve failed");

      }
  }
}
