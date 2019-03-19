using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Building : ScriptableObject {

  public enum BuildingTypes {General, CollectResource, Crafting, Crop, Livestock, Expedition, Producer, Temple}
  public BuildingTypes buildingType;
  public BuildableTile buildingTile;
  public CollectResourceBuildingTile collectResourceTile;
  public CropBuildingTile cropTile;

  public BuildableTile GetBuildableTile() {
    BuildableTile bT = buildingTile;
    switch(buildingType) {
      case BuildingTypes.CollectResource:
        bT = collectResourceTile;
        break;
      case BuildingTypes.Crop:
        bT = cropTile;
        break;
    }
    return bT;
  }

}
