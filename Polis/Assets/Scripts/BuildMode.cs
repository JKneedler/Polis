using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildMode : MonoBehaviour {

  GameObject buildPlaceholder;
  List<Tile> buildingTiles = new List<Tile>();
  public GameObject[] structures;
  public int structureIndex;
  public float scale;
  public Map mapScript;
  MasterManager mm;
  TownManager tm;
  UIManager ui;
  public GameObject canvas;
  public bool build;
  public int curRotAmt;
  public Vector2 curBuildRotScale;
  public Tile hoverTile;
  public Material greenMat;
  public Material redMat;

  public GameObject buildMenuObject;
  public GameObject buildMenuButton;

    // Start is called before the first frame update
    void Start() {
      ui = canvas.GetComponent<UIManager>();
        mm = gameObject.GetComponent<MasterManager>();
        tm = gameObject.GetComponent<TownManager>();
        GenerateBuildMenuOptions();
    }

    // Update is called once per frame
    void Update() {

    }

    public void StartBuildMode() {
      build = true;
      buildMenuObject.SetActive(true);
      SetCurBuildRotation(0);
    }

    public void EndBuildMode() {
      build = false;
      buildMenuObject.SetActive(false);
      if(buildPlaceholder){
        for(int i = 0; i < buildingTiles.Count; i++) {
          buildingTiles[i].SetMaterialBackToOriginal();
        }
        Destroy(buildPlaceholder);
        buildPlaceholder = null;
        buildingTiles.Clear();
      }
    }

    public void Build(Vector3 mousePos, Tile newTile) {
      Structure str = structures[structureIndex].GetComponent<Structure>();
      if(newTile != null) {
        Vector2 tileWorldLoc = newTile.GetWorldLoc();
        Vector3 newBuildPos = GetNewBuildPos((int)tileWorldLoc.x, (int)tileWorldLoc.y);
        if(buildPlaceholder){
          List<Tile> possibleHoverTiles = GetHoverTiles(newTile);
          if(newTile != hoverTile && !possibleHoverTiles.Contains(null)) {
            hoverTile = newTile;
            buildPlaceholder.transform.position = newBuildPos;
            ChangeBuildTiles(possibleHoverTiles);
          }

          // Rotate the Object
          if(Input.GetButtonDown("R") && !possibleHoverTiles.Contains(null)) {
            buildPlaceholder.transform.Rotate(Vector3.up * 90);
            if(curRotAmt < 3) {
              curRotAmt++;
            } else {
              curRotAmt = 0;
            }
            SetCurBuildRotation(curRotAmt);
            newBuildPos = GetNewBuildPos((int)tileWorldLoc.x, (int)tileWorldLoc.y);
            buildPlaceholder.transform.position = newBuildPos;
            possibleHoverTiles = GetHoverTiles(newTile);
            ChangeBuildTiles(possibleHoverTiles);
          }

        } else {
          buildPlaceholder = (GameObject)Instantiate(structures[structureIndex], newBuildPos, Quaternion.identity);
          buildPlaceholder.transform.Rotate(Vector3.up * 90 * curRotAmt);
          buildPlaceholder.GetComponent<BoxCollider>().enabled = false;
        }
      }
    }

    public Vector3 GetNewBuildPos(int x, int y) {
      int newX = x;
      int newY = y;
      switch(curRotAmt) {
        case 1:
          newY += 1;
          break;
        case 2:
          newX += 1;
          newY += 1;
          break;
        case 3:
          newX += 1;
          break;
      }
      return new Vector3(newX, 0, newY);
    }

    public void AttemptToBuild() {
      bool canBuild = true;
      for(int i = 0; i < buildingTiles.Count; i++){
        if(!buildingTiles[i].GetCanBuild()) {
          canBuild = false;
        }
      }
      if(canBuild){
        Structure str = buildPlaceholder.GetComponent<Structure>();
        str.Place();
        buildPlaceholder.GetComponent<BoxCollider>().enabled = true;
        buildPlaceholder = null;

        //Change tile Color
        for(int i = 0; i < buildingTiles.Count; i++){
          buildingTiles[i].ChangeMaterial(redMat);
          buildingTiles[i].SetCanBuild(false);
          str.tilesOn.Add(buildingTiles[i]);
        }

        tm.BuiltStructure(str);
      }
    }

    void ChangeBuildTiles(List<Tile> newTiles) {
      for(int i = 0; i < buildingTiles.Count; i++) {
        if(!newTiles.Contains(buildingTiles[i])) {
          buildingTiles[i].SetMaterialBackToOriginal();
        }
      }
      for(int i = 0; i < newTiles.Count; i++) {
        if(newTiles[i].GetCanBuild()) {
          newTiles[i].ChangeMaterial(greenMat);
        } else {
          newTiles[i].ChangeMaterial(redMat);
        }
      }
      buildingTiles = newTiles;
    }

    void SetCurBuildRotation(int rotAmt) {
      Structure str = structures[structureIndex].GetComponent<Structure>();
      Vector2 newBuildRotScale = new Vector2();
      switch(rotAmt) {
        case 0:
          newBuildRotScale.x = str.objScale.x;
          newBuildRotScale.y = str.objScale.y;
          break;
        case 1:
          newBuildRotScale.x = str.objScale.y;
          newBuildRotScale.y = -str.objScale.x;
          break;
        case 2:
          newBuildRotScale.x = -str.objScale.x;
          newBuildRotScale.y = -str.objScale.y;
          break;
        case 3:
          newBuildRotScale.x = -str.objScale.y;
          newBuildRotScale.y = str.objScale.x;
          break;
      }
      curBuildRotScale = newBuildRotScale;
    }

    List<Tile> GetHoverTiles(Tile hoverT) {
      List<Tile> newTiles = new List<Tile>();
      Vector2 originalTileLoc = hoverT.GetWorldLoc();
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

    void GenerateBuildMenuOptions() {
      float yOffset = 60;
      for(int i = 0; i < structures.Length; i++) {
        Vector3 newLoc = new Vector3(0, 170 - (yOffset * i), 0);
        GameObject newButtonObject = (GameObject)Instantiate(buildMenuButton, newLoc, buildMenuButton.transform.rotation, buildMenuObject.transform.GetChild(0));
        newButtonObject.transform.localPosition = newLoc;
        Button newButt = newButtonObject.GetComponent<Button>();
        newButtonObject.transform.GetChild(0).GetComponent<Text>().text = structures[i].gameObject.GetComponent<WorldDescriptor>().title;
        int index = i;
        newButt.onClick.AddListener(() => ChangeBuildingToBuild(index));
      }
    }

    public void ChangeBuildingToBuild(int i) {
      EndBuildMode();
      structureIndex = i;
      StartBuildMode();
    }

    public void DestroyBuilding() {
      GameObject building = mm.activeObject.gameObject;
      Structure str = building.GetComponent<Structure>();
      for(int i = 0; i < str.tilesOn.Count; i++) {
        str.tilesOn[i].SetCanBuild(true);
      }
      tm.DestroyedStructure(str);
      Destroy(building);
      mm.activeObject = null;
      ui.HideAllActiveObjectWindows();
    }
}
