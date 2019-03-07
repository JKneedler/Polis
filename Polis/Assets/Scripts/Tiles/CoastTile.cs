using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastTile : Tile {

  public CoastTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj)
  : base(mapLoc, worldLoc, type, tileObj) {

  }

  public override void SetTileType(char typeChar) {
    tileType = TileTypes.Coast;
  }

}
