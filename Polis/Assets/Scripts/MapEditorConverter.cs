using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapEditorConverter : MonoBehaviour {

  public char[,] tiles;
  public Vector2 mapSize;
  public Texture2D grassTex;
  public Texture2D waterTex;
  public int gridSize;
  public List<MapEditorSave> savedMaps = new List<MapEditorSave>();
  public string pathToArchives;
  public int curSaveOpen;

  public void NewMap() {
    char[,] newTiles = new char[(int)mapSize.x, (int)mapSize.y];
    for(int x = 0; x < (int)mapSize.x; x++) {
      for(int y = 0; y < (int)mapSize.y; y++) {
        newTiles[x, y] = 'G';
      }
    }
    savedMaps.Add(new MapEditorSave(mapSize, newTiles));
  }

  public void ChangeMapTile(int x, int y) {
    tiles[x, y] = 'W';
  }

  public void SaveCurrentMap(int index) {
    savedMaps[index] = new MapEditorSave(mapSize, tiles);
  }

  public void LoadMap(int index) {
    mapSize = savedMaps[index].mapSize;
    curSaveOpen = index;
    tiles = savedMaps[index].Get2DArrayFromSavedTiles();
  }

  public void DeleteSavedMap(int index) {
    savedMaps.Remove(savedMaps[index]);
  }

  public void GetArchivedMaps() {
    StreamReader reader = new StreamReader(pathToArchives);
    string line;
    savedMaps = new List<MapEditorSave>();
    while((line = reader.ReadLine()) != null) {
      MapEditorSave newSave = JsonUtility.FromJson<MapEditorSave>(line);
      savedMaps.Add(newSave);
    }
    reader.Close();
    LoadMap(0);
  }

  public void ArchiveSaves() {
    StreamWriter writer = new StreamWriter(pathToArchives);
    for(int i = 0; i < savedMaps.Count; i++) {
      string json = JsonUtility.ToJson(savedMaps[i]);
      writer.WriteLine(json);
    }
    writer.Close();
  }

  public Tile[] GetTilesFromMapEditor() {
    return savedMaps[curSaveOpen].GetTilesArray();
  }

}

public class MapEditorSave {
  public Vector2 mapSize;
  public char[] savedTiles;
  public int[] resourceNums;

  public MapEditorSave(Vector2 mapSize, char[,] savedTiles) {
    this.mapSize = mapSize;
    this.savedTiles = new char[(int)mapSize.x * (int)mapSize.y];
    this.resourceNums = new int[this.savedTiles.Length];
    for(int x = 0; x < mapSize.x; x++) {
      for(int y = 0; y < mapSize.y; y++) {
        this.savedTiles[(y * (int)mapSize.x) + x] = savedTiles[x, y];
        this.resourceNums[(y * (int)mapSize.x) + x] = 0;
      }
    }
  }

  public char[,] Get2DArrayFromSavedTiles() {
    char[,] tiles = new char[(int)mapSize.x, (int)mapSize.y];
    for(int x = 0; x < (int)mapSize.x; x++) {
      for(int y = 0; y < (int)mapSize.y; y++) {
        tiles[x, y] = savedTiles[(y * (int)mapSize.x) + x];
      }
    }
    return tiles;
  }

  public Tile[] GetTilesArray() {
    Tile[] newTiles = new Tile[(int)mapSize.x * (int)mapSize.y];
    for(int x = 0; x < (int)mapSize.x; x++) {
      for(int y = 0; y < (int)mapSize.y; y++) {
        char tileChar = savedTiles[((int)mapSize.x * y) + x];
        Tile tile = new Tile(new Vector2(x, y), tileChar, (int)mapSize.x / 2, (int)mapSize.y / 2);
        tile.SetNumResources(resourceNums[((int)mapSize.x * y) + x]);
        if(tileChar == 'W') {
          tile.SetAutoTileID(-1);
        } else {
          tile.SetAutoTileID(15);
        }
        newTiles[((int)mapSize.x * y) + x] = tile;
      }
    }
    return newTiles;
  }

}
