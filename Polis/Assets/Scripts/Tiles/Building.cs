using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Building : ScriptableObject {

  public enum BuildingTypes {General, ChopTrees, Crafting, Farm, Livestock, Expedition, Producer, Temple}
  public BuildingTypes buildingType;
  public BuildableTile buildingTile;
  public ChopTreesBuildingTile chopTreesBuildTile;

  public BuildableTile GetBuildableTile() {
    BuildableTile bT = buildingTile;
    switch(buildingType) {
      case BuildingTypes.ChopTrees:
        bT = chopTreesBuildTile;
        break;
    }
    return bT;
  }

}
