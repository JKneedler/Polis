using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildMode : MonoBehaviour {

  GameObject buildPlaceholder;
  public GameObject tileSelectorPrefab;
  List<GameObject> tileSelectors = new List<GameObject>();
  List<Tile> buildingTiles = new List<Tile>();
  public BuildableTile tileToBuild;
  private int curRotAmt;
  private Vector2 curBuildRotScale;
  private Tile curHoverTile;
  public Material redMat;
  Material originalMat;

  // References
  public Map mapScript;
  MasterManager mm;
  TownManager tm;
  UIManager ui;
  public GameObject canvas;

  // Temporary
  public Button buildTest;
  BuildableTile testBuild;
  public Vector2 testScale;

    // Start is called before the first frame update
    void Start() {
      ui = canvas.GetComponent<UIManager>();
      mm = gameObject.GetComponent<MasterManager>();
      tm = gameObject.GetComponent<TownManager>();
      originalMat = tileSelectorPrefab.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial;
      testBuild = new BuildableTile(testScale);
      buildTest.onClick.AddListener(() => mm.StartBuildModeForTile(testBuild));
    }

    // Update is called once per frame
    void Update() {

    }

    public void StartBuildMode(BuildableTile buildTile) {
      tileToBuild = buildTile;
      curRotAmt = 2;
      SetCurBuildRotation(2);
    }

    public void EndBuildMode() {
      for(int i = 0; i < tileSelectors.Count; i++) {
        Destroy(tileSelectors[i]);
      }
      tileToBuild = null;
      buildingTiles.Clear();
      tileSelectors.Clear();
    }

    public void Build(Tile newHoverTile) {
      if(newHoverTile != null && newHoverTile != curHoverTile) {
        List<Tile> possibleBuildTiles = GetHoverTiles(newHoverTile);
        if(!possibleBuildTiles.Contains(null)) {
          if(tileSelectors.Count > 0){
            curHoverTile = newHoverTile;
            buildingTiles = possibleBuildTiles;
            MoveSelectors();
          } else {
            for(int i = 0; i < possibleBuildTiles.Count; i++) {
              Vector3 spawnLoc = possibleBuildTiles[i].GetTileObj().transform.position;
              GameObject tileSel = (GameObject)Instantiate(tileSelectorPrefab, spawnLoc, Quaternion.identity);
              tileSelectors.Add(tileSel);
            }
          }
        }
      }

      // Rotate the Object
      if(Input.GetButtonDown("R")) {
        if(curRotAmt < 3) {
          curRotAmt++;
        } else {
          curRotAmt = 0;
        }
        SetCurBuildRotation(curRotAmt);
        List<Tile> possibleBuildTiles = GetHoverTiles(newHoverTile);
        if(!possibleBuildTiles.Contains(null)) {
          buildingTiles = possibleBuildTiles;
          MoveSelectors();
        }
      }

    }

    public void AttemptToBuild() {
      bool canBuild = true;
      for(int i = 0; i < buildingTiles.Count; i++){
        if(!buildingTiles[i].GetCanBuild()) {
          canBuild = false;
        }
      }
      if(canBuild){
        // Destroy previous tileObj
        // Change the corresponding tile to the tileToBuild
        // Instantiate new tileObj


        // Change tile selectors Color
        for(int i = 0; i < tileSelectors.Count; i++){
          // Change color to red
        }
      }
    }

    void MoveSelectors() {
      for(int i = 0; i < tileSelectors.Count; i++) {
        tileSelectors[i].transform.position = buildingTiles[i].GetTileObj().transform.position;
        MeshRenderer selMat = tileSelectors[i].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        if(buildingTiles[i].GetCanBuild()) {
          selMat.material = originalMat;
          Debug.Log("Can Build");
        } else {
          selMat.material = redMat;
          Debug.Log("Can't Build");
        }
      }
    }

    void SetCurBuildRotation(int rotAmt) {
      Vector2 tileScale = tileToBuild.GetScale();
      Vector2 newBuildRotScale = new Vector2();
      switch(rotAmt) {
        case 0:
          newBuildRotScale.x = tileScale.x;
          newBuildRotScale.y = tileScale.y;
          break;
        case 1:
          newBuildRotScale.x = tileScale.y;
          newBuildRotScale.y = -tileScale.x;
          break;
        case 2:
          newBuildRotScale.x = -tileScale.x;
          newBuildRotScale.y = -tileScale.y;
          break;
        case 3:
          newBuildRotScale.x = -tileScale.y;
          newBuildRotScale.y = tileScale.x;
          break;
      }
      curBuildRotScale = newBuildRotScale;
    }

    List<Tile> GetHoverTiles(Tile hoverT) {
      List<Tile> newTiles = new List<Tile>();
      Vector3 originalTileLoc3 = hoverT.GetWorldLoc();
      Vector2 originalTileLoc = new Vector2(originalTileLoc3.x, originalTileLoc3.z);
      for(int x = 0; x < Mathf.Abs(curBuildRotScale.x); x++) {
        for(int y = 0; y < Mathf.Abs(curBuildRotScale.y); y++) {
          int indexX = x;
          int indexY = y;
          if(curBuildRotScale.x < 0) indexX = -indexX;
          if(curBuildRotScale.y < 0) indexY = -indexY;
          int posX = (int)originalTileLoc.x + indexX;
          int posY = (int)originalTileLoc.y + indexY;
          Tile buildTile = mapScript.GetTileFromWorldPos(posX, posY);
          newTiles.Add(buildTile);
        }
      }
      return newTiles;
    }

    public void DestroyBuilding() {
      // Change to grass tile
    }
}
