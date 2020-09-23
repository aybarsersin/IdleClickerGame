using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu]
public class Building : ScriptableObject
{
    public string incomeType;
    public float incomePerSecond;
    public float incomeIncreasePerLevel;
    public float buildCost;
    public float levelUpgradeCost;
    public float levelUpgradeCostIncrease;
    public float level;
    public Building(string _incomeType, float _buildCost, float _incomePerSecond, float _incomeIncreasePerLevel, float _levelUpgradeCost, float _levelUpgradeCostIncrease,float _level)
    {
        incomeType = _incomeType;
        incomePerSecond = _incomePerSecond;
        incomeIncreasePerLevel = _incomeIncreasePerLevel;
        buildCost = _buildCost;
        levelUpgradeCost = _levelUpgradeCost;
        levelUpgradeCostIncrease = _levelUpgradeCostIncrease;
        level = 1;
    }
}
public class Manager : MonoBehaviour
{
    float gold = 0;
    List<Building> buildings = new List<Building>();
    [SerializeField] Building[] buildsToBuild;
    [SerializeField]GameObject[] TownParents;
    static public Manager instance;
    [SerializeField] Text goldText;
    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Update()
    {
        CalculateEarnedGold();
        CheckForBuildable();
    }
    string Simplify(float number)
    {
        if (number < 1000) return "" + (int)number;
        else if (number >= 1e+30f) return "" + +(int)(number / 1e+30f) + "Non";
        else if (number >= 1e+27f) return "" + +(int)(number / 1e+27f) + "Oc";
        else if (number >= 1e+24f) return "" + +(int)(number / 1e+24f) + "Sp";
        else if (number >= 1e+21f) return "" + +(int)(number / 1e+21f) + "Sx";
        else if (number >= 1e+18f) return "" + +(int)(number / 1e+18f) + "Qi";
        else if (number >= 1e+15f) return "" + +(int)(number / 1e+15f) + "Qa";
        else if (number >= 1e+12f) return "" + (int)(number / 1e+12f) + "T";
        else if (number >= 1e+09f) return "" + (int)(number / 1e+09f) + "B";
        else if (number >= 1000000f) return "" + (int)(number / 1000000f) + "M";
        else if (number >= 1000f) return "" + (int)(number / 1000f) + "K";
        return "NaN";
    }
    void UpdateUI()
    {
        goldText.text = Simplify(gold);
    }
    void CalculateEarnedGold()
    {
        gold += 1 * Time.deltaTime;
        if (buildings != null)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                gold += Time.deltaTime * buildings[i].incomePerSecond;
            }
        }
        if (gold > 10000000000f)
        {
            buttonParent.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (gold > 10000000f)
        {
            buttonParent.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (gold > 10000f)
        {
            buttonParent.transform.GetChild(1).gameObject.SetActive(true);
        }
        UpdateUI();
    }
    [SerializeField] GameObject buttonParent;
    public void ActivateTownBuilding(int index, int _level)
    {
        for (int i = 0; i < TownParents[index].transform.childCount; i++)
        {
            TownParents[index].transform.GetChild(i).gameObject.SetActive(false);
        }
        TownParents[index].transform.GetChild(_level-1).gameObject.SetActive(true);
    }

    public void Build(int index)
    {
        if (gold >= buildsToBuild[index].buildCost)
        {
            ActivateTownBuilding(index, 1);
               gold -= buildsToBuild[index].buildCost;
            Building buildToAdd = new Building(buildsToBuild[index].incomeType, buildsToBuild[index].buildCost, buildsToBuild[index].incomePerSecond, buildsToBuild[index].incomeIncreasePerLevel, buildsToBuild[index].levelUpgradeCost, buildsToBuild[index].levelUpgradeCostIncrease, buildsToBuild[index].level);
            buildings.Add(buildToAdd);
            buttonParent.transform.GetChild(index).transform.GetChild(1).transform.gameObject.SetActive(false);
        }
    }
    void CheckForBuildable()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            Text currentText = buttonParent.transform.GetChild(i).transform.GetChild(0).transform.GetComponentInChildren<Text>();

            currentText.text = Simplify(buildings[i].levelUpgradeCost) + " Upgrade " + buildings[i].incomeType + " +" + Simplify(buildings[i].incomePerSecond);
        }
    }
    public void Upgrade(int index)
    {
        if (gold >= buildings[index].levelUpgradeCost)
        {
           
            Debug.Log(buildings[index].levelUpgradeCost);
            gold -= buildings[index].levelUpgradeCost;
            Debug.Log("Upgrading");
            if (buildings[index] != null)
            {
                Debug.Log("Complete");
                buildings[index].incomePerSecond += buildings[index].incomeIncreasePerLevel;
                buildings[index].levelUpgradeCost *= buildings[index].levelUpgradeCostIncrease;
                buildings[index].level++;
            }
            if (buildings[index].level>8)
            {
                ActivateTownBuilding(index, 5);
            }
            else if (buildings[index].level > 7)
            {
                ActivateTownBuilding(index, 4);
            }
            else if (buildings[index].level > 6)
            {
                ActivateTownBuilding(index, 3);
            }
            else if (buildings[index].level > 5)
            {
                ActivateTownBuilding(index, 2);
            }
        }
    }
}
