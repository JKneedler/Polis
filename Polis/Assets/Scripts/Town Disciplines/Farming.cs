using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Farming : Discipline {

  public int maxFarmingTiles;
  public GameObject[] farmTilePrefabs;
  public WorldDescriptor tempWD;

  public override void RemoveAssignedTile(Tile tile) {
    assignedTiles.Remove(tile);
  }

  public override void AddAssignedTile(Tile tile) {
    if(assignedTiles.Count < maxFarmingTiles) {
      assignedTiles.Add(tile);
    }

    Queue<Task> taskQ = new Queue<Task>();
    WorldDescriptor tileWD = tile.GetTileObject().GetComponent<WorldDescriptor>();
    Task taskTill = new Task(tileWD, tempWD, 5f, false, true);
    Task taskWater = new Task(tileWD, tempWD, 5f, true, true);
    Task taskHarvest = new Task(tileWD, tempWD, 5f, false, true);
    taskQ.Enqueue(taskTill);
    taskQ.Enqueue(taskWater);
    taskQ.Enqueue(taskHarvest);
    Process farmProcess = new Process(taskQ, true);
    processes.Add(farmProcess);
    for(int i = 0; i < workers.Count; i++) {
      if(!workers[i].hasTask) {
        AttemptToGetNextTask(workers[i]);
      }
    }
  }


}
