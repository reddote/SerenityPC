using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BestiaryMonster : MonoBehaviour
{

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI monsterNameText;
    public Image monsterImage;

    public Sprite MonsterSprite;

    public string description;
    public string monsterName;
    //public Sprite monsterSprite;





    // Start is called before the first frame update
    void Start()
    {
        setUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUI()
    {
        monsterImage.sprite = MonsterSprite;
        descriptionText.text = description;
        monsterNameText.text = monsterName;
    }





}
