using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceStorage {
  public Resource res;
  public int amount;
}

public class TownManager : MonoBehaviour {

  public int foodTotal;
  public int drachmaTotal;
  private UIManager ui;
  MasterManager mm;
  public GameObject canvas;
  public JobWorkers[] villagers;
  public List<Task> citizenTasks;
  public List<BuildableTile> builtTiles;
  public List<ResourceStorage> resources;
  public ResourceStorage woodRes;
  public ResourceStorage stoneRes;
  public List<ResourceTile> resTiles;

  public Vector3 tempStorageLoc;

  [System.Serializable]
  public class JobWorkers {
    public List<Villager> workers;
  }

    // Start is called before the first frame update
    void Start() {
      mm = gameObject.GetComponent<MasterManager>();
      ui = canvas.GetComponent<UIManager>();
      SetUI();
    }

    // Update is called once per frame
    void Update() {

    }

    void SetUI() {
      ui.SetFood(foodTotal);
      ui.SetDrachma(drachmaTotal);
      ui.SetWood(woodRes.amount);
      ui.SetStone(woodRes.amount);
      ui.InitializeResourcesPanel(resources);
    }

    void SetFoodTotal() {
      int newTotal = 0;
      for(int i = 0; i < resources.Count; i++) {
        // if(resources[i].GetResourceType() == Resource.ResourceTypes.Food) {
        //   newTotal += resources[i].GetAmount();
        // }
      }
      foodTotal = newTotal;
      SetUI();
    }

    public void BuiltTile(BuildableTile bTile) {
      builtTiles.Add(bTile);
    }

    public void DestroyedTile(BuildableTile bTile) {
      builtTiles.Remove(bTile);
    }

    public Task RequestNextTask() {
      if(citizenTasks.Count > 0) {
         Task t = citizenTasks[0];
         citizenTasks.RemoveAt(0);
         return t;
      } else {
        return GetRandomTask();
      }
    }

    public Task GetRandomTask() {
      Queue<Target> taskQ = new Queue<Target>();
      taskQ.Enqueue(new Target(new Vector3(2, 0, 1), false));
      taskQ.Enqueue(new Target(new Vector3(-2, 0, -1), false));
      return new Task(taskQ, false);
    }

    public void AddTask(Task newTask) {
      bool given = false;
      List<Villager> citizens = villagers[(int)Villager.Jobs.Citizen].workers;
      for(int i = 0; i < citizens.Count; i++) {
        if(!citizens[i].curTask.GetIsNecessary()) {
          citizens[i].SwitchTask(newTask);
          i = citizens.Count;
          given = true;
        }
      }
      if(!given) citizenTasks.Add(newTask);
    }

    public List<Villager> GetPossibleWorkers(Villager.Jobs jType) {
      List<Villager> possibleWorkers = new List<Villager>();
      for(int i = 0; i < villagers[0].workers.Count; i++) {
        possibleWorkers.Add(villagers[0].workers[i]);
      }
      for(int i = 0; i < villagers[(int)jType].workers.Count; i++) {
        possibleWorkers.Add(villagers[(int)jType].workers[i]);
      }
      return possibleWorkers;
    }

    public void AssignVillagerToTile(Villager vill, BuildableTile bTile) {
      bTile.AssignedVillager(vill);
    }

    public void AddBuilding(BuildableTile bTile) {
      builtTiles.Add(bTile);
    }

    public void AddResource(ResourceStorage newRes) {
      int baseType = (int)newRes.res.GetBaseType();
      if(baseType == 1) {
        woodRes.amount += newRes.amount;
        ui.SetWood(woodRes.amount);
      } else if(baseType == 2) {
        stoneRes.amount += newRes.amount;
        ui.SetStone(stoneRes.amount);
      } else {
        int resIndex = GetResourceIndex(newRes);
        if(resIndex == -1) {
          resources.Add(newRes);
          ui.NewResource(newRes);
        } else {
          if(baseType == 0) {
            foodTotal += newRes.amount;
            ui.SetFood(foodTotal);
          }
          resources[resIndex].amount += newRes.amount;
          ui.UpdateResourceValue(resources[resIndex]);
        }
      }
    }

    public int GetResourceIndex(ResourceStorage indRes) {
      int index = -1;
      for(int i = 0; i < resources.Count; i++) {
        if(resources[i].res == indRes.res) {
          index = i;
        }
      }
      return index;
    }

}
