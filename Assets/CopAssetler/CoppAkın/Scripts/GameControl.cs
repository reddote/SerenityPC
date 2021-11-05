using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

[Serializable]
public class GameControl : MonoBehaviour
{
    public static GameControl control;
    private GameObject playerrr;
    public float pPosx = 1, pPosy = 1, pPosz = 1;
    public float health;
    private Vector3 Playerpos;
    public GameObject laserPrefab, flamePrefab, mechPrefab, icePrefab,Sun,Moon;
    public float[] flameTur = new float[50];
    public float[] laserTur = new float[50];
    public float[] mechTur = new float[50];
    public float[] iceTur = new float[50];
    public float sunPosy, sunPosz, sunRotx, sunRoty, moonPosy, moonPosz, moonRotx, moonRoty;
    public int flameNum = 0, laserNum = 0, mechNum = 0, iceNum = 0;
    public bool Loaded = false;
    public int inventorySlot;
    public List<Turret> turretsInScene = new List<Turret>();
    
    public List<int> items = new List<int>();
    public List<int> itemCount= new List<int>();
    
    public int iter = 0;

    void Awake()
    {

        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
        

    }
    private void Start()
    {
        playerrr = GameObject.FindGameObjectWithTag("Player");
        
            LoadCharPos();
            //LoadInventory();
            LoadTurretCoor();
        
        
    }
    private void FixedUpdate()
    {
        
        if (Input.GetKeyUp(KeyCode.T))
        {
            SaveCharPos();
            SaveTurretCoor();
            //SaveInventory();

        }

    }
    private void Update()
    {

       
        
        if (Input.GetKeyUp(KeyCode.O))
        {
            switchNums();
        }


    }
    //======================== GÜNEŞ VE AYIN SAVE VE LOAD FONKSİYONU ======================================

    public void SMposrotSave()
    {
        
        Debug.Log(Sun.transform.position.y);
        Debug.Log(Sun.transform.position.z);
        Debug.Log(Sun.transform.eulerAngles.x);
        Debug.Log(Sun.transform.eulerAngles.y);
        sunPosy = Sun.transform.position.y;
        sunPosz = Sun.transform.position.z;
        sunRotx = Sun.transform.rotation.eulerAngles.x;
        sunRoty = Sun.transform.rotation.eulerAngles.y;

        moonPosy = Moon.transform.position.y;
        moonPosz = Moon.transform.position.z;
        moonRotx = Moon.transform.rotation.eulerAngles.x;
        moonRoty = Moon.transform.rotation.eulerAngles.y;

    }

    //======================== KARAKTER POZİSYONU SAVE&LOADLARI BURADA BAŞLADI ============================

    public void SaveCharPos()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/charPosInfo.dat");
        PlayerData data = new PlayerData();

        // ============ ZAMAN DA BURADA KAYDEDİLİYOR ======================

        
        Playerpos = playerrr.transform.position;
        pPosx = Playerpos.x;
        pPosy = Playerpos.y;
        pPosz = Playerpos.z;
        data.pPosx = pPosx;
        data.pPosy = pPosy;      
        data.pPosz = pPosz;
        // =========KARAKTER SAVE'I BİTİYOR==========
        // =========GÜNEŞ AY SAVE'I BAŞLIYOR==========


        SMposrotSave();
        data.sunPosy = sunPosy;
        data.sunPosz = sunPosz;
        data.sunRotx = sunRotx;
        data.sunRoty = sunRoty;
        Debug.Log(sunPosz);
        Debug.Log(sunPosy);
        data.moonPosy = moonPosy;
        data.moonPosz = moonPosz;
        data.moonRotx = moonRotx;
        data.moonRoty = moonRoty;


