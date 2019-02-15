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
            // Other Biome if I decide to put one in
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
          GameObject tileObj;
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
              tileObj.transform.parent = transform;
              tile.SetTileObject(tileObj);
              if(tileType == 'F') GenerateResourcesOnTile(tile);
            } else {
              tileObj = (GameObject)Instantiate(edgeTiles[tileAutoID], tilePos, edgeTiles[tileAutoID].transform.rotation);
              tileObj.transform.parent = transform;
              tile.SetTileObject(tileObj);
            }
          } else if(tileType == 'W') {
            tileObj = (GameObject)Instantiate(clearTile, tilePos, clearTile.transform.rotation);
            tileObj.transform.parent = transform;
            tile.SetTileObject(tileObj);
            tileObj.SetActive(false);
          }
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
        GameObject treeObj = (GameObject)Instantiate(treeModels[treeSize + (3*treeType)], new Vector3(posX, 0.05f, posY), resRot);
        treeObj.transform.parent = resourceParent;
        TreeResource tree = new TreeResource(treeObj);
        tile.AddResource(tree);
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
