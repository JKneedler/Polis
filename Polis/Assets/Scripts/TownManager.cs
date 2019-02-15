using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour {

  public int foodTotal;
  public int woodTotal;
  public int stoneTotal;
  public int drachmaTotal;
  private UIManager ui;
  public GameObject canvas;
  public List<Villager> villagers;
  public List<Structure> structures;
  public List<Resource> resources;
  public Resource wood;
  public Resource stone;

    // Start is called before the first frame update
    void Start() {
      ui = canvas.GetComponent<UIManager>();
      SetUI();
    }

    // Update is called once per frame
    void Update() {

    }

    void SetUI() {
      ui.SetPopulation(villagers.Count);
      ui.SetFood(foodTotal);
      ui.SetWood(wood.GetAmount());
      ui.SetStone(stone.GetAmount());
    }

    void SetFoodTotal() {
      int newTotal = 0;
      for(int i = 0; i < resources.Count; i++) {
        if(resources[i].GetResourceType() == Resource.ResourceTypes.Food) {
          newTotal += resources[i].GetAmount();
        }
      }
      foodTotal = newTotal;
      SetUI();
    }

    public void BuiltStructure(Structure str) {
      structures.Add(str);
    }

    public void DestroyedStructure(Structure str) {
      structures.Remove(str);
    }

}
