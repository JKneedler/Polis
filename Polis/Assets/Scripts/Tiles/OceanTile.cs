using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanTile : Tile {

  public int fishType;

  public OceanTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj)
  : base(mapLoc, worldLoc, type, tileObj) {

  }

  public override void SetTileType(char typeChar) {
    tileType = TileTypes.Ocean;
  }

}
