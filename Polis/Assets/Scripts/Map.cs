using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Map : MonoBehaviour {

  public bool useEditorMap;
  private int mapWidth;
  private int mapHeight;
  public int generatedMapWidth;
  public int generatedMapHeight;
  public float minErosionPercent;
  public float maxErosionPercent;
  public float erosionIterations;

  public GameObject clearTile;
  public GameObject[] grassTiles;
  public GameObject[] edgeTiles;
  public float flowerCutoff;

  public GameObject[] treeModels;
  public Transform resourceParent;
  public int maxTrees;
  public int minTrees;
  public float treePerlinScale;
  public float treePerlinCutoff;
  public AnimationCurve treeNumCurve;

  //private bool[,] map; //get rid of this for just the tiles array
  private Tile[] tiles;
  private Tile[] tempTiles;
  private int MAP_CORRECTION_WIDTH;
  private int MAP_CORRECTION_HEIGHT;
  private float offsetX;
  private float offsetY;
  private MapEditorConverter editorMap;

  private Tile[] coastTiles {
    get{
      return tempTiles.Where(t => t.GetAutoTileID() != 15).ToArray();
    }
  }

    // Start is called before the first frame update
    void Start() {
      editorMap = gameObject.GetComponent<MapEditorConverter>();
      offsetX = Random.Range(0f, 100f);
      offsetY = Random.Range(0f, 100f);
      Tile[] newTiles = new Tile[0];
      if(useEditorMap) {
        mapWidth = (int)editorMap.mapSize.x;
        mapHeight = (int)editorMap.mapSize.y;
        MAP_CORRECTION_WIDTH = mapWidth / 2;
        MAP_CORRECTION_HEIGHT = mapHeight / 2;
        editorMap.GetArchivedMaps();
        newTiles = editorMap.GetTilesFromMapEditor();
        SetTileIDs(newTiles);
      } else {
        mapWidth = generatedMapWidth;
        mapHeight = generatedMapHeight;
        MAP_CORRECTION_WIDTH = mapWidth / 2;
        MAP_CORRECTION_HEIGHT = mapWidth / 2;
        newTiles = ProceduralGenerateMapArray(mapWidth, mapHeight);
      }
      CreateMapFromTiles(newTiles);
    }

    // Update is called once per frame
    void Update() {

    }

    public bool GetMapValueFromWorldCoord(int x, int y) {
      Tile tile = tiles[(y + MAP_CORRECTION_HEIGHT) * mapWidth + (x + MAP_CORRECTION_WIDTH)];
      return tile.GetCanBuild();
    }

    public void SetMapValue(int x, int y, bool newValue) {
      Tile tile = tiles[(y + MAP_CORRECTION_HEIGHT) * mapWidth + (x + MAP_CORRECTION_WIDTH)];
      tile.SetCanBuild(newValue);
    }

    public Tile GetTileFromWorldPos(int x, int y) {
      if(x > (-(mapWidth/2) - 1) && x < (mapWidth/2) && y > (-(mapHeight/2) - 1) && y < (mapHeight/2)) {
        int tileIndex = (y + MAP_CORRECTION_HEIGHT) * mapWidth + (x + MAP_CORRECTION_WIDTH);
        return tiles[(y + MAP_CORRECTION_HEIGHT) * mapWidth + (x + MAP_CORRECTION_WIDTH)];
      } else {
        return null;
      }
    }

    public Tile[] ProceduralGenerateMapArray(int width, int height) {
      Tile[] procedTiles = new Tile[width * height];
      for(int x = 0; x < width; x++) {
        for(int y = 0; y < height; y++) {
          Tile tile = new Tile(new Vector2(x, y), 'G', MAP_CORRECTION_WIDTH, MAP_CORRECTION_HEIGHT);
          procedTiles[(mapWidth * y) + x] = tile;
        }
      }
      tempTiles = procedTiles;
      SetTileIDs(tempTiles);
      CreateCoast(procedTiles);
      for(int i = 0; i < procedTiles.Count(); i++) {
        if(procedTiles[i].GetAutoTileID() != -1) {
          Vector2 tilePos = procedTiles[i].GetMapLoc();
          float val = Mathf.PerlinNoise(tilePos.x/treePerlinScale + offsetX, tilePos.y/treePerlinScale + offsetY);
          if(val > treePerlinCutoff) {
            // //Plain biome - maybe add certain texture for this
            // float val2 = Mathf.PerlinNoise(tilePos.x/rockPerlinScale + offsetX, tilePos.y/rockPerlinScale + offsetY);
            // if(val2 <= rockPerlinCutoff) {
            //   procedTiles[i].SetNumResources(Mathf.RoundToInt((val2/rockPerlinCutoff) * (maxRocks - minRocks) + minRocks));
            //   procedTiles[i].SetTypeChar('R');
            // }
          } else {
            //Forest Biome - separate texture for this possibly
            procedTiles[i].SetNumResources(Mathf.RoundToInt(treeNumCurve.Evaluate(1 - (val/treePerlinCutoff)) * (maxTrees - minTrees)));
            procedTiles[i].SetTypeChar('F');
          }
        } else {
          procedTiles[i].SetTypeChar('W');
        }
      }
      return procedTiles;
    }

    public void CreateMapFromTiles(Tile[] newTiles) {
      for(int x = 0; x < mapWidth; x++) {
        for(int y = 0; y < mapHeight; y++) {
          Tile tile = newTiles[(mapWidth * y) + x];
          Vector2 worldLoc2 = tile.GetWorldLoc();
          Vector3 tilePos = new Vector3(worldLoc2.x, 0, worldLoc2.y);
          tilePos = new Vector3(tilePos.x + 0.5f, 0, tilePos.z + 0.5f);
          GameObject tileObj = new GameObject();
          int rotateAmt = Random.Range(0, 4);
          Quaternion tileRot = Quaternion.Euler(0, 90 * rotateAmt, 0);
          char tileType = tile.GetTypeChar();
          int tileAutoID = tile.GetAutoTileID();
          if(tileType == 'G' || tileType == 'F') {
            if(tileAutoID == 15) {
              float flowerPercent = Random.Range(0f, (float)grassTiles.Length);
              int tileNum = 0;
              if(flowerPercent < flowerCutoff) tileNum = 1;
              tileObj = (GameObject)Instantiate(grassTiles[tileNum], tilePos, tileRot);
              if(tileType == 'F') GenerateResourcesOnTile(tile);
            } else {
              tileObj = (GameObject)Instantiate(edgeTiles[tileAutoID], tilePos, edgeTiles[tileAutoID].transform.rotation);
            }
          } else if(tileType == 'W') {
            tileObj = (GameObject)Instantiate(clearTile, tilePos, clearTile.transform.rotation);
            tileObj.SetActive(false);
          }
          tileObj.transform.parent = transform;
          tile.SetTileObject(tileObj);
        }
      }
      tiles = newTiles;
    }

    public void GenerateResourcesOnTile(Tile tile) {
      Vector2 tileWorldPos = tile.GetWorldLoc();
      int treeAmt = tile.GetNumResources();
      if(treeAmt > 0) {
        tile.SetCanBuild(false);
      }
      for(int i = 0; i < treeAmt; i++) {
        //Get Random Position on Tile
        float posX = Random.Range(tileWorldPos.x, tileWorldPos.x + 1);
        float posY = Random.Range(tileWorldPos.y, tileWorldPos.y + 1);
        float randRot = Random.Range(0, 360);
        Quaternion resRot = Quaternion.Euler(0, randRot, 0);
        //Determine Amount of Trees on Tile
        int treeSize = Mathf.CeilToInt(Random.Range(0f, 1f) * ((float)treeAmt/(float)maxTrees) * 3) - 1;
        int treeType = Random.Range(0, 2);
        GameObject tree = (GameObject)Instantiate(treeModels[treeSize + (3*treeType)], new Vector3(posX, 0.05f, posY), resRot);
        tree.transform.parent = resourceParent;
        NaturalResource res = new NaturalResource(Structure.Resources.Tree, tree);
        tile.AddResource(res);
      }
    }

    public void CreateCoast(Tile[] tilesToCoast) {
      for(int it = 0; it < erosionIterations; it++) {
        for(int i = 0; i < coastTiles.Length; i++) {
          Tile tile = coastTiles[i];
          Vector2 tilePos = tile.GetMapLoc();
          float combinedDistFromMiddle = Mathf.Abs(tilePos.x - (mapWidth/2)) + Mathf.Abs(tilePos.y - (mapHeight/2));
          float erosionChance = ((maxErosionPercent - minErosionPercent) * Mathf.Abs(combinedDistFromMiddle/(mapWidth/2 + mapHeight/2) - 0.5f)) + minErosionPercent;
          float rand = Random.Range(0f, 1f);
          if(rand < erosionChance) {
            coastTiles[i].SetAutoTileID(-1);
          }
        }
        SetTileIDs(tilesToCoast);
      }
    }

    public void SetTileIDs(Tile[] newTiles) {
      for(int x = 0; x < mapWidth; x++) {
        for(int y = 0; y < mapHeight; y++) {
          Tile tile = newTiles[(mapWidth * y) + x];
          if(tile.GetAutoTileID() != -1) {
            bool topTile = GetTileBool(x, y - 1, newTiles);
            bool rightTile = GetTileBool(x + 1, y, newTiles);
            bool bottomTile = GetTileBool(x, y + 1, newTiles);
            bool leftTile = GetTileBool(x - 1, y, newTiles);
            if(topTile && bottomTile && rightTile && leftTile) {
              tile.SetAutoTileID(15);
            } else {
              int tileVal = (ToInt(topTile) * 1) + (ToInt(leftTile) * 2) + (ToInt(rightTile) * 4) + (ToInt(bottomTile) * 8);
              tile.SetAutoTileID(tileVal);
            }
          } else {
            tile.SetCanBuild(false);
          }
        }
      }
    }

    public int ToInt(bool b) {
      return b ? 1 : 0;
    }

    private bool GetTileBool(int x, int y, Tile[] newTiles) {
      if(x < 0 || x > (mapWidth - 1) || y < 0 || y > (mapHeight - 1)) {
        return false;
      } else if(newTiles[(mapWidth * y) + x].GetAutoTileID() == -1){
        return false;
      } else {
        return true;
      }
    }
}

