using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {

  public Vector2 objScale;
  public Villager.Jobs structureJob;
  public int foodProdMax;
  public int foodProdCur;
  public int woodProdMax;
  public int woodProdCur;
  public int stoneProdMax;
  public int stoneProdCur;
  public int maxWorkers;
  public int curWorkers;
  public Villager[] workers;
  public int resourceRange;
  public enum Resources {Tree, Rock}
  public Resources resourceDesignation;
  public List<Tile> nearTiles;
  public List<NaturalResource> nearResources;
  public List<Tile> tilesOn;
  private TownManager tm;
  private Map map;

  //Add location for the structure to villagers know where to stay around

    // Start is called before the first frame update
    void Start() {
      workers = new Villager[maxWorkers];
      tm = GameObject.FindWithTag("GameController").GetComponent<TownManager>();
      map = GameObject.FindWithTag("Map").GetComponent<Map>();
      nearTiles = new List<Tile>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Place() {
      tilesOn = new List<Tile>();
      FindNearResources();
    }

    public void FindNearResources() {

      for(int i = 1; i < resourceRange + 1; i++) {
        for(int x = -i; x < i + 1; x++) {
          Tile tilePlusY = map.GetTileFromWorldPos((int)transform.position.x + x, (int)transform.position.z + (resourceRange - Mathf.Abs(x)));
          Tile tileMinusY = map.GetTileFromWorldPos((int)transform.position.x + x, (int)transform.position.z - (resourceRange - Mathf.Abs(x)));
          nearTiles.Add(tilePlusY);
          if(x != -i && x != i) {
            nearTiles.Add(tileMinusY);
          }
        }
      }
    }

    public void AssignVillager(Villager vill) {
      bool placedVill = false;
      int i = 0;
      while(!placedVill && i < workers.Length) {
        if(!workers[i]) {
          workers[i] = vill;
          placedVill = true;
          vill.ChangedAssignment(this, structureJob);
          curWorkers++;
        }
      }
      UpdateStructureResourceRates();
      tm.UpdateResourceRates();
    }

    public void UnassignVillager(Villager vill) {
      for(int i = 0; i < workers.Length; i++) {
        if(workers[i] == vill) {
          workers[i] = null;
          curWorkers--;
        }
      }
      UpdateStructureResourceRates();
      tm.UpdateResourceRates();
    }

    public bool CanAssignVillager() {
      return !(curWorkers == maxWorkers);
    }

    void UpdateStructureResourceRates() {
      foodProdCur = (int)(foodProdMax * ((float)curWorkers/maxWorkers));
      woodProdCur = (int)(woodProdMax * ((float)curWorkers/maxWorkers));
      stoneProdCur = (int)(stoneProdMax * ((float)curWorkers/maxWorkers));
    }

    public NaturalResource GetRandomNaturalResource() {
      List<NaturalResource> resources = new List<NaturalResource>();
      for(int i = 0; i < nearTiles.Count; i++) {
        List<NaturalResource> tileResources = nearTiles[i].GetResourcesList();
        for(int k = 0; k < tileResources.Count; k++) {
          resources.Add(tileResources[k]);
        }
      }
      return resources[Random.Range(0, resources.Count)];
    }

}
