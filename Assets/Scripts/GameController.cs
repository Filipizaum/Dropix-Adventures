using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    public int totalScore;
    public TMP_Text scoreText; // https://learn.unity.com/tutorial/working-with-textmesh-pro

    public GameObject gameOver;
    public GameObject scoreGroup;
    public GameObject newGameGroup;

    public GameObject player;

    public static GameController instance;

    public static Dictionary<string, List<string>> macasDasFases = new Dictionary<string, List<string>>();

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) // Se ainda não tem a instância da singleton
        {
            if(player != null) // Se o player foi passado pro GameController
            {
                // Pega o Script dele
                Player playerComponent = player.GetComponent<Player>();
                // Faz com que ele não possa se mexer
                playerComponent.ableToMove = false; // Não permite que o jogador se mova
            }
            DontDestroyOnLoad(gameObject); // Faz com que este objeto permaneça entre as cenas
            instance = this; // Guarda o objeto na instância singleton
        }
        else
        {
            /* Repassando objetos específicos pra instância singleton */
            //instance.totalScore = this.totalScore; // Repassa o novo scoreText
            instance.scoreText = this.scoreText; // Repassa o novo scoreText
            instance.gameOver = this.gameOver; // Repassa o novo gameover

            UpdateScoreText();

            Destroy(gameObject); // Apaga o objeto desnecessário
        }
        // Pega o nome do nível ativo
        string levelName = SceneManager.GetActiveScene().name;
        // Atribui o UUID de cada maçã
        
        // Verifica se ele já tem registro no dicionário
        if(!macasDasFases.ContainsKey(levelName))
        {
            Debug.Log("A lista de maçãs deste nível ainda não existe, criando...");
            macasDasFases[levelName] = new List<string>(); // Cria a lista vazia de maçãs
            ProcessaMacas((GameObject apple, int key) => // Percorre cada uma das maçãs da cena
            {
                Debug.Log("Percorrendo maçã " + key);
                Apple appleComponent = apple.GetComponent<Apple>(); // Pega o Script da maçã
                string uuid = key.ToString(); // Usa o índice da maçã como UUID
                appleComponent.uuid = uuid; // Associa a UUID ao componente da maçã
                macasDasFases[(levelName)].Add(uuid); // Adiciona a UUID na lista de maçãs da fase
            });
        }
        else
        {
            Debug.Log("As macas das fases já existiam e eram estas:");
            foreach(string key in macasDasFases[levelName])
            {
                Debug.Log("Key " + key);
            }
        }
        ProcessaMacas((GameObject apple, int key) => // Percorre cada uma das maçãs da cena
        {
            Debug.Log("Percorrendo maçã " + key);
            Apple appleComponent = apple.GetComponent<Apple>(); // Pega o Script da maçã
            DontDestroyOnLoad(apple);
            string uuid = key.ToString();
            appleComponent.uuid = uuid;
            int posicao = macasDasFases[levelName].IndexOf(key.ToString());
            Debug.Log("A posição da maçã " + key + " encontrads foi " + posicao);
            if (posicao == -1) // Se a maçã não existe mais na lista
            {
                Destroy(apple); // Se a maçã da fase não for encontrada na lista, destrói ela
            }
        });
        
    }

    public void NewGame()
    {
        Player playerComponent = player.GetComponent<Player>();
        scoreGroup.SetActive(true); // Mostra o score
        newGameGroup.SetActive(false); // Esconde o menu de New Game
        playerComponent.ableToMove = true; // Permite que o jogador se mova
    }

    private void ProcessaMacas(Action<GameObject, int> Funcao)
    {
        // Pega todos os objetos raiz da cena
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject go in rootObjects) // Percorre cada um destes objetos raíz
        {
            if (go.name == "Apples") // Se o nome do objeto raiz é "Apples"
            {
                // Percorre cada um dos filhos deste objeto
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    // Salva o gameObject deste filho em uma variável
                    GameObject apple = go.transform.GetChild(i).gameObject;
                    Funcao(apple, i);
                }
            }
        }
    }

    void Update()
    {
        // https://stackoverflow.com/questions/54431635/is-there-a-keyboard-shortcut-to-maximize-the-game-window-in-unity-in-play-mode#answer-60230758
        // Pressiona Esc para alternar entre tela cheia ou não
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
        }
#endif
    }

    public static void UpdateScoreText()
    {
        instance.scoreText.text = instance.totalScore.ToString();
    }

    public static void ShowGameOver()
    {
        instance.gameOver.SetActive(true);
    }

    public static void RestartGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

}
