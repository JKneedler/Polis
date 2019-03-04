using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDescriptor : MonoBehaviour {

  public string title;
  public enum objectTypes {Villager, Building, Resource, Other}
  public objectTypes objectType;
  public string description;
  public Transform villagerTarget;
  public Tile tile;

  public WorldDescriptor() {
    title = "";
    objectType = objectTypes.Other;
    description = "";
  }

  public void SetTile(Tile tile) {
    this.tile = tile;
    villagerTarget = tile.GetTileObject().transform;
  }

  public Tile GetTile() {
    return tile;
  }

    // Start is called before the first frame update
    void Start() {
      if(tile == null) {
        villagerTarget = transform;
      }
    }

    // Update is called once per frame
    void Update() {

    }
}
