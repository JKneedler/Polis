using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour {

  public enum Jobs {Citizen, Forester, StoneMiner, Farmer}
  public Jobs curJob;
  public Structure curStructure;
  public float moveSpeed;
  public Transform target;
  public bool atTarget = false;
  public float minDist;
  private MasterManager mm;
  private bool inAnimation;
  private bool atStructure;
  public string villagerName;

  //Use if(target == curStructure.gameObject.transform.position) to get whether out or back at structure

    void Start() {
      mm = GameObject.FindWithTag("GameController").GetComponent<MasterManager>();
    }

    void Update() {

      if(!atTarget && target) {
        atTarget = (Vector3.Distance(target.position, transform.position) <= minDist);
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime * mm.timeMultiplier);
      } else if(atTarget && target){

        if(target == curStructure.gameObject.transform) {
          atStructure = true;
          DoneAtTarget();
        } else {

          atStructure = false;
          //When I get animations of the specific jobs, add the function trigger at the end of the animation
          //Use this else if block to start the actual animation
          DoneAtTarget();
        }
      }

    }

    public void ChangeTarget(Transform target) {
      this.target = target;
      atTarget = false;
    }

    public void DoneAtTarget() {
      switch (curJob) {
        case Jobs.Forester:
          if(atStructure) {
            //Deal with if no more resources in range
            TreeResource newTree = curStructure.foresterData.GetRandomTree(curStructure.tilesInRange);
            target = newTree.GetResourceObject().transform;
          } else {
            target = curStructure.gameObject.transform;
          }
          atTarget = false;
          break;
        default:
          break;
      }
    }

    public void ChangedAssignment(Structure str, Jobs newJob) {
      curJob = newJob;
      curStructure = str;
      ChangeTarget(curStructure.gameObject.transform);
    }

}
