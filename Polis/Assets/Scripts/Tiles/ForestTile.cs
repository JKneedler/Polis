using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTile : Tile {

  protected List<TreeResource> trees;

  public ForestTile(Vector2 mapLoc, Vector3 worldLoc, char type, GameObject tileObj, List<TreeResource> trees)
  : base(mapLoc, worldLoc, type, tileObj) {
    this.trees = trees;
  }

  public override bool GetCanBuild() {
    return false;
  }

  public override void SetTileType(char typeChar) {
    tileType = TileTypes.Forest;
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

  public int GetNumResources() {
    return trees.Count;
  }

}
