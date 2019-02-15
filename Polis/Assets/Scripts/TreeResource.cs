using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeResource {

  private GameObject treeObj;

  public TreeResource(GameObject treeObj) {
    this.treeObj = treeObj;
  }

  public GameObject GetResourceObject() {
    return treeObj;
  }

}
