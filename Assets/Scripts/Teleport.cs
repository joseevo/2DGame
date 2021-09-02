using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private GameController _GameController;

    public Transform pontoSaida;
    public Transform posCamera;
    public Transform limitCamEsc, limitCamDir, limitCamSup, limitCamBaixo;
    public musicafase novaMusica;
    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;//atribui a esta variavel acesso à informaçao do GameController;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            _GameController.trocaMusica(musicafase.CAVERNA);

            col.transform.position = pontoSaida.position;//muda o personagem para a nova posiçao
            Camera.main.transform.position = posCamera.position;//muda a posiçao de inicio da camera para uma nova posiçao

            _GameController.limitCamEsc = limitCamEsc;//muda os limites todos da camera para a nova área
            _GameController.limitCamDir = limitCamDir;
            _GameController.limitCamSup = limitCamSup;
            _GameController.limitCamBaixo = limitCamBaixo;
        }
    }

}
