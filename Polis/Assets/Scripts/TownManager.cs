using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour {

  public int foodTotal;
  public int foodStorageMax;
  public int foodProductionRate;
  public int population;
  public int populationMax;
  public int woodTotal;
  public int woodProductionRate;
  public int stoneTotal;
  public int stoneProductionRate;
  private UIManager ui;
  public GameObject canvas;
  public List<Structure> structures;

    // Start is called before the first frame update
    void Start() {
      ui = canvas.GetComponent<UIManager>();
      SetUI();
      UpdateResourceRates();
      InvokeRepeating("ProduceResources", 1.0f, 2.0f);
    }

    // Update is called once per frame
    void Update() {

    }

    void SetUI() {
      ui.SetPopulation(population, populationMax);
      ui.SetFood(foodTotal, foodStorageMax);
      ui.SetWood(woodTotal);
      ui.SetStone(stoneTotal);
    }

    void ProduceResources() {
      if(foodTotal < foodStorageMax) {
        foodTotal += foodProductionRate;
        if(foodTotal > foodStorageMax) foodTotal = foodStorageMax;
      }

      woodTotal += woodProductionRate;
      stoneTotal += stoneProductionRate;
      SetUI();
    }

    public void UpdateResourceRates() {
      int foodRate = 0;
      int woodRate = 0;
      int stoneRate = 0;
      for(int i = 0; i < structures.Count; i++) {
        foodRate += structures[i].foodProdCur;
        woodRate += structures[i].woodProdCur;
        stoneRate += structures[i].stoneProdCur;
      }
      foodProductionRate = foodRate;
      woodProductionRate = woodRate;
      stoneProductionRate = stoneRate;
    }

    public void BuiltStructure(Structure str) {
      structures.Add(str);
      UpdateResourceRates();
    }

    public void DestroyedStructure(Structure str) {
      structures.Remove(str);
      UpdateResourceRates();
    }
}
