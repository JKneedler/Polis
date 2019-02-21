using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Foresting : Discipline {

  public WorldDescriptor tempWD;

  public override void RemoveAssignedTile(Tile tile) {
    assignedTiles.Remove(tile);
    for(int i = 0; i < processes.Count; i++) {
      if(processes[i].tileOn == tile) {
        Process curPr = processes[i];
        processes.Remove(curPr);
        for(int j = 0; j < curPr.villagersWorking.Count; j++) {
          curPr.villagersWorking[j].curTask = null;
          curPr.villagersWorking[j].target = null;
          AttemptToGetNextTask(curPr.villagersWorking[j]);
        }
      }
    }
  }

  public override void AddAssignedTile(Tile tile) {
    assignedTiles.Add(tile);
    //Create new tasks
    Queue<Task> taskQ = new Queue<Task>();
    List<TreeResource> trees = tile.GetResourcesList();
    foreach (TreeResource tree in trees) {
      GameObject treeObj = tree.gameObject;
      Task chopTree = new Task(treeObj.GetComponent<WorldDescriptor>(), tempWD, 5f, false, true);
      taskQ.Enqueue(chopTree);
    }
    Process newTileProcess = new Process(taskQ);
    newTileProcess.tileOn = tile;
    processes.Add(newTileProcess);
    for(int i = 0; i < workers.Count; i++) {
      if(workers[i].hasTask == false) {
        AttemptToGetNextTask(workers[i]);
      }
    }
  }

  public override void AddWorker(Villager vill) {
    workers.Add(vill);
    vill.curJob = designatedJob;
    AttemptToGetNextTask(vill);
  }

  public override void RemoveWorker(Villager vill) {
    workers.Remove(vill);
  }

  public override void ReachedTaskTarget(Villager vill) {
    for(int i = 0; i < processes.Count; i++) {
      if(processes[i].villagersWorking.Contains(vill)) {
        Tile tile = processes[i].tileOn;
        TreeResource tr = vill.curTask.GetTargetWD().gameObject.GetComponent<TreeResource>();
        tile.RemoveResource(tr);
        tr.DestroyResource();
        tile.DecrementNumResources();
        if(tile.GetNumResources() == 0) {
          tile.SetTypeChar('G');
          tile.SetCanBuild(true);
          tile.SetMaterialBackToOriginal();
          assignedTiles.Remove(tile);
        }
      }
    }
  }

  public override void VillagerCompletedTask(Villager vill) {
    for(int i = 0; i < processes.Count; i++) {
      processes[i].villagersWorking.Remove(vill);
    }
    vill.hasTask = false;
    vill.curTask = null;
    vill.target = null;
    AttemptToGetNextTask(vill);
  }

  public override void AttemptToGetNextTask(Villager vill) {
    while(processes.Count > 0 && processes[0].villagersWorking.Count == 0 && processes[0].tasks.Count == 0) {
      processes.RemoveAt(0);
    }
    for(int i = 0; i < processes.Count; i++) {
      if(processes[i].tasks.Count > 0) {
        Task newTask = processes[i].tasks.Dequeue();
        processes[i].villagersWorking.Add(vill);
        vill.NewTask(newTask);
        i = processes.Count;
      }
    }
  }

}
