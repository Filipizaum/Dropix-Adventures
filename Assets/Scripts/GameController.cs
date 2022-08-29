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
        if (instance == null) // Se ainda n�o tem a inst�ncia da singleton
        {
            if(player != null) // Se o player foi passado pro GameController
            {
                // Pega o Script dele
                Player playerComponent = player.GetComponent<Player>();
                // Faz com que ele n�o possa se mexer
                playerComponent.ableToMove = false; // N�o permite que o jogador se mova
            }
            DontDestroyOnLoad(gameObject); // Faz com que este objeto permane�a entre as cenas
            instance = this; // Guarda o objeto na inst�ncia singleton
        }
        else
        {
            /* Repassando objetos espec�ficos pra inst�ncia singleton */
            //instance.totalScore = this.totalScore; // Repassa o novo scoreText
            instance.scoreText = this.scoreText; // Repassa o novo scoreText
            instance.gameOver = this.gameOver; // Repassa o novo gameover

            UpdateScoreText();

            Destroy(gameObject); // Apaga o objeto desnecess�rio
        }
        // Pega o nome do n�vel ativo
        string levelName = SceneManager.GetActiveScene().name;
        // Atribui o UUID de cada ma��
        
        // Verifica se ele j� tem registro no dicion�rio
        if(!macasDasFases.ContainsKey(levelName))
        {
            Debug.Log("A lista de ma��s deste n�vel ainda n�o existe, criando...");
            macasDasFases[levelName] = new List<string>(); // Cria a lista vazia de ma��s
            ProcessaMacas((GameObject apple, int key) => // Percorre cada uma das ma��s da cena
            {
                Debug.Log("Percorrendo ma�� " + key);
                Apple appleComponent = apple.GetComponent<Apple>(); // Pega o Script da ma��
                string uuid = key.ToString(); // Usa o �ndice da ma�� como UUID
                appleComponent.uuid = uuid; // Associa a UUID ao componente da ma��
                macasDasFases[(levelName)].Add(uuid); // Adiciona a UUID na lista de ma��s da fase
            });
        }
        else
        {
            Debug.Log("As macas das fases j� existiam e eram estas:");
            foreach(string key in macasDasFases[levelName])
            {
                Debug.Log("Key " + key);
            }
        }
        ProcessaMacas((GameObject apple, int key) => // Percorre cada uma das ma��s da cena
        {
            Debug.Log("Percorrendo ma�� " + key);
            Apple appleComponent = apple.GetComponent<Apple>(); // Pega o Script da ma��
            DontDestroyOnLoad(apple);
            string uuid = key.ToString();
            appleComponent.uuid = uuid;
            int posicao = macasDasFases[levelName].IndexOf(key.ToString());
            Debug.Log("A posi��o da ma�� " + key + " encontrads foi " + posicao);
            if (posicao == -1) // Se a ma�� n�o existe mais na lista
            {
                Destroy(apple); // Se a ma�� da fase n�o for encontrada na lista, destr�i ela
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
        foreach (GameObject go in rootObjects) // Percorre cada um destes objetos ra�z
        {
            if (go.name == "Apples") // Se o nome do objeto raiz � "Apples"
            {
                // Percorre cada um dos filhos deste objeto
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    // Salva o gameObject deste filho em uma vari�vel
                    GameObject apple = go.transform.GetChild(i).gameObject;
                    Funcao(apple, i);
                }
            }
        }
    }

    void Update()
    {
        // https://stackoverflow.com/questions/54431635/is-there-a-keyboard-shortcut-to-maximize-the-game-window-in-unity-in-play-mode#answer-60230758
        // Pressiona Esc para alternar entre tela cheia ou n�o
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
