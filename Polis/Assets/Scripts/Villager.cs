using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour {

  // Should only be dictating how the person looks + UI information
  public enum Jobs {Farmer, Fisher, Forester}
  public Jobs curJob;
  public string villagerName;
  public Process processOn;
  public Task curTask;
  public bool hasTask;
  public bool returningFromTask;
  public float taskTimer;
  public float moveSpeed;
  public bool atTarget = false;
  public float minDist;

  public Transform target;
  private MasterManager mm;
  private TownManager tm;

  //Use if(target == curStructure.gameObject.transform.position) to get whether out or back at structure

    void Start() {
      mm = GameObject.FindWithTag("GameController").GetComponent<MasterManager>();
      tm = mm.gameObject.GetComponent<TownManager>();
      curTask = null;
      hasTask = false;
    }

    void Update() {
      if(target && hasTask) {
        if(!atTarget) {
          atTarget = (Vector3.Distance(target.position, transform.position) <= minDist);
          transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime * mm.timeMultiplier);
        } else if(atTarget && taskTimer > 0 && !returningFromTask){
          taskTimer -= Time.deltaTime;
        } else if(atTarget && taskTimer < 0) {
          Discipline disc = tm.GetDisciplineFromIndex((int)curJob);
          if(returningFromTask) {
            disc.VillagerCompletedTask(this);
          } else {
            if(curTask.GetContactDisciplineWhenReached()) disc.ReachedTaskTarget(this);
            target = curTask.GetReturnTargetWD().villagerTarget;
            Debug.Log(curTask.GetReturnTargetWD().villagerTarget);
            atTarget = false;
            returningFromTask = true;
          }
        }
      }
    }

    public void ChangedAssignment(Jobs newJob) {
      curJob = newJob;
      tm.GetDisciplineFromIndex((int)curJob).AttemptToGetNextTask(this);
    }

    public void NewTask(Task newTask) {
      curTask = newTask;
      returningFromTask = false;
      atTarget = false;
      hasTask = true;
      target = curTask.GetTargetWD().villagerTarget;
      taskTimer = curTask.GetTaskDuration();
      Debug.Log("Got new Task");
    }

}
