using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Farming : Discipline {
  //Have farming implement(interface) TaskTileSelector to have a button in tasks that selects tiles
  //This script will then interpret and constrain that selection to farmingTiles

  public List<Tile> farmTiles;
  public int maxFarmingTiles;


}
