using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableTileChild : Tile {

  protected BuildableTile parent;

  public BuildableTileChild(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, BuildableTile parent)
  : base(mapLoc, worldLoc, type, tileObj) {
    this.parent = parent;
  }

  public override void SetTileType(char typeChar) {
    tileType = TileTypes.BuildingChild;
  }

  public BuildableTile GetParent() {
    return parent;
  }

}
