using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour {

  public enum Jobs {Citizen, Explorer, Farmer, Herder, Fisher, Adventurer, Miner, Priest}
  public Jobs curJob;
  public BuildableTile workTile;
  public string villagerName;
  public Task curTask;
  public Target curTarget;
  public float taskTimer;
  public float moveSpeed;
  public bool atTarget;

  // References
  public Transform target;
  private MasterManager mm;
  private TownManager tm;

    void Start() {
      mm = GameObject.FindWithTag("GameController").GetComponent<MasterManager>();
      tm = mm.gameObject.GetComponent<TownManager>();
      if(curJob == Jobs.Citizen) {
        GetNewTask(tm.RequestNextTask());
      } else {
        GetNewTask(workTile.RequestNextTask());
      }
      atTarget = false;
    }

    void Update() {
      if(!atTarget) {
        transform.position = Vector3.MoveTowards(transform.position, curTarget.location, moveSpeed * Time.deltaTime);
        atTarget = (Vector3.Distance(curTarget.location, transform.position) <= curTarget.dist);
        if(atTarget) {
          GotToTarget();
        }
      } else if(atTarget && taskTimer > 0){
        taskTimer -= Time.deltaTime;
      } else if(atTarget && taskTimer <= 0) {
        DoneAtTarget();
      }
      workTile.Update();
    }

    public void AssignToTile(BuildableTile newTile) {
      workTile = newTile;
      curJob = newTile.GetRequiredJobType();
    }

    public void GetNewTask(Task newTask) {
      curTask = newTask;
      curTarget = curTask.GetNextTarget();
      atTarget = false;
    }

    public void DoneAtTarget() {
      if(curTarget.callWhenDone) {
        curTarget.targetTile.FinishedTargetLocation(curTarget.targetTile, curTarget.targetResource);
      }
      Target nextTarget = curTask.GetNextTarget();
      if(nextTarget != null) {
        curTarget = nextTarget;
        atTarget = false;
      } else {
        if(curJob != Jobs.Citizen) {
          GetNewTask(workTile.RequestNextTask());
        } else {
          GetNewTask(tm.RequestNextTask());
        }
      }
    }

    public void GotToTarget() {
      if(curTarget.callWhenReach) {
        curTarget.targetTile.ReachedTargetLocation(curTarget.targetTile, curTarget.targetResource);
      }
      taskTimer = curTarget.duration;
    }

    public void SwitchTask(Task newTask) {
      GetNewTask(newTask);
    }

}
