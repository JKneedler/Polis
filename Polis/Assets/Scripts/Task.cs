using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Task {

  protected Queue<Target> targets;
  protected bool isNecessary;

  public Task() {}

  public Task(Queue<Target> targets, bool isNecessary) {
    this.targets = targets;
    this.isNecessary = isNecessary;
  }

  public Target GetNextTarget() {
    if(targets.Count > 0) {
      return targets.Dequeue();
    } else {
      return null;
    }
  }

  public bool GetIsNecessary() {
    return isNecessary;
  }

}

[System.Serializable]
public class Target {

  public Vector3 location;
  public Tile targetTile;
  public WorldResource targetResource;
  public Animation anim;
  public float duration;
  public bool callWhenReach;
  public bool callWhenDone;
  public float dist;

  public Target(Vector3 location, Animation anim, float duration, bool callWhenReach, bool callWhenDone) {
    this.location = location;
    this.targetTile = null;
    this.targetResource = null;
    this.anim = anim;
    this.duration = duration;
    this.callWhenReach = callWhenReach;
    this.callWhenDone = callWhenDone;
    this.dist = 0.5f;
  }

  public Target(Vector3 location, Tile targetTile, Animation anim, float duration, bool callWhenReach, bool callWhenDone) {
    this.location = location;
    this.targetTile = targetTile;
    this.targetResource = null;
    this.anim = anim;
    this.duration = duration;
    this.callWhenReach = callWhenReach;
    this.callWhenDone = callWhenDone;
    this.dist = 0.5f;
  }

  public Target(Vector3 location, WorldResource targetResource, Animation anim, float duration, bool callWhenReach, bool callWhenDone) {
    this.location = location;
    this.targetTile = null;
    this.targetResource = targetResource;
    this.anim = anim;
    this.duration = duration;
    this.callWhenReach = callWhenReach;
    this.callWhenDone = callWhenDone;
    this.dist = 0.5f;
  }

  public Target(Vector3 location, Tile targetTile, WorldResource targetResource, Animation anim, float duration, bool callWhenReach, bool callWhenDone) {
    this.location = location;
    this.targetTile = targetTile;
    this.targetResource = targetResource;
    this.anim = anim;
    this.duration = duration;
    this.callWhenReach = callWhenReach;
    this.callWhenDone = callWhenDone;
    this.dist = 0.5f;
  }

  public Target(Vector3 location, bool callWhenDone) {
    this.location = location;
    this.targetTile = null;
    this.targetResource = null;
    this.anim = null;
    this.duration = 0f;
    this.callWhenReach = false;
    this.callWhenDone = callWhenDone;
    this.dist = 0.5f;
  }

  public void SetDistance(float dist) {
    this.dist = dist;
  }
}
