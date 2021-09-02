using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class buttonHandler : MonoBehaviour
{
    [SerializeField] public highScoreTable HighScoreTable;

    public GameController _GameController;

    public GameObject inputField;

    public bool routina = false;

    private void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    
    public void buttonClick(string answer)
    {

        if (answer == "OkButton")
        {

            //chama a função para adicionar o highscore com as moedas apanhadas e o nome inserido na inputfield
            HighScoreTable.AddHighScoreEntry(_GameController.moedasApanhadas, inputField.GetComponent<TMPro.TextMeshProUGUI>().text.ToString());
            
            //desativa o painel para inserção do nome
            _GameController.painelButtonHandler.SetActive(false);
            //ativa o painel dos highsores
            _GameController.painelHighScore.SetActive(true);

            routina = true;
           
        }

        else if (answer == "CancelButton")
        {
            //desativa o painel para inserção do nome
            _GameController.painelButtonHandler.SetActive(false);
            //ativa o painel dos highsores
            _GameController.painelHighScore.SetActive(true);
        }

    }

}
