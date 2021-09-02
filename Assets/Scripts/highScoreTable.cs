using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class highScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highScoreEntryTransformList;


    public void Awake()
    {
       
        //painel dos highsocores
        entryContainer = transform.Find("highScoreEntryContainer");

        //lista dos high scores
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        //inativa a lista dos high scores
       entryTemplate.gameObject.SetActive(false);


        //armazena o caminho onde é armazenado o ficheiro json
        string jsonString = PlayerPrefs.GetString("highScoreTable");

        

        //carrega a lista highscores com a informaçao dentro do ficheiro json
        HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);

        //ordena os scores do mais alto para o mais baixo
        for (int i= 0;i< highscores.highScoreEntryList.Count; i++)
        {
            for(int j = i + 1; j < highscores.highScoreEntryList.Count; j++)
            {
                if (highscores.highScoreEntryList[j].score > highscores.highScoreEntryList[i].score)
                {
                    HighScoreEntry tmp = highscores.highScoreEntryList[i];
                    highscores.highScoreEntryList[i] = highscores.highScoreEntryList[j];
                    highscores.highScoreEntryList[j] = tmp; 
                }
            }
        }
        


        highScoreEntryTransformList = new List<Transform>();

        //para cada registo na lista do highScoresEntryList chama a função createHighScoreEntryTransform para a parametrização estetica dos registos
        //foreach (HighScoreEntry highScoreEntry in highscores.highScoreEntryList)
        //{
        //    createHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
        //}


        //para os primeiros 10 registos na lista do highScoresEntryList chama a função createHighScoreEntryTransform para a parametrização estetica dos registos
        for (int i=0; i < 10; i++)
        {
            createHighScoreEntryTransform(highscores.highScoreEntryList[i], entryContainer, highScoreEntryTransformList);
        }



    }

    //funçao que faz a parametrização estetica dos registos dos highscores
    public void createHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        //parametriza o display da lista de highscores
        float templateHeight = 35f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryrectTransform = entryTransform.GetComponent<RectTransform>();
        entryrectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        //atribui um rank a cada um dos elementos da lista dos highscores
        int rank = transformList.Count + 1;
        string rankString;

        //if (rank <= 10)
        //{



            //para os tres primeiros scores concatena o 1st 2nd e 3rd enquanto para os restantes adiciona apenas o "th" ao valor do rank
            switch (rank)
            {
                default:
                    rankString = rank + "TH"; break;

                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;

            }

            //atribui ao elemento posText o rankString, ao scoreText o score.ToString() e ao nameText o name.
            entryTransform.Find("posText").GetComponent<Text>().text = rankString;

            int score = highScoreEntry.score;

            entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

            string name = highScoreEntry.name;

            entryTransform.Find("nameText").GetComponent<Text>().text = name;

            //muda a cor do background linha a linha consoante o ranking é par ou impar
            entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

            //muda a cor do texto do primeiro no ranking para verde
            if (rank == 1)
            {
                entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
                entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
                entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
            }

            //muda cor do trofeu consoante a posição
            switch (rank)
            {
                default:
                    entryTransform.Find("trophy").gameObject.SetActive(false);

                    break;

                case 1:
                    entryTransform.Find("trophy").GetComponent<Image>().color = Color.yellow;

                    break;

                case 2:
                    entryTransform.Find("trophy").GetComponent<Image>().color = Color.cyan;

                    break;

                case 3:
                    entryTransform.Find("trophy").GetComponent<Image>().color = Color.magenta;

                    break;
            }

        //}

       
            //adiciona todas estas mudanças esteticas à lista transformList
            transformList.Add(entryTransform);
        
    }


    public void AddHighScoreEntry(int score, string name)
    {
        //cria o highScoreEntry
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };

        //Carrega os highScores guardados
        string jsonString = PlayerPrefs.GetString("highScoreTable"); //path
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString); //carrega na highScores a lista dos highscores nesse path


            //verifica se ja existe o ficheiro "highScoreTable" quando tenta adicionar informação se nao existir cria um   
            try
            {
                highScores.highScoreEntryList.Add(highScoreEntry);
            }
            catch (Exception ex)
            {

            List<HighScoreEntry> highScoreEntryList = new List<HighScoreEntry>();
         
            HighScores highScoresTwo = new HighScores { highScoreEntryList = highScoreEntryList };

            string jsonTwo = JsonUtility.ToJson(highScoresTwo);
                PlayerPrefs.SetString("highScoreTable", jsonTwo);
                PlayerPrefs.Save();

            //depois de ter sido criado um ficheiro "highScoreTable" volta a pedir o conteudo do ficheiro
            jsonString = PlayerPrefs.GetString("highScoreTable");
            highScores = JsonUtility.FromJson<HighScores>(jsonString);

            //volta a tentar adicionar a nova entrada
            highScores.highScoreEntryList.Add(highScoreEntry);
            //carrega o conteudo de highScores em formato json numa variavel chamada json
            string jsonThree = JsonUtility.ToJson(highScores);
            //carrega o ficheiro json highScoreTable com a informaçao da variável json
            PlayerPrefs.SetString("highScoreTable", jsonThree);
            PlayerPrefs.Save();
            return;
            }
        
        
    
        //salva os highScores atualizados
        string json = JsonUtility.ToJson(highScores);//carrega o conteudo de highScores em formato json numa variavel chamada json
        PlayerPrefs.SetString("highScoreTable", json);//carrega o ficheiro json highScoreTable com a informaçao da variável json
        PlayerPrefs.Save();//faz o guardar das alterações realizadas


    }


    //lista que serve para carregar os highscores
    public   class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }


    //class que serve para armazenar a informaçao de um novo registo dos highScores
    [System.Serializable]
    public class HighScoreEntry
    {
        public int score;
        public string name;
    }
}


