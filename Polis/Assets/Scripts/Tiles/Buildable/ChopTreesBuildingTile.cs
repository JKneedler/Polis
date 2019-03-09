using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChopTreesBuildingTile : BuildableTile {

  public Map mapScript;
  protected int treesLeft;

  public ChopTreesBuildingTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, Vector2 scale)
  : base(mapLoc, worldLoc, type, tileObj, scale) {

  }

  public ChopTreesBuildingTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, ChopTreesBuildingTile tileToCopy)
  : base(mapLoc, worldLoc, type, tileObj, new Vector2(1, 1)) {
    CopyTile(tileToCopy);
  }

  public override void PlaceTile(Map map, List<Tile> buildingTiles, int rotAmt, Vector2 curBuildRotScale) {
    rotatedScale = curBuildRotScale;
    ChopTreesBuildingTile bMain = new ChopTreesBuildingTile(buildingTiles[0].GetMapLoc(), buildingTiles[0].GetWorldLoc(), 'B', buildingTiles[0].GetTileObj(), this);
    bMain.mapScript = map;
    Vector2 tileMapPos = buildingTiles[0].GetMapLoc();
    ForestTile fT = buildingTiles[0] as ForestTile;
    List<TreeResource> trees = fT.GetResourcesList();
    map.SetTileFromMapPos((int)tileMapPos.x, (int)tileMapPos.y, bMain);
    TownManager tm = GameObject.FindWithTag("GameController").GetComponent<TownManager>();
    bMain.SetTreesLeft(trees.Count);
    for(int i = 0; i < trees.Count; i++) {
        // Create a task for each tree and send it to the town manager citizenTasks list
        Queue<Target> taskQ = new Queue<Target>();
        Vector3 loc = trees[i].GetResourceObject().transform.position;
        taskQ.Enqueue(new Target(loc, bMain, trees[i], null, 4f, false, true));
        taskQ.Enqueue(new Target(new Vector3(0, 0, 0), false));
        Task t = new Task(taskQ, true);
        tm.AddTask(t);
    }
  }

  public override void FinishedTargetLocation(Tile targetTile, TreeResource targetTree) {
    GameObject.Destroy(targetTree.GetResourceObject());
    treesLeft--;
    if(treesLeft == 0) {
      // Change tile back to Grass Tile
      GrassTile gT = new GrassTile(mapLoc, worldLoc, type, tileObj);
      mapScript.SetTileFromMapPos((int)mapLoc.x, (int)mapLoc.y, gT);
    }
    // Give the town some wood resources
  }

  public void SetTreesLeft(int left) {
    treesLeft = left;
  }

}
