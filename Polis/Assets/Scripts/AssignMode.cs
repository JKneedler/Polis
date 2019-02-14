using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignMode : MonoBehaviour {

  public Villager villToAssign;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void StartAssignMode(Villager vill) {
      villToAssign = vill;
    }

    public void AttemptToAssign(WorldDescriptor wd) {
      if(wd.gameObject.GetComponent<Structure>()) {
        Structure str = wd.gameObject.GetComponent<Structure>();
        if(str.curWorkers != str.maxWorkers) {
          if(villToAssign.curJob != Villager.Jobs.Citizen) {
            villToAssign.curStructure.UnassignVillager(villToAssign);
          }
          str.AssignVillager(villToAssign);
        }
      }
    }
}
