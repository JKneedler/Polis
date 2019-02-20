using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Discipline {
  public bool unlocked; //Whether you can assign a villager to this discipline yet.
  public List<Villager> workers;
  public List<Structure> structures;
  public List<Process> processes;
  public List<Tile> assignedTiles;
  public Tile.TileTypes assignableTileType;
  public bool assignedTilesCanBuild;


  public virtual Task GetNextTask() {
    return new Task();
  }

  public virtual bool CanAssignTile(Tile tile) {
    return ((assignedTilesCanBuild || tile.GetCanBuild()) && tile.tileTypeEnum == assignableTileType);
  }

}
