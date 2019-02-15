using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

  public Text populationText;
  public Text foodText;
  public Text woodText;
  public Text stoneText;
  public GameObject infoWindow;
  public Text infoWindowTitle;
  public Text infoWindowDescription;
  public GameObject villagerWindow;
  public GameObject buildingWindow;
  public GameObject resourcesWindow;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetPopulation(int total) {
      populationText.text = "" + total;
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

    public void DisplayInfoPanel(WorldDescriptor info) {
      infoWindow.SetActive(true);
      infoWindowTitle.text = info.title;
      infoWindowDescription.text = info.description;
    }

    public void HideInfoPanel() {
      infoWindow.SetActive(false);
    }

    public void HideAllActiveObjectWindows() {
      villagerWindow.SetActive(false);
      buildingWindow.SetActive(false);
    }

    public void DisplayVillagerWindow(Villager vill) {
      villagerWindow.SetActive(true);
    }

    public void DisplayBuildingWindow(Structure str) {
      buildingWindow.SetActive(true);
    }

    public void ToggleResourcesWindow() {
      if(resourcesWindow.activeSelf) {
        resourcesWindow.SetActive(false);
      } else {
        resourcesWindow.SetActive(true);
      }
    }
}
