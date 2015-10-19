using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface

//This is the class you will be storing
//in the different collections. In order to use
//a collection's Sort() method, this class needs to
//implement the IComparable interface.
public class Upgrades : IComparable<Upgrades>
{
    public int type;
    public string name;
    public int level;
    public int cost;
    public int upgradeAmount;

    public Upgrades(int newType, string newName, int newLevel, int newCost, int newUpgradeAmount)
    {
        // button type, upgrade name, weapon level--still upgradable?, cost, upgrade amount
        type = newType;
        name = newName;
        level = newLevel;
        cost = newCost;
        upgradeAmount = newUpgradeAmount;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(Upgrades other)
    {
        if (other == null)
        {
            return 1;
        }
        else
            return 0;
    }
}