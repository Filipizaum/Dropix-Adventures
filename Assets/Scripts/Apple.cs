using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Apple : MonoBehaviour
{
    private SpriteRenderer sr;
    private CircleCollider2D circle;

    public GameObject collected;
    public int Score;
    public string uuid;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        circle = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            sr.enabled = false;
            circle.enabled = false;
            collected.SetActive(true);

            string currentLevelName = SceneManager.GetActiveScene().name;
            List<string> lista = GameController.macasDasFases[currentLevelName];
            string muiid = gameObject.GetComponent<Apple>().uuid;
            Debug.Log("Comendo a maçã de UUID " + muiid);
            lista.Remove(muiid);

            GameController.instance.totalScore += Score;
            GameController.UpdateScoreText();
            
            Destroy(gameObject, 0.5f);
        }
    }
}
