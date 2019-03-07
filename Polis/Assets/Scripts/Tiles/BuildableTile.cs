using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableTile : Tile {

  protected Vector2 scale;
  protected GameObject[][] tilePrefabs;
  protected bool isMainTile;
  protected BuildableTile[][] childrenTiles;
  protected BuildableTile mainTile;
  protected TileTypes buildOnTileType;

  public BuildableTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, Vector2 scale)
  : base(mapLoc, worldLoc, type, tileObj) {
    this.scale = scale;
  }

  public override bool GetCanBuild() {
    return false;
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

}
