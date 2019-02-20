using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {
  private Vector3 targetLocation;
  private float taskDuration;
  public bool repeat;
  // private Animation taskAnimation; -- Add when I have animations added in

  public Task() {}

  public Task(Vector3 targetLocation, float taskDuration, bool repeat) {
    this.targetLocation = targetLocation;
    this.taskDuration = taskDuration;
    this.repeat = repeat;
  }

  public Vector3 GetTargetLocation() {
    return targetLocation;
  }

  public float GetTaskDuration() {
    return taskDuration;
  }

  public bool GetRepeat() {
    return repeat;
  }

}