        // =========GÜNEŞ AY SAVE'I BİTİYOR==========



        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Karakter Save'ine girdi");
    }
    public void LoadCharPos()
    {
        if (File.Exists(Application.persistentDataPath + "/charPosInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/charPosInfo.dat", FileMode.Open);
            PlayerData data =(PlayerData) new PlayerData();
            if(file.Length!=0)
                data = (PlayerData)bf.Deserialize(file);
            file.Close();

            pPosx = data.pPosx;
            pPosy = data.pPosy;       
            pPosz = data.pPosz;
            Vector3 newPos = new Vector3(pPosx, pPosy, pPosz);
            playerrr.transform.position = newPos;
            // =========KARAKTER SAVE'I BİTİYOR==========

            // =========GÜNEŞ AY LOAD'I BAŞLIYOR==========
            

            Sun.transform.position = new Vector3(0, data.sunPosy, data.sunPosz);
            Moon.transform.position = new Vector3(0, data.moonPosy, data.moonPosz);
            Sun.transform.eulerAngles =new Vector3(data.sunRotx, data.sunRoty,0 );
            Moon.transform.eulerAngles =new Vector3(data.moonRotx, data.moonRoty,0 );


            // =========GÜNEŞ AY LOAD'I BİTİYOR==========
        }
    }
    //======================== KARAKTER POZİSYONU SAVE&LOADLARI BURADA BİTTİ ============================
    public void switchNums()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/turretInfo.dat");
        PlayerData data = new PlayerData();
        laserNum = 0;
        mechNum = 0;
        iceNum = 0;
        flameNum = 0;
        data.laserNum = laserNum;
        data.mechNum = mechNum;
        data.iceNum = iceNum;
        data.flameNum = flameNum;
        Loaded = false;
        // TURRET LOADLAREN NUMLARI SIFIRLAMAK İÇİN FONKSİYON
    }

    //======================== TURRET SAVE&LOADLARI BURADAN YAPILIR ============================
    public void SaveTurretCoor()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/turretInfo.dat");
        PlayerData data = new PlayerData();
        
        //if (PlayType.type == PlayType.playType.drone)
        // {
        turretsInScene = FindObjectsOfType<Turret>().ToList();
        foreach (Turret turret in turretsInScene)
        {
            if (turret.GetType() == typeof(TurretLaser))
            {
                laserNum++;
                data.laserTur[laserNum] = turret.gameObject.transform.position.x;
                laserNum++;
                data.laserTur[laserNum] = turret.gameObject.transform.position.y;
                laserNum++;
                data.laserTur[laserNum] = turret.gameObject.transform.position.z;
                data.laserNum = laserNum;

            }
            if (turret.GetType() == typeof(TurretFlame))
            {
                flameNum++;
                data.flameTur[flameNum] = turret.gameObject.transform.position.x;
                flameNum++;
                data.flameTur[flameNum] = turret.gameObject.transform.position.y;
                flameNum++;
                data.flameTur[flameNum] = turret.gameObject.transform.position.z;
                data.flameNum = flameNum;
            }
            if (turret.GetType() == typeof(TurretIce))
            {
                iceNum++;

                data.iceTur[iceNum] = turret.transform.position.x;
                iceNum++;
                data.iceTur[iceNum] = turret.transform.position.y;
                iceNum++;
                data.iceTur[iceNum] = turret.transform.position.z;
                
                data.iceNum = iceNum;
            }
            if (turret.GetType() == typeof(TurretMech))
            {
                mechNum++;
                data.mechTur[mechNum] = turret.gameObject.transform.position.x;
                mechNum++;
                data.mechTur[mechNum] = turret.gameObject.transform.position.y;
                mechNum++;
                data.mechTur[mechNum] = turret.gameObject.transform.position.z;
                
                data.mechNum = mechNum;
            }
        }
        //}
        
        data.health = health;
       
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Turret Save'ine girdi");
        


    }
    public void LoadTurretCoor()
    {
        if (File.Exists(Application.persistentDataPath + "/turretInfo.dat"))
        {
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/turretInfo.dat", FileMode.Open);
            PlayerData data =(PlayerData) new PlayerData();
            if(file.Length!=0)
                data = (PlayerData)bf.Deserialize(file);
            file.Close();

            
            health = data.health;
            

            for (int i = 1; i <= data.laserNum; i++)
            {
                Instantiate(laserPrefab, new Vector3(data.laserTur[i], data.laserTur[i + 1], data.laserTur[i + 2]), Quaternion.identity);
                i = i + 2;
                
            }
            for (int i = 1; i <= data.flameNum; i++)
            {
                Instantiate(flamePrefab, new Vector3(data.flameTur[i], data.flameTur[i + 1], data.flameTur[i + 2]), Quaternion.identity);
                i = i + 2;

            }
            for (int i = 1; i <= data.mechNum; i++)
            {
                Instantiate(mechPrefab, new Vector3(data.mechTur[i], data.mechTur[i + 1], data.mechTur[i + 2]), Quaternion.identity);
                i = i + 2;
                
            }
            for (int i = 1; i <= data.iceNum; i++)
            {
                Instantiate(icePrefab, new Vector3(data.iceTur[i], data.iceTur[i + 1], data.iceTur[i + 2]), Quaternion.identity);
                i = i + 2;
            }
            
            Loaded = true;
            Debug.Log("Turret Load'una girdi");
        }


    }
    //======================== TURRET SAVE&LOADLARI BURADA BİTTİ ============================


    //======================== INVENTORY SAVE&LOADLARI BURADA BAŞLADI ============================

