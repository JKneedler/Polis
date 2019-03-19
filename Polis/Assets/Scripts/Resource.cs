using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource")]
public class Resource : ScriptableObject {

	public string resourceName;
	public enum SubTypes{Farming, Livestock, Fishing, Mining};
	public enum BaseTypes{Food, Wood, Stone, None};
	public SubTypes subType;
	public BaseTypes baseType;
	public Sprite icon;


	public BaseTypes GetBaseType() {
		return baseType;
	}

	public SubTypes GetSubType() {
		return subType;
	}

	public Sprite GetIcon() {
		return icon;
	}

}
