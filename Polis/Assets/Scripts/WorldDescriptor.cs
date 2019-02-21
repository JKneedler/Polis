using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDescriptor : MonoBehaviour {

  public string title;
  public enum objectTypes {Villager, Building, Resource}
  public objectTypes objectType;
  public string description;
  public Transform villagerTarget;

    // Start is called before the first frame update
    void Start() {
      villagerTarget = transform;
    }

    // Update is called once per frame
    void Update() {

    }
}
