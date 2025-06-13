using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    [SerializeField] int bossHP = 100;

    [SerializeField] Animator animator;
    [SerializeField] Slider enemyHealthBar;


    // Start is called before the first frame update
    void Start()
    {
        enemyHealthBar.value = bossHP;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        /*if(target != null)
        {
            
        }*/
        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        else
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
    }


    public void TakeDamage(int damageAmount){
        bossHP -= damageAmount;
        enemyHealthBar.value = bossHP;
        if(bossHP > 0){
            //Get hit
            animator.SetTrigger("Damage");
        }
        else{
            //Become death
            animator.SetTrigger("Death");
            GetComponent<CapsuleCollider2D>().enabled = false;
            this.enabled = false;
        }
    }
}
