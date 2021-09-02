using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    private GameController _GameController;
    private Animator chestAnimator;
    private Rigidbody2D chestBody;
    public GameObject hitBox2;
    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        chestAnimator=GetComponent<Animator>();
        chestBody=GetComponent<Rigidbody2D>();

        
    }



    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "bau")
    //    {

    //        chestAnimator.SetTrigger("colided");
    //        chestAnimator.SetInteger("counter", chestAnimator.GetInteger("counter") + 1);
    //    }

    //    if (col.gameObject.tag == "bau" && chestAnimator.GetInteger("counter") == 2)
    //    {
    //        chestAnimator.ResetTrigger("colided");
    //        chestAnimator.SetInteger("counter", 0);
    //        _GameController.playSFX(_GameController.coin, 0.5f);
    //        Destroy(hitBox2);
    //    }
    //}
}