using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private TowDefMonster Monster;
    private Collider attackCollider;
    private Player player;
    public float damageMultiplier=1;
    



    // Start is called before the first frame update
    void Start()
    {
        Monster = GetComponentInParent<TowDefMonster>();
        attackCollider = GetComponent<Collider>();
        player = FindObjectOfType<Player>().GetComponent<Player>();
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag.Equals("Player"))
        {
            var direction= transform.position - player.transform.position;
            var damage = Monster.SizeRng(Monster.monsterSize);
            player.takeDamage(damage*damageMultiplier);
            player.Knockback(direction.normalized,50);
            gameObject.SetActive(false);
        }
        if (other.gameObject.tag.Equals("Base"))
        {
            var damage = Monster.SizeRng(Monster.monsterSize);
            Base.takeDamage(damage*damageMultiplier);
        }
       
    }


}
