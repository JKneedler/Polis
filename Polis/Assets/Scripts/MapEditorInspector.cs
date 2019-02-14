using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEditorConverter))]
public class MapEditorInspector : Editor {

  public MapEditorConverter mapData;

  public int selectedSave;

  public override void OnInspectorGUI() {
    EditorGUILayout.BeginHorizontal();
    mapData.mapSize = EditorGUILayout.Vector2Field("Map Size : ", mapData.mapSize);
    EditorGUILayout.EndHorizontal();
    mapData.gridSize = EditorGUILayout.IntField("Grid Size : ", mapData.gridSize);
    mapData.pathToArchives = EditorGUILayout.TextField("Map Saves File Path :", mapData.pathToArchives);
    EditorGUILayout.BeginHorizontal();
    if(GUILayout.Button("Save Maps")) {
      mapData.ArchiveSaves();
    }
    if(GUILayout.Button("Load Maps")) {
      mapData.GetArchivedMaps();
    }
    EditorGUILayout.EndHorizontal();
    mapData.grassTex = (Texture2D)EditorGUILayout.ObjectField("Grass Texture :", mapData.grassTex, typeof(Texture2D), false);
    mapData.waterTex = (Texture2D)EditorGUILayout.ObjectField("Water Texture :", mapData.waterTex, typeof(Texture2D), false);
    EditorGUILayout.BeginHorizontal();
    if(GUILayout.Button("New Map")) {
      mapData.NewMap();
      selectedSave = mapData.savedMaps.Count - 1;
      mapData.curSaveOpen = mapData.savedMaps.Count - 1;
      mapData.LoadMap(selectedSave);
    }
    if(GUILayout.Button("Delete Map") && mapData.savedMaps.Count > 1) {
      mapData.DeleteSavedMap(selectedSave);
      selectedSave = 0;
      mapData.curSaveOpen = 0;
    }
    EditorGUILayout.EndHorizontal();
    if(GUILayout.Button("Save Map")) {
      mapData.SaveCurrentMap(selectedSave);
    }
    for(int i = 0; i < mapData.savedMaps.Count; i++) {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.Toggle("Save : " + i, selectedSave == i);
      if(GUILayout.Button("Select")) {
        selectedSave = i;
        mapData.LoadMap(selectedSave);
      }
      EditorGUILayout.EndHorizontal();
    }
  }

  void OnEnable() {
    mapData = target as MapEditorConverter;
  }

}
