using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

  public TextMeshProUGUI foodText;
  public TextMeshProUGUI woodText;
  public TextMeshProUGUI stoneText;
  public TextMeshProUGUI drachmaText;

  public TextMeshProUGUI yearText;
  public TextMeshProUGUI weekText;
  public Slider weekProgressSlider;

  public MenuNode[] menuArray;
  public GameObject buildMenuContainer;
  public GameObject buildMenuLevel;
  public GameObject buildMenuItem;
  public UIMenuLevel curMenuNode;
  public int curLevelInt;

  public GameObject villagerPanel;

  public GameObject resourcesPanel;
  public GameObject resourcesContainer;
  public GameObject resourceBarPre;
  public GameObject resourceElementPre;
  public Dictionary<Resource, GameObject> resElements;

  // References
  public MasterManager mm;
  BuildingSelectorUI bsUI;


  public class UIMenuLevel {
    public UIMenuLevel parent;
    public GameObject level;
    public List<UIMenuLevel> children;

    public UIMenuLevel(UIMenuLevel parent, GameObject level) {
      this.parent = parent;
      this.level = level;
      children = new List<UIMenuLevel>();
    }

    public UIMenuLevel(GameObject level) {
      this.parent = null;
      this.level = level;
      children = new List<UIMenuLevel>();
    }
  }


    // Start is called before the first frame update
    void Start() {
      bsUI = gameObject.GetComponent<BuildingSelectorUI>();
      CreateMenuTree();
    }

    // Update is called once per frame
    void Update() {

    }

    public void CreateMenuTree() {
      int curLevelNum = 1;
      GameObject newLevel = (GameObject)Instantiate(buildMenuLevel, transform.position, transform.rotation, buildMenuContainer.transform);
      UIMenuLevel curLevel = new UIMenuLevel(newLevel);
      UIMenuLevel rootLevel = curLevel;
      for(int i = 0; i < menuArray.Length; i++) {
        MenuNode item = menuArray[i];
        UIMenuLevel nextLevel = curLevel;
        if(item.level > curLevelNum) {
          GameObject newLevelObj = (GameObject)Instantiate(buildMenuLevel, transform.position, transform.rotation, buildMenuContainer.transform);
          UIMenuLevel newNode = new UIMenuLevel(curLevel, newLevelObj);
          curLevel.children.Add(newNode);
          nextLevel = newNode;
        } else if(item.level < curLevelNum) {
          nextLevel = curLevel;
          for(int k = 0; k < (curLevelNum - item.level); k++) {
            nextLevel = nextLevel.parent;
          }
        }

        GameObject newButton = (GameObject)Instantiate(buildMenuItem, transform.position, transform.rotation);
        newButton.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = item.icon;
        newButton.transform.SetParent(nextLevel.level.transform, false);
        Button butt = newButton.GetComponent<Button>();
        if(item.isLeaf) {
          BuildableTile bTile = item.building.GetBuildableTile();
          butt.onClick.AddListener(() => ClickedLeafNode(bTile));
        } else {
          butt.onClick.AddListener(() => ClickedNonLeafNode(newButton.transform.GetSiblingIndex(), item.level));
        }
        curLevel = nextLevel;
        curLevelNum = item.level;
      }
      buildMenuContainer.GetComponent<LayoutGroup>().SetLayoutHorizontal();
      LayoutRebuilder.ForceRebuildLayoutImmediate(buildMenuContainer.GetComponent<RectTransform>());
      for(int i = 1; i < buildMenuContainer.transform.childCount; i++) {
        buildMenuContainer.transform.GetChild(i).gameObject.SetActive(false);
      }
      buildMenuContainer.SetActive(false);
      curMenuNode = rootLevel;
      curLevelInt = 1;
    }

    public void ClickedNonLeafNode(int childNum, int level) {
      mm.StartRunMode();
      if(level < curLevelInt) {
        for(int i = 0; i < (curLevelInt - level); i++) {
          curMenuNode.level.SetActive(false);
          curMenuNode = curMenuNode.parent;
        }
        curLevelInt -= (curLevelInt - level);
      }
      curMenuNode = curMenuNode.children[childNum];
      curMenuNode.level.SetActive(true);
      curLevelInt++;
    }

    public void ClickedLeafNode(BuildableTile buildTile) {
      mm.StartBuildModeForTile(buildTile);
    }

    public void ShowBuildMenu() {
      if(buildMenuContainer.activeSelf) {
        HideBuildMenu();
      } else {
        HideVillagerPanel();
        buildMenuContainer.SetActive(true);
        bsUI.HideSelectorPanel();
      }
    }

    public void HideBuildMenu() {
      for(int i = 1; i < buildMenuContainer.transform.childCount; i++) {
        buildMenuContainer.transform.GetChild(i).gameObject.SetActive(false);
      }
      while(curMenuNode.parent != null) {
        curMenuNode = curMenuNode.parent;
      }
      buildMenuContainer.SetActive(false);
      curLevelInt = 1;
    }

    public void ShowVillagerPanel() {
      if(villagerPanel.activeSelf) {
        HideVillagerPanel();
      } else {
        HideBuildMenu();
        villagerPanel.SetActive(true);
        bsUI.HideSelectorPanel();
      }
    }

    public void HideVillagerPanel() {
      villagerPanel.SetActive(false);
    }

    public void ShowResourcesPanel() {
      resourcesPanel.SetActive(!resourcesPanel.activeSelf);
    }

    public void InitializeResourcesPanel(List<ResourceStorage> res) {
      resElements = new Dictionary<Resource, GameObject>();
      for(int i = 0; i < res.Count; i++) {
        NewResource(res[i]);
      }
      resourcesContainer.GetComponent<LayoutGroup>().SetLayoutVertical();
      LayoutRebuilder.ForceRebuildLayoutImmediate(resourcesContainer.GetComponent<RectTransform>());
      resourcesPanel.GetComponent<LayoutGroup>().SetLayoutVertical();
      LayoutRebuilder.ForceRebuildLayoutImmediate(resourcesPanel.GetComponent<RectTransform>());
    }

    public void NewResource(ResourceStorage newRes) {
      int subType = (int)newRes.res.GetSubType();
      Transform typeCont = resourcesContainer.transform.GetChild(subType);
      Transform newResParent = typeCont;
      if(typeCont.childCount == 0 || typeCont.GetChild(typeCont.childCount - 1).childCount == 3) {
        GameObject newBar = (GameObject)Instantiate(resourceBarPre, transform.position, transform.rotation);
        newResParent = newBar.transform;
        newResParent.SetParent(typeCont, false);
      } else {
        newResParent = typeCont.GetChild(typeCont.childCount - 1);
      }
      GameObject newResElement = (GameObject)Instantiate(resourceElementPre, transform.position, transform.rotation);
      newResElement.transform.SetParent(newResParent);
      newResElement.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = newRes.res.GetIcon();
      newResElement.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = newRes.amount + "";
      resElements.Add(newRes.res, newResElement);
      typeCont.GetComponent<LayoutGroup>().SetLayoutVertical();
      LayoutRebuilder.ForceRebuildLayoutImmediate(typeCont.GetComponent<RectTransform>());
      resourcesContainer.GetComponent<LayoutGroup>().SetLayoutVertical();
      LayoutRebuilder.ForceRebuildLayoutImmediate(resourcesContainer.GetComponent<RectTransform>());
      resourcesPanel.GetComponent<LayoutGroup>().SetLayoutVertical();
      LayoutRebuilder.ForceRebuildLayoutImmediate(resourcesPanel.GetComponent<RectTransform>());
    }

    public void UpdateResourceValue(ResourceStorage updateRes) {
      GameObject updateResElement = resElements[updateRes.res];
      if(updateResElement) {
        updateResElement.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = updateRes.amount + "";
      }
    }

    public void SetFood(int total) {
      foodText.text = "" + total;
    }

    public void SetWood(int total) {
      woodText.text = "" + total;
    }

    public void SetStone(int total) {
      stoneText.text = "" + total;
    }

    public void SetDrachma(int total) {
      drachmaText.text = "" + total;
    }

    public void SetClockUI(int year, int weekInYear, float weekProgress) {
      yearText.text = "" + year;
      weekText.text = "" + weekInYear;
      weekProgressSlider.value = weekProgress;
    }

    public void SetBuildingSelector(BuildableTile bTile) {
      bsUI.ShowSelectorPanel(bTile);
    }

    public void HideBuildingSelector() {
      bsUI.HideSelectorPanel();
    }
}
