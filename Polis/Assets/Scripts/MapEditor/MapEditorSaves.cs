using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorSaves : MonoBehaviour {

  public List<char[,]> saves;
  public int numSaves;

  public List<char[,]> GetSaves() {
    return saves;
  }

  public void SaveSaves(List<char[,]> newSaves) {
    saves = newSaves;
    numSaves = newSaves.Count;
  }
}
