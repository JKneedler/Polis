using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterManager : MonoBehaviour {

  private Villager mouseOverVillager;
  private Tile mouseOverTile;
  public enum Mode {Run, Build};
  public Mode curMode = Mode.Run;
  public int mouseLocLayerMask = 1 << 9;

  // References
  public Map map;
  public GameObject canvas;
  private UIManager ui;
  private BuildMode buildMode;
  private TownManager tm;

  // Clock
  public int year;
  public int weekInYear;
  public float weekProgress;
  public float timeScale;

  public MenuNode[] menuTree;

    // Start is called before the first frame update
    void Start() {
      ui = canvas.GetComponent<UIManager>();
      buildMode = gameObject.GetComponent<BuildMode>();
      // assignMode = gameObject.GetComponent<AssignMode>();
      tm = gameObject.GetComponent<TownManager>();
    }

    // Update is called once per frame
    void Update() {
      SetMousePosition();
      AdjustClock();

      if(curMode == Mode.Run) {
        if(Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {
        }
      } else if(curMode == Mode.Build) {
        buildMode.Build(mouseOverTile);
        if(Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {
          buildMode.AttemptToBuild();
        }
      }

      if(Input.GetKey(KeyCode.Escape)) {
        StartRunMode();
        ui.HideAllActiveObjectWindows();
      }
    }

    void AdjustClock() {
      weekProgress += (Time.deltaTime * timeScale);
      if(weekProgress >= 100) {
        weekProgress = 0;
        weekInYear++;
        if(weekInYear == 53) {
          weekInYear = 1;
          year++;
        }
      }
      ui.SetClockUI(year, weekInYear, weekProgress);
    }

    void SetMousePosition(){
      Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      Ray ray = Camera.main.ScreenPointToRay(mouse);
      RaycastHit hit;

      if(Physics.Raycast(ray, out hit)) {
        if(hit.collider.gameObject.GetComponent<Villager>()) {
          mouseOverVillager = hit.collider.gameObject.GetComponent<Villager>();
        }
      }

      if(Physics.Raycast(ray, out hit, 100f, mouseLocLayerMask)) {
        int posX = Mathf.FloorToInt(hit.point.x);
        int posY = Mathf.FloorToInt(hit.point.z);
        mouseOverTile = map.GetTileFromWorldPos(posX, posY);
      }
    }

    public void StartRunMode() {
      if(curMode == Mode.Build) {
        buildMode.EndBuildMode();
      }
      curMode = Mode.Run;
    }

    public void StartBuildModeForTile(BuildableTile buildTile) {
      if(curMode == Mode.Build) {
        buildMode.EndBuildMode();
        if(buildTile != buildMode.tileToBuild) {
          buildMode.StartBuildMode(buildTile);
          curMode = Mode.Build;
        }
      } else {
        buildMode.StartBuildMode(buildTile);
        curMode = Mode.Build;
      }
    }

    // public void ChangeTab(int tabNum) {
    //   Tab newTab = (Tab)tabNum;
    //   ui.HideAllTabWindows();
    //   if(newTab != curTab) {
    //     if(newTab == Tab.Tasks) {
    //       ui.DisplayTasksWindow();
    //     } else if(newTab == Tab.TownInfo) {
    //       ui.DisplayJobsWindow();
    //       ui.DisplayResourcesWindow();
    //     } else if(newTab == Tab.Build) {
    //       ui.HideAllTabWindows();
    //       ChangeCurrentMode(Mode.Build, -1);
    //     }
    //     curTab = newTab;
    //   } else {
    //     curTab = Tab.None;
    //   }
    // }
}
