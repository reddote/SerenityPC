using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TowDefTool : MonoBehaviour
{

    public static int Currency = 5;
    public static Text CurrencyText;


    // Start is called before the first frame update
    void Start()
    {
        CurrencyText = FindObjectsOfType<Text>().ToList().FirstOrDefault(t => t.name == "Tool");
        textUpdater(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void textUpdater()
    {
        CurrencyText.text = "Tools=" + Currency;
    }

    public void CurrencyUpdater(int price)
    {
        Currency -= price;
    }




}