//    void SaveInventory()
//    {
//        if (!InventoryMenu.instance.isNewGame)
//        {
//            BinaryFormatter bf = new BinaryFormatter();
//            
//            FileStream file = File.Create(Application.persistentDataPath + "/inventoryInfo.dat");
//            PlayerData data = new PlayerData();
//            
//           
//            items = InventoryMenu.instance.items;
//            itemCount = InventoryMenu.instance.itemCount.ToList();
//             
//            foreach (int item in items)
//            {
//                
//                data.itemsTemp.Add(item);  
//            }
//            
//            iter = 0;
//            foreach (int itemCoun in itemCount)
//            {
//                
//                data.itemCountTemp.Add(itemCoun);
//                iter++;
//            }
//            
//            iter = 0;
//            bf.Serialize(file, data);
//            file.Close();
//            Debug.Log("inventory saved");
//            
//        }
//
//    }
//    void LoadInventory()
//    {
//        if (!InventoryMenu.instance.isNewGame)
//        {   
//            if (File.Exists(Application.persistentDataPath + "/inventoryInfo.dat"))
//            {
//            BinaryFormatter bf = new BinaryFormatter();
//            FileStream file = File.Open(Application.persistentDataPath + "/inventoryInfo.dat", FileMode.Open);
//
//            PlayerData data =(PlayerData) new PlayerData();
//            if(file.Length!=0)
//                data = (PlayerData)bf.Deserialize(file);
//            file.Close();
//
//
//                items = data.itemsTemp;
//                itemCount = data.itemCountTemp;
//                foreach (int item in items)
//                {
//                    InventoryMenu.instance.items.Add(item);
//                }
//                iter = 0;
//                foreach (int itemC in itemCount)
//                {
//                    InventoryMenu.instance.itemCount.Add(itemC);
//                    iter++;
//                }
//            }
//            //Debug.Log("inventory Load'una girdi");
//        }
//
//    }
    //======================== INVENTORY SAVE&LOADLARI BURADA BİTTİ ============================
}
[Serializable]
class PlayerData
{
   

    
    // LİSTE CEVRİLECEK =========
    public float[] flameTur = new float[50];
    public float[] laserTur = new float[50];
    public float[] mechTur = new float[50];
    public float[] iceTur = new float[50];
 //==============================

    public float health;
    public int flameNum, laserNum, mechNum, iceNum;
    public float pPosx, pPosy, pPosz;
    public float sunPosy, sunPosz, sunRotx, sunRoty,moonPosy,moonPosz,moonRotx,moonRoty;
    
    
    
    public List<int> itemCountTemp = new List<int>();
    public List<int> itemsTemp = new List<int>();
    public int iter;
}

