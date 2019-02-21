﻿using System.Collections;
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
  public Farming farmingD;
  public Fishing fishingD;
  public Foresting forestD;

    // Start is called before the first frame update
    void Start() {
      ui = canvas.GetComponent<UIManager>();
      farmingD.InitializeDiscipline();
      fishingD.InitializeDiscipline();
      forestD.InitializeDiscipline();
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
      ui.SetForesterTextAmt(forestD.workers.Count);
      ui.SetFarmerTextAmt(farmingD.workers.Count);
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

    public Discipline GetDisciplineFromIndex(int discNum) {
      switch(discNum) {
        case 0:
          return farmingD;
        case 1:
          return fishingD;
        case 2:
          return forestD;
        default:
          return farmingD;
      }
    }

    public void TryIncreaseDisciplineWorkers(int discNum) {
      if(forestD.workers.Count > 0) {
        Villager vill = forestD.workers[0];
        forestD.RemoveWorker(vill);
        GetDisciplineFromIndex(discNum).AddWorker(vill);
        SetUI();
      }
    }

    public void TryDecreaseDisciplineWorkers(int discNum) {
      if(GetDisciplineFromIndex(discNum).workers.Count > 0) {
        Villager vill = GetDisciplineFromIndex(discNum).workers[0];
        GetDisciplineFromIndex(discNum).RemoveWorker(vill);
        forestD.AddWorker(vill);
        SetUI();
      }

    }

}
