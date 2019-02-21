using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterManager : MonoBehaviour {

  public Vector3 mousePos;
  public GameObject mouseOverObject;
  private WorldDescriptor mouseOverScript;
  public WorldDescriptor activeObject;
  public Map map;
  public enum Mode {Run, Build, Assign};
  public Mode curMode = Mode.Run;
  public int timeMultiplier;
  public GameObject canvas;
  private UIManager ui;
  private BuildMode buildMode;
  private AssignMode assignMode;
  private TownManager tm;
  private bool showObjectInfo;
  private Tile hoverTile;
  public int mouseLocLayerMask = 1 << 9;

    // Start is called before the first frame update
    void Start() {
      showObjectInfo = true;
      ui = canvas.GetComponent<UIManager>();
      buildMode = gameObject.GetComponent<BuildMode>();
      assignMode = gameObject.GetComponent<AssignMode>();
      tm = gameObject.GetComponent<TownManager>();
    }

    // Update is called once per frame
    void Update() {
      SetMousePosition();

      if(curMode == Mode.Run) {

        if(Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {
          if(mouseOverScript) {
            SetActiveObject(mouseOverScript);
          }
        }
      } else if(curMode == Mode.Build) {
        buildMode.Build(mousePos, hoverTile);
        if(Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {
          buildMode.AttemptToBuild();
        }
      } else if(curMode == Mode.Assign) {
        if(assignMode.assignMode == AssignMode.AssignModeTypes.TileToDiscipline) {
          if(hoverTile != null)  assignMode.UpdateLandAssign(hoverTile);
        }
        if(Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {
          assignMode.AttemptToAssign(mouseOverScript);
        }
      }

      if(showObjectInfo) {
        if(mouseOverObject && mouseOverObject.GetComponent<WorldDescriptor>()) {
          WorldDescriptor wd = mouseOverObject.GetComponent<WorldDescriptor>();
          mouseOverScript = wd;
          ui.DisplayInfoPanel(wd);
        } else {
          ui.HideInfoPanel();
          mouseOverScript = null;
        }
      }

      if(Input.GetKey(KeyCode.Escape)) {
        PressedNewMode(0);
        ui.HideAllActiveObjectWindows();
        activeObject = null;
      }
    }

    void SetMousePosition(){
      Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      Ray ray = Camera.main.ScreenPointToRay(mouse);
      RaycastHit hit;

      if(Physics.Raycast(ray, out hit)) {
        mousePos = hit.point;
        mouseOverObject = hit.collider.gameObject;
      }

      if(Physics.Raycast(ray, out hit, 100f, mouseLocLayerMask)) {
        int posX = Mathf.FloorToInt(hit.point.x);
        int posY = Mathf.FloorToInt(hit.point.z);
        hoverTile = map.GetTileFromWorldPos(posX, posY);
      }
    }

    public void SetActiveObject(WorldDescriptor newActive) {
      ui.HideAllActiveObjectWindows();
      activeObject = newActive;
      switch (activeObject.objectType) {
        case WorldDescriptor.objectTypes.Villager:
          ui.DisplayVillagerWindow(activeObject.gameObject.GetComponent<Villager>());
          break;
        case WorldDescriptor.objectTypes.Building:
          ui.DisplayBuildingWindow(activeObject.gameObject.GetComponent<Structure>());
          break;
        case WorldDescriptor.objectTypes.Resource:
          break;
        default:
          break;
      }
    }

    public void PressedNewMode(int modeNum) {
      ChangeCurrentMode((Mode)modeNum, -1);
    }

    public void StartAssigningForDiscipline(int discNum) {
      ChangeCurrentMode(Mode.Assign, discNum);
    }

    public void ChangeCurrentMode(Mode newMode, int discNum) {
      if(curMode == Mode.Build) {
        showObjectInfo = true;
        buildMode.EndBuildMode();
      } else if(curMode == Mode.Assign) {
        assignMode.EndAssignMode();
      }

      if(newMode == Mode.Build) {
        buildMode.StartBuildMode();
        showObjectInfo = false;
      } else if(newMode == Mode.Assign) {
        if(discNum == -1) {
          if(activeObject.objectType == WorldDescriptor.objectTypes.Villager) {
            assignMode.StartVillagerAssignMode(activeObject.gameObject.GetComponent<Villager>());
          }
        } else {
          assignMode.StartTileAssignMode(tm.GetDisciplineFromIndex(discNum));
        }
      }

      curMode = newMode;
    }

}
