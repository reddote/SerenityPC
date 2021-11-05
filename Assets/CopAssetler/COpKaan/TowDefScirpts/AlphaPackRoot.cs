using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaPackRoot : MonoBehaviour
{
    BasicMonsterAlpha basicMonsterAlpha;


    // Start is called before the first frame update
    void Start()
    {
        basicMonsterAlpha = GetComponentInChildren<BasicMonsterAlpha>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = basicMonsterAlpha.transform.position;
    }










}
