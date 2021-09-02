using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum musicafase //vai servir para defenir a musica a tocar de acordo com a respectiva fase do jogo
{
    TITULO, FLORESTA, CAVERNA, GAMEOVER, THEEND
}

public enum gameState//vai servir para podermos defenir o comportamento de alguns elementos consoante o estado do jogo
{
    TITULO, GAMEPLAY, GAMEOVER, THEEND, INSERTNAME
}
public class GameController : MonoBehaviour
{
    public gameState currentState;//automaticamente carrega o valor do primeiro enum de gamestate (TITULO)


    public GameObject painelTitulo, painelGameOver, painelEnd, painelHighScore, painelButtonHandler;

    public Transform playerTransform;
    private Camera cam;
    public float speedCam;
    
    public Transform limitCamEsc, limitCamDir, limitCamSup, limitCamBaixo;

    private Animator imagem;
    private imagemTitulo carregarImagem;

    [Header("Audio")]
    public AudioSource sfxSource, musicSource;

    public AudioClip sfxJump;//cria as variáveis de audio publicas no unity para podermos carregar com clips de som
    public AudioClip sfxAtack;
    public AudioClip[] sfxStep;
    public AudioClip sfxEnemyDead;
    public AudioClip sfxDamage;

    public AudioClip musicFloresta, musicCaverna, musicGameOver, musicTheEnd,musicTitulo;
    public GameObject[] fase;

    public musicafase musicaAtual;//automaticamente carrega o primeiro valor do enum que é TITULO

    public AudioClip coin;

    public int moedasApanhadas;
    public Text moedasTxt;
    public Image[] coracoes;
    public int vida;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; // recebe os valores da camera principal do jogo;  

        heartController();//vefifica os coraçoes ativados comparando com a vida que temos

        carregarImagem = FindObjectOfType(typeof(imagemTitulo))as imagemTitulo;
        imagem=carregarImagem.GetComponent<Animator>();



    }

    // Update is called once per frame
    void Update()
    {

        if (currentState==gameState.TITULO && Input.GetKeyDown(KeyCode.Space))//activa o jogo a partir do titulo
        {
            currentState = gameState.GAMEPLAY;
            painelTitulo.SetActive(false);
            trocaMusica(musicafase.FLORESTA);
            imagem.SetBool("mudouTitulo", true);//para controlar a imagem em movimento no titulo
            
        }
        else if(currentState==gameState.GAMEOVER && Input.GetKeyDown(KeyCode.Space))//recarrega a cena do jogo a partir do game over
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(currentState==gameState.THEEND && Input.GetKeyDown(KeyCode.Space))//recarrega a cena do jogo a partir do fim do jogo
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
           
        }

        
        
    }

    private void LateUpdate() //faz um update quando acabar de processar todos os updates diferentes, mas ainda dentro do mesmo frame
    {
        camController();


    }

    void camController() //funçao para por limites à camera e fazer esta acompanhar o personagem ao longo do jogo;
    {
        float posCamX = playerTransform.position.x; // variavel recebe a posição horizontal do jogador
        float posCamY = playerTransform.position.y; // variavel recebe a posição vertical do jogador


        if (cam.transform.position.x < limitCamEsc.position.x && playerTransform.position.x < limitCamEsc.position.x) //verifica os limites horizontais esquerdos da camera
        {
            posCamX = limitCamEsc.position.x;
        }
        else
         if (cam.transform.position.x > limitCamDir.position.x && playerTransform.position.x > limitCamDir.position.x)// verifica os limite horizontais direitos da camera
        {
            posCamX = limitCamDir.position.x;
        }


        if (cam.transform.position.y < limitCamBaixo.position.y && playerTransform.position.y < limitCamBaixo.position.y)//verifica os limites verticais inferiores da camera
        {
            posCamY = limitCamBaixo.position.y;
        }
        else
        if (cam.transform.position.y > limitCamSup.position.y && playerTransform.position.y > limitCamSup.position.y)//verifica os limites verticais superiores da camera
        {
            posCamY = limitCamSup.position.y;
        }



        Vector3 posCam = new Vector3(posCamX, posCamY, cam.transform.position.z);

        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, speedCam * Time.deltaTime); // move a camera da posiçao de horigem até à posição de destino a uma velocidade determinada pela variável speedCam
    }

    public void playSFX(AudioClip sfxClip, float volume)//serve para fazer o play dos efeitos sonoros uma so vez com um determinado volume
    {

        sfxSource.PlayOneShot(sfxClip, volume);

    }

    public void trocaMusica(musicafase novaMusica)//funçao que controla que musica tocar de acordo com a musicfase em que o player esta
    {
        AudioClip clip = null;

        switch(novaMusica)
        {
            case musicafase.CAVERNA:
                clip = musicCaverna;
                break;

            case musicafase.FLORESTA:
                clip = musicFloresta;
                break;
            case musicafase.GAMEOVER:
                clip = musicGameOver;
                break;
            case musicafase.THEEND:
                clip = musicTheEnd;
                break;
            case musicafase.TITULO:
                clip = musicTitulo;
                break;
                                
        }  
        
        StartCoroutine("controleMusica", clip);
    }

    IEnumerator controleMusica(AudioClip musica)//funçao que controla a musica e alteração da mesma
    {
        float volumeMaximo = musicSource.volume;

        for(float volume=volumeMaximo; volume>0;volume-=0.01f)//vai reduzindo o som ate este nao se ouvir
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();//espera ate ao final do frame para fazer o return
        }

        musicSource.clip = musica;
        musicSource.Play();

        for (float volume = 0; volume < volumeMaximo; volume += 0.01f)//aumenta o volume após o play do audioclip;
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }
    }

    public void heartController()//função que desabilita todos os coraçoes de vida no jogo e compara o valor da variavel vida para activar novamente os coraçoes de acordo com a vida que o player tem
    {
        foreach(Image h in coracoes)
        {
            h.enabled = false;
        }

        for(int v=0; v<vida; v++)
        {
            coracoes[v].enabled = true;
        }
    }

    public void getHit()
    {
        vida -= 1;
        heartController();
        if(vida<=0)//se nao tiver mais vidas
        {
            playerTransform.gameObject.SetActive(false);//desactiva o player (nao destroi o player para nao dar erro com as outras funções que usam o player)
            painelGameOver.SetActive(true);//activa o painel do gameOver
            currentState = gameState.GAMEOVER;//muda o current state para game over
            
            trocaMusica(musicafase.GAMEOVER);//
        }
    }

    public void getCoin()
    {
        moedasApanhadas += 1;
        moedasTxt.text = moedasApanhadas.ToString();
    }



    
}
 