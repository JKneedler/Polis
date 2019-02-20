using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingType {
  public string thing;

  public BuildingType() {}

  public BuildingType(string thing) {
    this.thing = thing;
  }

  public virtual void TestInheritance() {}
}
