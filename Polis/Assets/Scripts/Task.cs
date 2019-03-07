using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Task {
  private Tile target;
  private Tile returnTarget;
  private float taskDuration;
  public bool repeat;
  public bool contactDisciplineWhenReached;
  // private Animation taskAnimation; -- Add when I have animations added in

  public Task() {}

  public Task(Tile target, Tile returnTarget, float taskDuration, bool repeat, bool contactDisciplineWhenReached) {
    this.target = target;
    this.returnTarget = returnTarget;
    this.taskDuration = taskDuration;
    this.repeat = repeat;
    this.contactDisciplineWhenReached = contactDisciplineWhenReached;
  }

  public Tile GetTargetWD() {
    return target;
  }

  public Tile GetReturnTargetWD() {
    return returnTarget;
  }

  public float GetTaskDuration() {
    return taskDuration;
  }

  public bool GetRepeat() {
    return repeat;
  }

  public bool GetContactDisciplineWhenReached() {
    return contactDisciplineWhenReached;
  }

}
