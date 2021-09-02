using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeIA : MonoBehaviour
{
    private GameController _GameController; 
    private Rigidbody2D slimeRb;//variaveis defenidas do tipo dos componentes existentes dentro do objecto
    private Animator slimeAnimator;//variaveis defenidas do tipo dos componentes existentes dentro do objecto

    public float speed;
    public float timeToWalk;
    public bool isLookLeft;
    public GameObject hitBox;

    private int h;
    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof (GameController)) as GameController;//atribuimos à variável acesso aos dados do nosso script do GameController

        slimeRb = GetComponent<Rigidbody2D>();//atribuimos à variável os valores do componente rigidbody2D deste objecto
        slimeAnimator = GetComponent<Animator>();//atribuimos à variável os valores do componente animator deste objecto

        StartCoroutine("slimeWalk");
    }

    // Update is called once per frame
    void Update()
    {
        if(_GameController.currentState!=gameState.GAMEPLAY)//caso o jogo tenha mudado o seu estado(acabado o jogo, perdido, etc) nao faz a parte do movimento dos objectos
        {
            return;
        }

        if (h > 0 && isLookLeft)//verificar a posição que o personagem esta virado e vira-lo consoante a direção que pretendermos andar
        {
            flip();
        }
        else
       if (h < 0 && !isLookLeft)
        {
            flip();
        }

        slimeRb.velocity = new Vector2(h * speed, slimeRb.velocity.y);
           
        if (h != 0)
        {
            slimeAnimator.SetBool("isWalk",true);

        }
        else
            slimeAnimator.SetBool("isWalk",false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="hitBox")
        {
            h = 0;
            StopCoroutine("slimeWalk");
            Destroy(hitBox);
            _GameController.playSFX(_GameController.sfxEnemyDead, 0.1f);
            slimeAnimator.SetTrigger("dead");
        }

        if (col.gameObject.tag == "paredeInimigo")//para evitar que os slimes continuem sobre as cavernas ou alem dos limites do jogo
        {
            if (h == -1)
                h = 1;
            else if (h == 1)
                h = -1;
        }

    }

    IEnumerator slimeWalk()//funçao para fazer mover o slime onde é gerado um numero random de 0 a 100 e consoante o resultado ou anda para a esquerda, direita ou fica parado
    {
        int rand = Random.Range(0, 100);

        if (rand < 33)
        {
            h = -1;
        }
        else if (rand < 66)
        {
            h = 0;
        }
        else
            h = 1;

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("slimeWalk");

        
        
    }

    void onDead() //destroi o objecto quando acaba o frame do animator da sua destruiçao
    {
        Destroy(this.gameObject);
    }

    void flip()// função para virar a posição do personagem
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; //inverte o valor do scale x para virar o boneco para outro lado

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);//atribui ah scale da personagem o valor invertido de x
    }
}
