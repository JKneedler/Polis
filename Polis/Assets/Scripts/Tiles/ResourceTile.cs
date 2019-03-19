using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : Tile {

  protected List<WorldResource> resources;
  protected bool isTrees;
  protected bool canSpread;

  public ResourceTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, List<WorldResource> resources, bool isTrees)
  : base(mapLoc, worldLoc, type, tileObj) {
    this.resources = resources;
    this.isTrees = isTrees;
  }

  public override void SetTileType(char typeChar) {
    tileType = TileTypes.Resource;
  }

  public void AddResource(WorldResource res) {
    resources.Add(res);
  }

  public List<WorldResource> GetResourcesList() {
    return resources;
  }

  public bool GetIsTrees() {
    return isTrees;
  }

  public override bool GetCanBuild(TileTypes buildOnType) {
    // Change to account for the stage of growth that the oldest tree is in
    if(buildOnType == tileType) {
    }
    return (buildOnType == tileType);
  }

}
