using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private SpriteRenderer playerSr;

    public float speed;
    public float jumpForce;

    public bool isLookLeft;
    public Transform groundCheck;
    private bool isGrounded;
    private bool isAtack;
    public Color hitColor;
    public Color noHitColor;

    public GameObject hitBoxPreFab;
    public Transform mao;


    private GameController _GameController;
    private chest _Chest;
    private Animator chestAnimator;
    public int maxHp;
    public buttonHandler ButtonHandler;
    public highScoreTable HighScoreTable;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();//revebe as caracteristicas fisicas defenidas no Unity para o personagem
        playerAnimator = GetComponent<Animator>();//recebe as caracteristicas do animator do Unity (onde são geridas as animações dos elementos como a personagem e inimigos)

        playerSr=GetComponent<SpriteRenderer>();

        _GameController = FindObjectOfType(typeof(GameController)) as GameController; //vai procurar o script GameController que criei e armazena-o na variável _GameController


        _GameController.playerTransform = this.transform;//transfere para a variavel da unity playerTransform do _GameController os valores do transform do playerControler

        _Chest = FindObjectOfType(typeof(chest)) as chest;

        chestAnimator = _Chest.GetComponent<Animator>();


        

    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool("isGrounded", isGrounded);//atribui ao valor no animator da variavel isGrounded o valor da variavel isGrounded do VS (ativando assim as animações correspondentes no animator);

        if(_GameController.currentState!=gameState.GAMEPLAY)//caso o jogo tenha mudado o seu estado(acabado o jogo, perdido, etc) para o movimento do jogador 
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            playerAnimator.SetInteger("h", 0);
            return;
        }


        float h = Input.GetAxisRaw("Horizontal"); //recebe o input horizontal descrito no Unity onde as teclas sao o left e right do teclado 
        float speedY = playerRb.velocity.y; //diz-nos a posição vertical do personagem para sabermos se esta a subir ou a não

        if(h>0 && isLookLeft)//verificar a posição que o personagem esta virado e vira-lo consoante a direção que pretendermos andar
        {
            flip();
        }
        else
        if(h<0 && !isLookLeft)
        {
            flip();
        }

        if(Input.GetButtonDown("Jump") && isGrounded)//se o botao pressionado for space e o personagem estiver em contacto com o chão, este salta o valor de jumpForce
        {
            _GameController.playSFX(_GameController.sfxJump, 0.3f);//associa a acção de saltar o som de salto armazenado na variavel do unity sfxJump;
            playerRb.AddForce(new Vector2(0, jumpForce));
            
        }

        if(Input.GetButtonDown("Fire1") && !isAtack)//se o botao pressionado for o ctrl esquerdo, e a personagem nao estiver a atacar, activa a animaçao de atacar
        {
            _GameController.playSFX(_GameController.sfxAtack, 0.5f);
            isAtack = true;
            playerAnimator.SetTrigger("atack");
           
        }

        if(isAtack && isGrounded)// verifica que se o personagem tiver a atacar e estiver no chao, este para de andar
        {
            h = 0;
        }

       


        playerRb.velocity = new Vector2  (h * speed,speedY);// se o valor de h for diferente de zero o personagem mexe-se de acordo com o valor recebido (positivo direita, negativo esquerda)

        playerAnimator.SetInteger("h", (int)h); // serve para dar ao controlador animator o valor da variável h que nos diz para que direçao o personagem se move (>0 para a direita <0 para a esquerda)
        
        playerAnimator.SetFloat("speedY", speedY);//atribui ao valor no animator da variavel speedY o valor da variavel speedY do VS (ativando assim as animações correspondentes no animator);
        playerAnimator.SetBool("isAtack", isAtack);  //atribui ao valor do animator da variavel isAtack o valor da variavel isAtack do VS(ativando assim as animações correspondentes no animator) 
    }


    private void FixedUpdate()// função que faz uma verificação de estado a cada 0.2 segundos
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);//verifica a colisao da personagem com o chão
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        

        if (col.gameObject.tag == "colectavel")
        {
            _GameController.playSFX(_GameController.coin, 0.5f);
            Destroy(col.gameObject);
            _GameController.getCoin();

        }
        else
        if (col.gameObject.tag == "heart")
        {
            _GameController.playSFX(_GameController.coin, 0.5f);
            Destroy(col.gameObject);
            _GameController.vida += 1;
            _GameController.heartController();
        }
        else
        if (col.gameObject.tag == "bau")
        {

            chestAnimator.SetTrigger("colided");
            chestAnimator.SetInteger("counter", (chestAnimator.GetInteger("counter") + 1));



            if (chestAnimator.GetInteger("counter") == 2)
            {
                chestAnimator.ResetTrigger("colided");
                chestAnimator.SetInteger("counter", 0);
                _GameController.playSFX(_GameController.coin, 0.5f);
                Destroy(col.gameObject);
            }

        }


        else if (col.gameObject.tag == "Damage")
        {

            _GameController.getHit();
            if (_GameController.vida > 0)
            {
                StartCoroutine("damageController");
            }
        }

        else if (col.gameObject.tag == "morte")
        {
            _GameController.playSFX(_GameController.sfxDamage, 0.5f);
            _GameController.vida = 0;
            _GameController.heartController();
            _GameController.painelGameOver.SetActive(true);
            _GameController.currentState = gameState.GAMEOVER;
            _GameController.trocaMusica(musicafase.GAMEOVER);
        }

        else if (col.gameObject.tag == "final")
        {
            
            _GameController.trocaMusica(musicafase.THEEND);

            //muda a layer do player para nao sofrer danos depois de acabar o nivel
            this.gameObject.layer = LayerMask.NameToLayer("Invencivel");

            //muda o game state para o player deixar de andar
            _GameController.currentState = gameState.INSERTNAME;

            //activa o painel para inserir o nome
            _GameController.painelButtonHandler.SetActive(true);

            StartCoroutine(timeHighScores());

            


        }

        if (col.gameObject.tag == "caverna")
        {
            _GameController.trocaMusica(musicafase.CAVERNA);
        }
        else if (col.gameObject.tag == "floresta")
            _GameController.trocaMusica(musicafase.FLORESTA);
    }

    //minhas funções----------------------------------------------------------------------------
    void flip()// função para virar a posição do personagem
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; //inverte o valor do scale x para virar o boneco para outro lado

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);//atribui ah scale da personagem o valor invertido de x
    }

    void OnEndAtack()// esta funçao é chamada no unity no ultimo frame do atack para parar a animação de ataque
    {
        isAtack = false;
    }

    void hitBoxAtack()
    {
        GameObject hitBoxTemp = Instantiate(hitBoxPreFab, mao.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.2f);
    }

    void footStep()
    {
        _GameController.playSFX(_GameController.sfxStep[Random.Range(0, _GameController.sfxStep.Length)], 1f);
       
    }

    IEnumerator damageController()//função que controla quando o personagem colide com os inimigos e gere as cores do mesmo
    {
        _GameController.playSFX(_GameController.sfxDamage, 0.3f);

    
        this.gameObject.layer = LayerMask.NameToLayer("Invencivel");
        playerSr.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        playerSr.color = noHitColor;

        for(int i=0; i<5; i++)
        {
            playerSr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        playerSr.color = Color.white;
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }


    //coroutina para fazer os timmings de apresentação do painel de highScores e o painel do final do jogo.
    IEnumerator timeHighScores()
    {
        yield return new WaitUntil(() => ButtonHandler.routina==true);

        _GameController.painelHighScore.SetActive(true);

        yield return new WaitForSeconds(8f);

        _GameController.painelHighScore.SetActive(false);
       
        _GameController.currentState = gameState.THEEND;
        _GameController.painelEnd.SetActive(true);
        this.gameObject.layer = LayerMask.NameToLayer("Player");

    }


}
