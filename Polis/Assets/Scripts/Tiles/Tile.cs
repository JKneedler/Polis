using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

  protected int autotileID;
  protected Vector2 mapLoc;
  protected Vector3 worldLoc;
  protected char type;
  protected GameObject tileObj;

  public enum TileTypes{Grass, Forest, Coast, Ocean, Building};
  protected TileTypes tileType;

  public Tile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj) {
    this.mapLoc = mapLoc;
    this.worldLoc = worldLoc;
    this.type = type;
    this.tileObj = tileObj;
  }

  public Tile() {}

  public virtual void SetTileType(char typeChar) {}

  public char GetTypeChar() {
    return type;
  }

  public Vector2 GetMapLoc() {
    return mapLoc;
  }

  public Vector3 GetWorldLoc() {
    return worldLoc;
  }

  public GameObject GetTileObj() {
    return tileObj;
  }

  public void SetTileObj(GameObject tileObj) {
    this.tileObj = tileObj;
  }

  public int GetAutoTileID() {
    return autotileID;
  }

  public virtual bool GetCanBuild() {
    return true;
  }

}
