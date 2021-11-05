using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PrefabChecker : ScriptableWizard
{
    [MenuItem ("My Tools/Prefab Checker")]
    static void PrefabCheckerWizard()
    {
        ScriptableWizard.DisplayWizard<PrefabChecker>("Prefab Checker", "Check");
    }
    
    private void OnWizardCreate()
    {
        FindPrefabs();
    }

    void FindPrefabs()
    {
        Item[] data = Resources.LoadAll<Item>("");
        List<int> intData = new List<int>();
        
        foreach (var item in data)
        {
            intData.Add(item.ID);
        }
        List<int> duplicates = intData.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        if (duplicates.Any())
        {
            Debug.Log("duplicate item ID");
            foreach (var x in duplicates)
            {
                Debug.Log(x);
                foreach (var y in data)
                {
                    if (y.ID == x)
                    {
                        Debug.Log("duplicate item ID = " + y.ID + " name = " + y.itemName);
                    }
                }
            }
            
        }
    }
}
