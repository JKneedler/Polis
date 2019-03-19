using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectResourceBuildingTile : BuildableTile {

  public Map mapScript;
  protected int resourcesLeft;
  [SerializeField]
  protected ResourceStorage woodRes;
  [SerializeField]
  protected ResourceStorage stoneRes;
  protected bool isTrees;

  public CollectResourceBuildingTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, Vector2 scale)
  : base(mapLoc, worldLoc, type, tileObj, scale) {
  }

  public CollectResourceBuildingTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, CollectResourceBuildingTile tileToCopy, bool isTrees)
  : base(mapLoc, worldLoc, type, tileObj, new Vector2(1, 1)) {
    CopyTile(tileToCopy);
    this.woodRes = tileToCopy.woodRes;
    this.stoneRes = tileToCopy.stoneRes;
    this.isTrees = isTrees;
  }

  public override void PlaceTile(TownManager tm, Map map, List<Tile> buildingTiles, int rotAmt, Vector2 curBuildRotScale) {
    ResourceTile rT = buildingTiles[0] as ResourceTile;
    rotatedScale = curBuildRotScale;
    CollectResourceBuildingTile bMain = new CollectResourceBuildingTile(buildingTiles[0].GetMapLoc(), buildingTiles[0].GetWorldLoc(), 'B', buildingTiles[0].GetTileObj(), this, rT.GetIsTrees());
    bMain.mapScript = map;
    bMain.tm = tm;
    Vector2 tileMapPos = buildingTiles[0].GetMapLoc();
    List<WorldResource> resources = rT.GetResourcesList();
    map.SetTileFromMapPos((int)tileMapPos.x, (int)tileMapPos.y, bMain);
    bMain.SetResourcesLeft(resources.Count);
    for(int i = 0; i < resources.Count; i++) {
        // Create a task for each tree and send it to the town manager citizenTasks list
        Queue<Target> taskQ = new Queue<Target>();
        Vector3 loc = resources[i].GetResourceObject().transform.position;
        taskQ.Enqueue(new Target(loc, bMain, resources[i], null, 4f, false, true));
        taskQ.Enqueue(new Target(new Vector3(0, 0, 0), false));
        Task t = new Task(taskQ, true);
        tm.AddTask(t);
    }
  }

  public override void FinishedTargetLocation(Tile targetTile, WorldResource targetResource) {
    GameObject.Destroy(targetResource.GetResourceObject());
    resourcesLeft--;
    if(resourcesLeft == 0) {
      // Change tile back to Grass Tile
      GrassTile gT = new GrassTile(mapLoc, worldLoc, type, tileObj);
      mapScript.SetTileFromMapPos((int)mapLoc.x, (int)mapLoc.y, gT);
    }
    if(isTrees) {
      tm.AddResource(woodRes);
    } else {
      tm.AddResource(stoneRes);
    }
  }

  public void SetResourcesLeft(int left) {
    resourcesLeft = left;
  }
}
