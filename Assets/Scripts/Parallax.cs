using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform background;
    public float speed;

    private Transform cam;
    private Vector3 previewCamPosition;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        previewCamPosition = cam.position;
        
    }

    // Update is called once per frame


    private void LateUpdate()
    {
        float parallaxX = previewCamPosition.x - cam.position.x; /// variavel para armazenar a subtração da posiçao da camera actual com a posição da camera anteior;
        float bgTargetX = background.position.x + parallaxX;// variavel para armazenar a soma da posição horizontal do fundo com a variavel anterior

        Vector3 bgPsoition = new Vector3(bgTargetX, background.position.y, background.position.z);//altera o valor da posição horizontal
        background.position = Vector3.Lerp(background.position, bgPsoition, speed * Time.deltaTime); //desloca da posiçao a para a posiçao b a certa velocidade determinada pelo speed

        previewCamPosition = cam.position;// armazena o novo valor da posição após ter mexido o fundo.
    }
}
