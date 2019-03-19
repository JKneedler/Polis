using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingSelectorUI : MonoBehaviour {

  public GameObject selectorPanel;
  RectTransform selPanelTransform;
  public GameObject villagerBar;
  public GameObject villagerContent;
  public bool tileSelected;
  public Vector3 targetPos;
  public GeneralBuildingMenu generalBM;

  public TownManager tm;

    // Start is called before the first frame update
    void Start() {
      selPanelTransform = selectorPanel.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
      if(tileSelected) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);
        selectorPanel.transform.position = screenPos;
      }
    }

    public void ShowSelectorPanel(BuildableTile bTile) {
      for(int i = 0; i < villagerContent.transform.childCount; i++) {
        Destroy(villagerContent.transform.GetChild(i).gameObject);
      }
      List<Villager> possibleWorkers = tm.GetPossibleWorkers(bTile.GetRequiredJobType());
      for(int i = 0; i < possibleWorkers.Count; i++) {
        GameObject vBar = (GameObject)Instantiate(villagerBar, transform.position, transform.rotation);
        vBar.transform.SetParent(villagerContent.transform);
        vBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = possibleWorkers[i].villagerName;
        Villager vill = possibleWorkers[i];
        vBar.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => tm.AssignVillagerToTile(vill, bTile));
      }
      villagerContent.GetComponent<LayoutGroup>().SetLayoutVertical();
      LayoutRebuilder.ForceRebuildLayoutImmediate(villagerContent.GetComponent<RectTransform>());
      Vector2 rotatedSize = bTile.GetRotatedScale();
      float x = (rotatedSize.x - 1) / 2;
      if(rotatedSize.x < 0) x = (rotatedSize.x + 1) / 2;
      float y = (rotatedSize.y - 1) / 2;
      if(rotatedSize.y < 0) y = (rotatedSize.y + 1) / 2;
      Vector3 bTilePos = bTile.GetTileObj().transform.position;
      Vector3 middlePos = new Vector3(bTilePos.x + x, 0, bTilePos.z + y);
      targetPos = middlePos;
      Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);
      selectorPanel.transform.position = screenPos;
      selectorPanel.SetActive(true);
      tileSelected = true;
    }

    public void HideSelectorPanel() {
      selectorPanel.SetActive(false);
      tileSelected = false;
    }
}
