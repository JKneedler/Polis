using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Process {
  public Queue<Task> tasks;
  public List<Villager> villagersWorking;
  public Tile tileOn;
  public bool prevTaskMustComplete;

  public Process(Queue<Task> tasks, bool prevTaskMustComplete) {
    this.tasks = tasks;
    villagersWorking = new List<Villager>();
    this.prevTaskMustComplete = prevTaskMustComplete;
  }

}
