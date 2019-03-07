using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Farming : Discipline {

  public int maxFarmingTiles;
  public GameObject[] farmTilePrefabs;

  public override void RemoveAssignedTile(Tile tile) {
    assignedTiles.Remove(tile);
  }

  public override void AddAssignedTile(Tile tile) {
    if(assignedTiles.Count < maxFarmingTiles) {
      assignedTiles.Add(tile);
    }

    Queue<Task> taskQ = new Queue<Task>();
    Task taskTill = new Task(tile, tile, 5f, false, true);
    Task taskWater = new Task(tile, tile, 5f, true, true);
    Task taskHarvest = new Task(tile, tile, 5f, false, true);
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
