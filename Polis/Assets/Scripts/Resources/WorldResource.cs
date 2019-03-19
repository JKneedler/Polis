using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldResource {

  private GameObject resObj;

  public WorldResource(GameObject resObj) {
    this.resObj = resObj;
  }

  public GameObject GetResourceObject() {
    return resObj;
  }
}
