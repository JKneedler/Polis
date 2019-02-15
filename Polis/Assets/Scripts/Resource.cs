using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource")]
public class Resource : ScriptableObject {

	public string resourceName;
	public int amount;
	public enum ResourceTypes{Food, Wood, Stone};
	public ResourceTypes resourceType;

	public int GetAmount() {
		return amount;
	}

	public void AddAmount(int amt) {
		amount += amt;
	}

	public void RemoveAmount(int amt) {
		amount -= amt;
	}

	public ResourceTypes GetResourceType() {
		return resourceType;
	}

}
