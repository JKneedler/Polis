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
  public Villager.Jobs designatedJob;


  public virtual Task GetNextTask() {
    return new Task();
  }

  public virtual bool CanAssignTile(Tile tile) {
    return ((assignedTilesCanBuild || tile.GetCanBuild()) && tile.tileTypeEnum == assignableTileType);
  }

  public void InitializeDiscipline() {
    assignedTiles = new List<Tile>();
    processes = new List<Process>();
  }

  public virtual void RemoveAssignedTile(Tile tile) {
    assignedTiles.Remove(tile);
  }

  public virtual void AddAssignedTile(Tile tile) {
    assignedTiles.Add(tile);
  }

  public virtual void AddWorker(Villager vill) {
    workers.Add(vill);
    vill.curJob = designatedJob;
    AttemptToGetNextTask(vill);
  }

  public virtual void RemoveWorker(Villager vill) {
    workers.Remove(vill);
  }

  public virtual void ReachedTaskTarget(Villager vill) {}

  public virtual void VillagerCompletedTask(Villager vill) {
    Debug.Log("Completed Task");
  }

  public virtual void AttemptToGetNextTask(Villager vill) {

  }

}
