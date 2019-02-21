using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

  public int autotileID;
  private Vector2 mapLoc;
  private char type;
  private int numResources;
  private GameObject tileObject;
  private int MAP_CORRECTION_WIDTH;
  private int MAP_CORRECTION_HEIGHT;
  private bool canBuild;
  private List<TreeResource> trees;
  private Material originalMat;
  public enum TileTypes {Grass, Forest, Water, Coast, FarmLand, Pasture};
  public TileTypes tileTypeEnum;

  public Tile(Vector2 mapLoc, char type, int MAP_CORRECTION_WIDTH, int MAP_CORRECTION_HEIGHT) {
    this.mapLoc = mapLoc;
    SetTypeChar(type);
    this.MAP_CORRECTION_WIDTH = MAP_CORRECTION_WIDTH;
    this.MAP_CORRECTION_HEIGHT = MAP_CORRECTION_HEIGHT;
    trees = new List<TreeResource>();
  }

  public char GetTypeChar() {
    return type;
  }

  public void SetTypeChar(char type) {
    this.type = type;
    switch (type) {
      case 'G':
        tileTypeEnum = TileTypes.Grass;
        break;
      case 'W':
        tileTypeEnum = TileTypes.Water;
        break;
      case 'F':
        tileTypeEnum = TileTypes.Forest;
        break;
    }
  }

  public Vector2 GetMapLoc() {
    return mapLoc;
  }

  public Vector2 GetWorldLoc() {
    return new Vector2(mapLoc.x - MAP_CORRECTION_WIDTH, mapLoc.y - MAP_CORRECTION_HEIGHT);
  }

  public GameObject GetTileObject() {
    return tileObject;
  }

  public void SetTileObject(GameObject tileObj) {
    tileObject = tileObj;
    originalMat = tileObj.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
  }

  public void ChangeMaterial(Material mat) {
    tileObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = mat;
    if(autotileID == -1) {
      tileObject.SetActive(true);
    }
  }

  public void SetMaterialBackToOriginal() {
    tileObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = originalMat;
    if(autotileID == -1) {
      tileObject.SetActive(false);
    }
  }

  public int GetAutoTileID() {
    return autotileID;
  }

  public void SetAutoTileID(int id) {
    autotileID = id;
    DetermineBuildStatus();
  }

  private void DetermineBuildStatus() {
    if(autotileID == 15) {
      canBuild = true;
    } else {
      canBuild = false;
    }
  }

  public bool GetCanBuild() {
    return canBuild;
  }

  public void SetCanBuild(bool newVal) {
    canBuild = newVal;
  }

  public void AddResource(TreeResource res) {
    trees.Add(res);
  }

  public void RemoveResource(TreeResource res) {
    trees.Remove(res);
  }

  public List<TreeResource> GetResourcesList() {
    return trees;
  }

  public TreeResource GetRandomTree() {
    return trees[Random.Range(0, trees.Count)];
  }

  public void SetNumResources(int amt) {
    numResources = amt;
  }

  public void DecrementNumResources() {
    numResources--;
  }

  public int GetNumResources() {
    return numResources;
  }
}
