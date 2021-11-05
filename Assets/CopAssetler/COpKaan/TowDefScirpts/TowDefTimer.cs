using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowDefTimer : MonoBehaviour
{

   
    SpawnPointController spawnPointController;
    public static int newSpawn;
    public static float time=30;
    public Text text;
    public Camera droneCam;
    public Camera towDefCam;
    public static bool sunSetting;



    // Start is called before the first frame update
    void Start()
    {
        //PlayType.type = PlayType.playType.towDef; 
        spawnPointController = FindObjectOfType<SpawnPointController>();
        newSpawn = PlayerPrefs.GetInt("newSpawn", 1);
    }

    // Update is called once per frame
    void Update()
    {

       // Debug.Log(SpawnPoint.livingTowDefMonsters.Count);

        if(Input.GetKeyDown(KeyCode.P) && PlayType.type == PlayType.playType.towDef)
        {
            PlayType.type = PlayType.playType.drone;
            droneCam.gameObject.SetActive(true);
            towDefCam.gameObject.SetActive(false);
        }

        if (time > 0 && PlayType.type==PlayType.playType.drone)
        {
            time -= Time.deltaTime;
            text.text = "Till mmornin=" + time;
           
        }
        else if(time<0 && SpawnPoint.livingTowDefMonsters.Count<=0)
        {
            nextDay();
            
            PlayType.type = PlayType.playType.fps;
            
        }
    }

    public void nextDay()
    {
        time = 30;
        sunSetting = true;
        PlayType.type = PlayType.playType.fps;
        Cursor.lockState = CursorLockMode.None;
        droneCam.gameObject.SetActive(false);
       // towDefCam.gameObject.SetActive(true);
        spawnPointController.endTowDef();
        if (newSpawn < 3)
        {
            newSpawn++;
            PlayerPrefs.SetInt("newSpawn", newSpawn);
        }
        else
        {
            newSpawn = 1;
            PlayerPrefs.SetInt("newSpawn", newSpawn);
        }
       
    }


}
