using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalResource {

  public Structure.Resources resourceType;
  private GameObject resourceObj;

  public NaturalResource(Structure.Resources resourceType, GameObject resourceObj) {
    this.resourceType = resourceType;
    this.resourceObj = resourceObj;
  }

  public NaturalResource() {
    
  }

  public Structure.Resources GetResourceType() {
    return resourceType;
  }

  public GameObject GetResourceObject() {
    return resourceObj;
  }

}
