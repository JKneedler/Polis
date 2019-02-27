using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

  public Text populationText;
  public TextMeshProUGUI foodText;
  public TextMeshProUGUI woodText;
  public TextMeshProUGUI stoneText;
  public TextMeshProUGUI drachmaText;
  public GameObject infoWindow;
  public Text infoWindowTitle;
  public Text infoWindowDescription;
  public GameObject villagerWindow;
  public GameObject buildingWindow;
  public GameObject resourcesWindow;
  public GameObject jobWindow;
  public GameObject tasksWindow;
  public Text foresterAmtText;
  public Text farmerAmtText;
  public TextMeshProUGUI yearText;
  public TextMeshProUGUI weekText;
  public Slider weekProgressSlider;

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

    public void SetDrachma(int total) {
      drachmaText.text = "" + total;
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

    public void HideAllTabWindows() {
      resourcesWindow.SetActive(false);
      jobWindow.SetActive(false);
      tasksWindow.SetActive(false);
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

    public void SetForesterTextAmt(int amt) {
      foresterAmtText.text = "" + amt;
    }

    public void SetFarmerTextAmt(int amt) {
      farmerAmtText.text = "" + amt;
    }

    public void SetClockUI(int year, int weekInYear, float weekProgress) {
      yearText.text = "" + year;
      weekText.text = "" + weekInYear;
      weekProgressSlider.value = weekProgress;
    }

    public void DisplayResourcesWindow() {
      resourcesWindow.SetActive(true);
    }

    public void DisplayTasksWindow() {
      tasksWindow.SetActive(true);
    }

    public void DisplayJobsWindow() {
      jobWindow.SetActive(true);
    }
}
