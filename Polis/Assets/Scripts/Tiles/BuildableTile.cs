using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildableTile : Tile {

  [SerializeField]
  protected Vector2 scale;
  protected Vector2 rotatedScale;
  [SerializeField]
  protected GameObject[] tilePrefabs;
  protected List<BuildableTileChild> childrenTiles;
  [SerializeField]
  protected TileTypes buildOnTileType;
  [SerializeField]
  protected bool needsBuilt;
  protected bool beingBuilt;
  [SerializeField]
  protected GameObject[] preBuiltPrefabs;
  protected List<Villager> workers;
  [SerializeField]
  protected Villager.Jobs requiredJob;
  [SerializeField]
  protected Building.BuildingTypes bType;

  public BuildableTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, Vector2 scale)
  : base(mapLoc, worldLoc, type, tileObj) {
    this.scale = scale;
  }

  public BuildableTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, BuildableTile tileToCopy)
  : base(mapLoc, worldLoc, type, tileObj) {
    childrenTiles = new List<BuildableTileChild>();
    CopyTile(tileToCopy);
  }

  public void CopyTile(BuildableTile tileToCopy) {
    // Copy all necessary attributes of the tileToCopy
    // Can override this on inherited class to copy all attributes
    this.scale = tileToCopy.GetScale();
    this.tilePrefabs = tileToCopy.GetTilePrefabs();
    this.needsBuilt = tileToCopy.GetNeedsBuilt();
    this.preBuiltPrefabs = tileToCopy.GetPreBuiltPrefabs();
  }

  public override void SetTileType(char typeChar) {
    tileType = TileTypes.Building;
  }

  public BuildableTile(Vector2 scale) {
    this.scale = scale;
  }

  public Vector2 GetScale() {
    return scale;
  }

  public Vector2 GetRotatedScale() {
    return rotatedScale;
  }

  public TileTypes GetBuildOnType() {
    return buildOnTileType;
  }

  public GameObject[] GetTilePrefabs() {
    return tilePrefabs;
  }

  public bool GetNeedsBuilt() {
    return needsBuilt;
  }

  public GameObject[] GetPreBuiltPrefabs() {
    return preBuiltPrefabs;
  }

  public Building.BuildingTypes GetBuildingType() {
    return bType;
  }

  public void SetBeingBuilt(bool beingBuilt) {
    this.beingBuilt = beingBuilt;
  }

  public void AddBuildTileChild(BuildableTileChild newChild) {
    childrenTiles.Add(newChild);
  }

  public virtual void PlaceTile(Map map, List<Tile> buildingTiles, int rotAmt, Vector2 curBuildRotScale) {
    // Decide how to handle the placing of the tile
    BuildableTile bMain = new BuildableTile(buildingTiles[0].GetMapLoc(), buildingTiles[0].GetWorldLoc(), 'B', null, this);
    bMain.rotatedScale = curBuildRotScale;
    for(int i = 0; i < buildingTiles.Count; i++) {
      Vector3 pos = buildingTiles[i].GetWorldLoc();
      Vector2 tileMapPos = buildingTiles[i].GetMapLoc();
      Vector3 adjustedObjPos = new Vector3(pos.x + 0.5f, 0, pos.z + 0.5f);
      Quaternion tileRot = Quaternion.Euler(0, 90 * rotAmt + 90, 0);
      GameObject newTileObj;
      if(needsBuilt) {
        if(preBuiltPrefabs.Length > 0) {
          buildingTiles[i].DestroyTileObject();
          newTileObj = (GameObject)GameObject.Instantiate(preBuiltPrefabs[i], adjustedObjPos, tileRot);
        } else {
          newTileObj = buildingTiles[i].GetTileObj();
        }
        newTileObj.transform.parent = map.gameObject.transform;
      } else {
        // Wait for worker to be assigned to the tile
        buildingTiles[i].DestroyTileObject();
        newTileObj = (GameObject)GameObject.Instantiate(tilePrefabs[i], adjustedObjPos, tileRot);
        newTileObj.transform.parent = map.gameObject.transform;
      }
      if(i == 0) {
        bMain.SetTileObj(newTileObj);
        map.SetTileFromMapPos((int)tileMapPos.x, (int)tileMapPos.y, bMain);
      } else {
        BuildableTileChild bChild = new BuildableTileChild(buildingTiles[i].GetMapLoc(), buildingTiles[i].GetWorldLoc(), 'b', newTileObj, bMain);
        bMain.AddBuildTileChild(bChild);
        map.SetTileFromMapPos((int)tileMapPos.x, (int)tileMapPos.y, bChild);
      }
    }

    if(needsBuilt) {
      bMain.SetBeingBuilt(true);
      // Call town manager to get first available citizen villager
      // Call BeginBuildPhase(citizen)
    }
    bMain.PlacedTile();
  }

  public virtual void PlacedTile() {

  }

  public void ChangeTileModel(GameObject newModel) {
    Vector3 pos = tileObj.transform.position;
    // Figure out the rotation
    // Destroy previous tileObj
    // Instantiate newModel with pos and new rotation
  }

  public virtual void AssignedVillager(Villager vill) {
    // This will be called whenever a villager is assigned to this building through the menu

    // Give the assigned villager tasks to do
  }

  public virtual void BeginBuildPhase(Villager vill) {
    // Give the citizen tasks to 'build' this building
  }

  public override void ReachedTargetLocation(Tile targetTile, TreeResource targetTree) {

  }

  public override void FinishedTargetLocation(Tile targetTile, TreeResource targetTree) {

  }

  public virtual Task RequestNextTask() {
    return null;
  }

}
