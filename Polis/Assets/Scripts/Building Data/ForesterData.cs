using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForesterData : BuildingType {

  public TreeResource GetRandomTree(List<Tile> tilesInRange) {
    List<TreeResource> nearTrees = new List<TreeResource>();
    for(int i = 0; i < tilesInRange.Count; i++) {
      if(tilesInRange[i].GetNumResources() > 0) {
        nearTrees.Add(tilesInRange[i].GetRandomTree());
      }
    }
    return nearTrees[Random.Range(0, nearTrees.Count)];
  }

}
