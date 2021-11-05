using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {

        TowDefMonster monster = other.GetComponent<TowDefMonster>();
       

        if (monster!=null)
        {
            StartCoroutine(monster.flameMonster(3f, 10f)); 
        }

    }

}
