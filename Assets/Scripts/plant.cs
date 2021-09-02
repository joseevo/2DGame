using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plant : MonoBehaviour
{
   private GameController _GameController;
    private Animator plantAnimator;
    public GameObject hitBoxPreFab;
    public Transform planta;
    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        plantAnimator = GetComponent<Animator>();

    }


    void hitboxAtack()
    {
        GameObject hitboxTemp = Instantiate(hitBoxPreFab, planta.position, transform.localRotation);
        Destroy(hitboxTemp, 0.2f);
    }
}
