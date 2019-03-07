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
  private int MAP_CORRECTION_WIDTH;
  private int MAP_CORRECTION_HEIGHT;
  private float offsetX;
  private float offsetY;
  private MapEditorConverter editorMap;

  public class TempTile {
    public Vector2 loc;
    public char type;
    public int autotileID;
    public int numRes;

    public TempTile(Vector2 loc, char type) {
      this.loc = loc;
      this.type = type;
      autotileID = 15;
      numRes = 0;
    }
  }

  // private Tile[] coastTiles {
  //   get{
  //     return tempTiles.Where(t => t.GetAutoTileID() != 15).ToArray();
  //   }
  // }

    // Start is called before the first frame update
    void Start() {
      editorMap = gameObject.GetComponent<MapEditorConverter>();
      offsetX = Random.Range(0f, 100f);
      offsetY = Random.Range(0f, 100f);
      TempTile[] newTiles = new TempTile[0];
      if(useEditorMap) {
        mapWidth = (int)editorMap.mapSize.x;
        mapHeight = (int)editorMap.mapSize.y;
        MAP_CORRECTION_WIDTH = mapWidth / 2;
        MAP_CORRECTION_HEIGHT = mapHeight / 2;
        editorMap.GetArchivedMaps();
      //  newTiles = editorMap.GetTilesFromMapEditor();
      //  SetTileIDs(newTiles);
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
      //Get rid of this method once building is fixed
      return true;
    }

    public void SetMapValue(int x, int y, bool newValue) {
      //Get rid of this method
    }

    public Tile GetTileFromWorldPos(int x, int y) {
      if(x > (-(mapWidth/2) - 1) && x < (mapWidth/2) && y > (-(mapHeight/2) - 1) && y < (mapHeight/2)) {
        int tileIndex = (y + MAP_CORRECTION_HEIGHT) * mapWidth + (x + MAP_CORRECTION_WIDTH);
        return tiles[(y + MAP_CORRECTION_HEIGHT) * mapWidth + (x + MAP_CORRECTION_WIDTH)];
      } else {
        return null;
      }
    }

    public TempTile[] ProceduralGenerateMapArray(int width, int height) {
      // Tile[] procedTiles = new Tile[width * height];
      TempTile[] procedTempTiles = new TempTile[width * height];
      for(int x = 0; x < width; x++) {
        for(int y = 0; y < height; y++) {
          // Tile tile = new Tile(new Vector2(x, y), 'G', MAP_CORRECTION_WIDTH, MAP_CORRECTION_HEIGHT);
          TempTile tempTile = new TempTile(new Vector2(x, y), 'G');
          procedTempTiles[(mapWidth * y) + x] = tempTile;
        }
      }
      SetTileIDs(procedTempTiles);
      CreateCoast(procedTempTiles);
      for(int i = 0; i < procedTempTiles.Count(); i++) {
        if(procedTempTiles[i].autotileID != -1) {
          Vector2 tilePos = procedTempTiles[i].loc;
          float val = Mathf.PerlinNoise(tilePos.x/treePerlinScale + offsetX, tilePos.y/treePerlinScale + offsetY);
          if(val > treePerlinCutoff) {
            // Other Biome if I decide to put one in
          } else {
            //Forest Biome - separate texture for this possibly
            procedTempTiles[i].numRes = minTrees + Mathf.RoundToInt(treeNumCurve.Evaluate(1 - (val/treePerlinCutoff)) * (maxTrees - minTrees));
            procedTempTiles[i].type = 'F';
          }
        } else {
          procedTempTiles[i].type = 'W';
        }
      }
      return procedTempTiles;
    }

    public Vector2 GetTileWorldLoc(TempTile tile) {
      return new Vector2(tile.loc.x - MAP_CORRECTION_WIDTH, tile.loc.y - MAP_CORRECTION_HEIGHT);
    }

    public void CreateMapFromTiles(TempTile[] tempTiles) {
      Tile[] newTiles = new Tile[tempTiles.Length];
      for(int x = 0; x < mapWidth; x++) {
        for(int y = 0; y < mapHeight; y++) {
          TempTile tile = tempTiles[(mapWidth * y) + x];
          Vector2 worldLoc2 = GetTileWorldLoc(tile);
          Vector3 worldLoc3 = new Vector3(worldLoc2.x, 0, worldLoc2.y);
          Vector3 tilePos = new Vector3(worldLoc2.x + 0.5f, 0, worldLoc2.y + 0.5f);
          int rotateAmt = Random.Range(0, 4);
          Quaternion tileRot = Quaternion.Euler(0, 90 * rotateAmt, 0);
          char tileType = tile.type;
          int tileAutoID = tile.autotileID;
          if(tileType == 'F' && tileAutoID == 15) { // Forest Biome
            GameObject tileObj = (GameObject)Instantiate(grassTiles[0], tilePos, tileRot);
            tileObj.transform.parent = transform;
            List<TreeResource> trees = GenerateResources(tile, worldLoc2);
            ForestTile fT = new ForestTile(tile.loc, worldLoc3, 'F', tileObj, trees);
            newTiles[(mapWidth * y) + x] = fT;
          } else if(tileAutoID > -1) { // Grass and Coast Biome
            if(tileAutoID == 15) {
              float flowerPercent = Random.Range(0f, (float)grassTiles.Length);
              int tileNum = 0;
              if(flowerPercent < flowerCutoff) tileNum = 1;
              GameObject tileObj = (GameObject)Instantiate(grassTiles[tileNum], tilePos, tileRot);
              tileObj.transform.parent = transform;
              GrassTile gT = new GrassTile(tile.loc, worldLoc3, 'G', tileObj);
              newTiles[(mapWidth * y) + x] = gT;
            } else { // Ocean Biome
              GameObject tileObj = (GameObject)Instantiate(edgeTiles[tileAutoID], tilePos, edgeTiles[tileAutoID].transform.rotation);
              tileObj.transform.parent = transform;
              CoastTile cT = new CoastTile(tile.loc, worldLoc3, 'G', tileObj);
              newTiles[(mapWidth * y) + x] = cT;
            }
          } else if(tileType == 'W') {
            GameObject tileObj = (GameObject)Instantiate(clearTile, tilePos, clearTile.transform.rotation);
            tileObj.transform.parent = transform;
            tileObj.SetActive(false);
            OceanTile oT = new OceanTile(tile.loc, worldLoc3, 'W', tileObj);
            newTiles[(mapWidth * y) + x] = oT;
          }
        }
      }
      tiles = newTiles;
    }

    public List<TreeResource> GenerateResources(TempTile tile, Vector2 tileWorldPos) {
      int treeAmt = tile.numRes;
      List<TreeResource> trees = new List<TreeResource>();
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
        TreeResource tree = treeObj.GetComponent<TreeResource>();
        trees.Add(tree);
      }
      return trees;
    }

    private List<TempTile> FindCoastTiles(TempTile[] mapTiles) {
      List<TempTile> coastTiles = new List<TempTile>();
      for(int i = 0; i < mapTiles.Length; i++) {
        if(mapTiles[i].autotileID != 15 && mapTiles[i].autotileID != -1) {
          coastTiles.Add(mapTiles[i]);
        }
      }
      return coastTiles;
    }

    public void CreateCoast(TempTile[] allTiles) {
      for(int it = 0; it < erosionIterations; it++) {
        List<TempTile> coastTiles = FindCoastTiles(allTiles);
        for(int i = 0; i < coastTiles.Count; i++) {
          TempTile tile = coastTiles[i];
          Vector2 tilePos = tile.loc;
          float combinedDistFromMiddle = Mathf.Abs(tilePos.x - (mapWidth/2)) + Mathf.Abs(tilePos.y - (mapHeight/2));
          float erosionChance = ((maxErosionPercent - minErosionPercent) * Mathf.Abs(combinedDistFromMiddle/(mapWidth/2 + mapHeight/2) - 0.5f)) + minErosionPercent;
          float rand = Random.Range(0f, 1f);
          if(rand < erosionChance) {
            coastTiles[i].autotileID = -1;
          }
        }
        SetTileIDs(allTiles);
      }
    }

    public void SetTileIDs(TempTile[] allTiles) {
      for(int x = 0; x < mapWidth; x++) {
        for(int y = 0; y < mapHeight; y++) {
          TempTile tile = allTiles[(mapWidth * y) + x];
          if(tile.autotileID != -1) {
            bool topTile = GetTileBool(x, y - 1, allTiles);
            bool rightTile = GetTileBool(x + 1, y, allTiles);
            bool bottomTile = GetTileBool(x, y + 1, allTiles);
            bool leftTile = GetTileBool(x - 1, y, allTiles);
            if(topTile && bottomTile && rightTile && leftTile) {
              tile.autotileID = 15;
            } else {
              int tileVal = (ToInt(topTile) * 1) + (ToInt(leftTile) * 2) + (ToInt(rightTile) * 4) + (ToInt(bottomTile) * 8);
              tile.autotileID = tileVal;
            }
          }
        }
      }
    }

    public int ToInt(bool b) {
      return b ? 1 : 0;
    }

    private bool GetTileBool(int x, int y, TempTile[] newTiles) {
      if(x < 0 || x > (mapWidth - 1) || y < 0 || y > (mapHeight - 1)) {
        return false;
      } else if(newTiles[(mapWidth * y) + x].autotileID == -1){
        return false;
      } else {
        return true;
      }
    }
}
