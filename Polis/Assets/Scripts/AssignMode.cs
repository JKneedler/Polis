using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignMode : MonoBehaviour {

  private Villager villToAssign;
  private Structure farmToAssign;
  public Discipline discToAssign;
  private FarmData fd;
  public enum AssignModeTypes {VillagerToBuilding, TileToDiscipline};
  public AssignModeTypes assignMode;
  public Material blueMat;
  public Material greenMat;
  public Material redMat;
  public MasterManager mm;
  private Tile curHoverTile;

    // Start is called before the first frame update
    void Start() {

    }

    public void StartVillagerAssignMode(Villager vill) {
      villToAssign = vill;
      assignMode = AssignModeTypes.VillagerToBuilding;
    }

    public void StartTileAssignMode(Discipline disc) {
      discToAssign = disc;
      assignMode = AssignModeTypes.TileToDiscipline;
      for(int i = 0; i < disc.assignedTiles.Count; i++) {
        disc.assignedTiles[i].ChangeMaterial(blueMat);
      }
    }

    public void UpdateLandAssign(Tile newHoverTile) {
      if(newHoverTile != curHoverTile) {
        if(curHoverTile != null && !discToAssign.assignedTiles.Contains(curHoverTile)) curHoverTile.SetMaterialBackToOriginal();
        curHoverTile = newHoverTile;
        if(!discToAssign.assignedTiles.Contains(curHoverTile)) {
          if(discToAssign.CanAssignTile(curHoverTile)) {
            curHoverTile.ChangeMaterial(greenMat);
          } else {
            curHoverTile.ChangeMaterial(redMat);
          }
        }
      }
    }

    public void EndAssignMode() {
      if(assignMode == AssignModeTypes.TileToDiscipline) {
        if(curHoverTile != null) curHoverTile.SetMaterialBackToOriginal();
        for(int i = 0; i < discToAssign.assignedTiles.Count; i++) {
          discToAssign.assignedTiles[i].SetMaterialBackToOriginal();
        }
      }
    }

    public void AttemptToAssign(WorldDescriptor wd) {
      if(assignMode == AssignModeTypes.VillagerToBuilding && wd) {
        if(wd.gameObject.GetComponent<Structure>()) {
          Structure str = wd.gameObject.GetComponent<Structure>();
      		if(str.CanAssignVillager()) {
            // if(villToAssign.curJob != Villager.Jobs.Citizen) {
            //   villToAssign.curStructure.UnassignVillager(villToAssign);
            // }
            str.AssignVillager(villToAssign);
            mm.ChangeCurrentMode(MasterManager.Mode.Run, -1);
          }
        }
      } else if(assignMode == AssignModeTypes.TileToDiscipline) {
        if(discToAssign.assignedTiles.Contains(curHoverTile)) {
          discToAssign.RemoveAssignedTile(curHoverTile);
          curHoverTile.ChangeMaterial(greenMat);
          if(!discToAssign.assignedTilesCanBuild) {
            curHoverTile.SetCanBuild(true);
          }
        } else if(discToAssign.CanAssignTile(curHoverTile)){
          if(!discToAssign.assignedTilesCanBuild) {
            curHoverTile.SetCanBuild(false);
          }
          curHoverTile.ChangeMaterial(blueMat);
          discToAssign.AddAssignedTile(curHoverTile);
        }
      }
    }
}
