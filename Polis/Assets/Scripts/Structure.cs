﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {

  public Vector2 objScale;
  public Villager.Jobs structureJob;
  public int maxWorkers;
  public List<Villager> workers;
  public List<Tile> tilesOn;
  public List<Tile> tilesInRange;
  public int tileRange;
  private TownManager tm;
  private Map map;

  public enum BuildingTypes {Foresting, Mining, Farming, Livestock, Fishing};
  public BuildingTypes buildingType;
  public ForesterData foresterData;
  public FarmData farmData;

  //Add location for the structure to villagers know where to stay around

    // Start is called before the first frame update
    void Start() {
	    workers = new List<Villager>();
      tm = GameObject.FindWithTag("GameController").GetComponent<TownManager>();
      map = GameObject.FindWithTag("Map").GetComponent<Map>();
      tilesInRange = new List<Tile>();
      InitializeBuildingTypes();
    }

    // Update is called once per frame
    void Update() {

    }

    public void InitializeBuildingTypes() {
      switch(buildingType) {
        case BuildingTypes.Foresting:
          break;
        case BuildingTypes.Mining:
          break;
        case BuildingTypes.Farming:
          break;
        case BuildingTypes.Livestock:
          break;
        case BuildingTypes.Fishing:
          break;
      }
    }

    public void Place(List<Tile> tilesOn) {
      this.tilesOn = tilesOn;
      GetTilesInRange();
      switch(buildingType) {
        case BuildingTypes.Foresting:
          break;
        case BuildingTypes.Mining:
          break;
        case BuildingTypes.Farming:
          break;
        case BuildingTypes.Livestock:
          break;
        case BuildingTypes.Fishing:
          break;
      }
    }

    public void AssignVillager(Villager vill) {
	    workers.Add(vill);
      vill.ChangedAssignment(this, structureJob);
    }

    public void UnassignVillager(Villager vill) {
	    workers.Remove(vill);
    }

    public bool CanAssignVillager() {
      return !(workers.Count == maxWorkers);
    }

    public void GetTilesInRange() {
      for(int i = 1; i < tileRange + 1; i++) {
        for(int x = -i; x < i + 1; x++) {
          Tile tilePlusY = map.GetTileFromWorldPos((int)transform.position.x + x, (int)transform.position.z + (tileRange - Mathf.Abs(x)));
          Tile tileMinusY = map.GetTileFromWorldPos((int)transform.position.x + x, (int)transform.position.z - (tileRange - Mathf.Abs(x)));
          tilesInRange.Add(tilePlusY);
          if(x != -i && x != i) {
            tilesInRange.Add(tileMinusY);
          }
        }
      }
    }

}
