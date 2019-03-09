using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour {

  public int foodTotal;
  public int woodTotal;
  public int stoneTotal;
  public int drachmaTotal;
  private UIManager ui;
  public GameObject canvas;
  public JobWorkers[] villagers;
  public List<Task> citizenTasks;
  public List<BuildableTile> builtTiles;
  public List<Resource> resources;
  public Resource wood;
  public Resource stone;

  [System.Serializable]
  public class JobWorkers {
    public List<Villager> workers;
  }

    // Start is called before the first frame update
    void Start() {
      ui = canvas.GetComponent<UIManager>();
      SetUI();
    }

    // Update is called once per frame
    void Update() {

    }

    void SetUI() {
      // ui.SetPopulation(villagers[0].Count);
      ui.SetFood(foodTotal);
      ui.SetWood(wood.GetAmount());
      ui.SetStone(stone.GetAmount());
      ui.SetDrachma(drachmaTotal);

    }

    void SetFoodTotal() {
      int newTotal = 0;
      for(int i = 0; i < resources.Count; i++) {
        if(resources[i].GetResourceType() == Resource.ResourceTypes.Food) {
          newTotal += resources[i].GetAmount();
        }
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

}