public class Tile {

  public int autotileID;
  private Vector2 mapLoc;
  private char type;
  private int numResources;
  private GameObject tileObject;
  private int MAP_CORRECTION_WIDTH;
  private int MAP_CORRECTION_HEIGHT;
  private bool canBuild;
  private List<NaturalResource> resources;
  private Material originalMat;

  public Tile(Vector2 mapLoc, char type, int MAP_CORRECTION_WIDTH, int MAP_CORRECTION_HEIGHT) {
    this.mapLoc = mapLoc;
    this.type = type;
    this.MAP_CORRECTION_WIDTH = MAP_CORRECTION_WIDTH;
    this.MAP_CORRECTION_HEIGHT = MAP_CORRECTION_HEIGHT;
    resources = new List<NaturalResource>();
  }

  public char GetTypeChar() {
    return type;
  }

  public void SetTypeChar(char type) {
    this.type = type;
  }

  public Vector2 GetMapLoc() {
    return mapLoc;
  }

  public Vector2 GetWorldLoc() {
    return new Vector2(mapLoc.x - MAP_CORRECTION_WIDTH, mapLoc.y - MAP_CORRECTION_HEIGHT);
  }

  public GameObject GetTileObject() {
    return tileObject;
  }

  public void SetTileObject(GameObject tileObj) {
    tileObject = tileObj;
    originalMat = tileObj.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
  }

  public void ChangeMaterial(Material mat) {
    tileObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = mat;
    if(autotileID == -1) {
      tileObject.SetActive(true);
    }
  }

  public void SetMaterialBackToOriginal() {
    tileObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = originalMat;
    if(autotileID == -1) {
      tileObject.SetActive(false);
    }
  }

  public int GetAutoTileID() {
    return autotileID;
  }

  public void SetAutoTileID(int id) {
    autotileID = id;
    DetermineBuildStatus();
  }

  private void DetermineBuildStatus() {
    if(autotileID == 15) {
      canBuild = true;
    } else {
      canBuild = false;
    }
  }

  public bool GetCanBuild() {
    return canBuild;
  }

  public void SetCanBuild(bool newVal) {
    canBuild = newVal;
  }

  public void AddResource(NaturalResource res) {
    resources.Add(res);
  }

  public void RemoveResource(NaturalResource res) {
    resources.Remove(res);
  }

  public List<NaturalResource> GetResourcesList() {
    return resources;
  }

  public void SetNumResources(int amt) {
    numResources = amt;
  }

  public int GetNumResources() {
    return numResources;
  }
}
