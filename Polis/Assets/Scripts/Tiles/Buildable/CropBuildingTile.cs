using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropBuildingTile : BuildableTile {

  [SerializeField]
  protected Resource crop;
  [SerializeField]
  protected float growTime;
  [SerializeField]
  protected float[] stageTimes;
  protected float curGrowTime;
  int curStageNum;
  bool grown;

  public CropBuildingTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, Vector2 scale)
  : base(mapLoc, worldLoc, type, tileObj, scale) {

  }

  public CropBuildingTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, Vector2 scale, CropBuildingTile tileToCopy)
  : base(mapLoc, worldLoc, type, tileObj, scale) {
    CopyTile(tileToCopy);
    this.crop = tileToCopy.GetCrop();
    this.growTime = tileToCopy.GetGrowTime();
    this.stageTimes = tileToCopy.GetStageTimes();
  }

  public Resource GetCrop() {
    return crop;
  }

  public float GetGrowTime() {
    return growTime;
  }

  public float[] GetStageTimes() {
    return stageTimes;
  }

  public override void PlaceTile(TownManager tm, Map map, List<Tile> buildingTiles, int rotAmt, Vector2 curBuildRotScale) {
    CropBuildingTile bMain = new CropBuildingTile(buildingTiles[0].GetMapLoc(), buildingTiles[0].GetWorldLoc(), 'B', null, new Vector2(1, 1), this);
    bMain.rotatedScale = curBuildRotScale;
    Vector3 pos = buildingTiles[0].GetWorldLoc();
    Vector2 tileMapPos = buildingTiles[0].GetMapLoc();
    Vector3 adjustedObjPos = new Vector3(pos.x + 0.5f, 0, pos.z + 0.5f);
    Quaternion tileRot = Quaternion.Euler(0, 90 * rotAmt + 90, 0);
    GameObject newTileObj;
    buildingTiles[0].DestroyTileObject();
    newTileObj = (GameObject)GameObject.Instantiate(tileStages[0].tilePrefabs[0], adjustedObjPos, tileRot);
    newTileObj.transform.parent = map.gameObject.transform;
    bMain.SetTileObj(newTileObj);
    map.SetTileFromMapPos((int)tileMapPos.x, (int)tileMapPos.y, bMain);
    bMain.PlacedTile();
    tm.AddBuilding(bMain);
  }

  public override void PlacedTile() {
    curGrowTime = 0;
    curStageNum = 0;
    grown = false;
  }

  public override void AssignedVillager(Villager vill) {
    workers.Add(vill);
    vill.AssignToTile(this);
    vill.GetNewTask(RequestNextTask());
  }

  public override void ReachedTargetLocation(Tile targetTile, WorldResource targetResource) {
  }

  public override void FinishedTargetLocation(Tile targetTile, WorldResource targetResource) {
    if(grown) {
      Harvest();
    }
  }

  public override Task RequestNextTask() {
    Queue<Target> taskQ = new Queue<Target>();
    if(!grown) {
      Target t1 = new Target(tileObj.transform.position, false);
      Target t2 = new Target(new Vector3(0, 0, 0), false);
      taskQ.Enqueue(t1);
      taskQ.Enqueue(t2);
    } else {
      Target t1 = new Target(tileObj.transform.position, this, null, 0f, false, true);
      Target t2 = new Target(new Vector3(0, 0, 0), false);
      taskQ.Enqueue(t1);
      taskQ.Enqueue(t2);
    }
    Task newTask = new Task(taskQ, false);
    return newTask;
  }

  public override void Update() {
    if(!grown) {
      curGrowTime += Time.deltaTime;
      int stageNum = 0;
      for(int i = 0; i < stageTimes.Length; i++) {
        if(curGrowTime > stageTimes[i]) {
          stageNum = i;
        }
      }
      if(stageNum != curStageNum) {
        Debug.Log("Stage Changed : " + stageNum);
        Vector3 loc = tileObj.transform.position;
        Quaternion rot = tileObj.transform.rotation;
        Transform parent = tileObj.transform.parent;
        GameObject.Destroy(tileObj);
        GameObject newObj = (GameObject)GameObject.Instantiate(tileStages[stageNum].tilePrefabs[0], loc, rot);
        newObj.transform.parent = parent;
        tileObj = newObj;
        curStageNum = stageNum;
      }
      if(curGrowTime > growTime) {
        grown = true;
      }
    }
  }

  public void Harvest() {
    Debug.Log("Harvesting");
    curGrowTime = 0;
    curStageNum = 0;
    grown = false;
    Vector3 loc = tileObj.transform.position;
    Quaternion rot = tileObj.transform.rotation;
    Transform parent = tileObj.transform.parent;
    GameObject.Destroy(tileObj);
    GameObject newObj = (GameObject)GameObject.Instantiate(tileStages[curStageNum].tilePrefabs[0], loc, rot);
    newObj.transform.parent = parent;
    tileObj = newObj;
  }

}
