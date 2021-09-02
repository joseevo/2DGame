using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batIA : MonoBehaviour
{
    private GameController _GameController;
    private bool isFollowed;
    public float speed;
    public bool isLookLeft;
    //private int h;
    private float tempPos;
    private Animator batAnimator;
    public GameObject hitBox;
    public Rigidbody2D batRb;
    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        batAnimator = GetComponent<Animator>();
        batRb = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {

        if (_GameController.currentState != gameState.GAMEPLAY)//caso o jogo tenha mudado o seu estado(acabado o jogo, perdido, etc) nao faz a parte do movimento dos objectos
        {
            return;
        }
        if (tempPos < transform.position.x && isLookLeft)//verificar a posição que o personagem esta virado e vira-lo consoante a direção que pretendermos andar
        {
            flip();
        }
        else
      if (tempPos >= transform.position.x  && !isLookLeft)
        {
            flip();
        }

        if (isFollowed==true)
        {
            tempPos = transform.position.x;
            transform.position = Vector3.MoveTowards(transform.position, _GameController.playerTransform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "hitBox")
        {

            Destroy(hitBox);
            _GameController.playSFX(_GameController.sfxEnemyDead, 0.1f);
            batAnimator.SetTrigger("Dead");
        }
    }

    private void OnBecameVisible()//ao ver o player começa a segui-lo
    {

        isFollowed = true;
        
    }

    private void OnBecameInvisible()//ao deixar de ver o player deixa de segui-lo
    {
        isFollowed = false;
    }

    void flip()// função para virar a posição do personagem
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; //inverte o valor do scale x para virar o boneco para outro lado

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);//atribui ah scale da personagem o valor invertido de x
    }

   

    void onDead()
    {
        Destroy(this.gameObject);
    }

}
