using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditorWindow : EditorWindow {

  [MenuItem("Window/Map Editor")]
  public static void OpenMapEditorWindow() {
    EditorWindow window = EditorWindow.GetWindow(typeof(MapEditorWindow));
    GUIContent title = new GUIContent();
    title.text = "Map Editor";
    window.titleContent = title;
  }

  void OnGUI() {
    if(Selection.activeGameObject == null) {
      return;
    }
    MapEditorConverter data = Selection.activeGameObject.GetComponent<MapEditorConverter>();

    if(data != null) {
      GUILayout.BeginHorizontal();
      Vector2 screenPos = Event.current.mousePosition;
      for(int x = 0; x < data.mapSize.x; x++) {
        GUILayout.BeginVertical();
        for(int y = 0; y < data.mapSize.y; y++) {
          Rect rect = new Rect(x * data.gridSize, y * data.gridSize, data.gridSize, data.gridSize);
          GUILayout.BeginArea(rect);
          GUIStyle style = new GUIStyle();
          if(data.tiles[x, y] == 'G') {
            style.normal.background = data.grassTex;
          } else if(data.tiles[x, y] == 'W') {
            style.normal.background = data.waterTex;
          }
          if(GUILayout.Button("", style, GUILayout.Width(data.gridSize), GUILayout.Height(data.gridSize), GUILayout.ExpandWidth(false))) {
            data.ChangeMapTile(x, y);
          }
          GUILayout.EndArea();
        }
        GUILayout.EndVertical();
      }
      GUILayout.EndHorizontal();
    }
  }

}
