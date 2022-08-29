using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject squareTrigger;
    public GameObject theEnd;
    public float movementSpeed;
    public float theEndAlphaSpeed;
    public float theEndMaxAlpha;
    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = Screen.height / 220;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float alturaCreditos = gameObject.GetComponent<RectTransform>().sizeDelta.x;

        Vector2 newPosition = new Vector2(0, +movementSpeed);
        gameObject.transform.Translate(newPosition);
        Debug.Log(gameObject.transform.position.y);
        if(gameObject.transform.position.y > (alturaCreditos+310f + Screen.height)/2)
        {
            if(theEnd.activeSelf == false)
            {
                movementSpeed = 0f;
                setTheEndAlpha((Color color, TMP_Text renderer) =>
                {
                    color.a = 0f;
                    renderer.color = color;
                    theEnd.SetActive(true);
                });
            }
            else
            {
                if(theEnd.GetComponent<TMP_Text>().color.a < theEndMaxAlpha)
                {
                    setTheEndAlpha((Color color, TMP_Text renderer) =>
                    {
                        color.a = color.a+theEndAlphaSpeed;
                        renderer.color = color;
                    });
                }
                else
                {
                    if (Input.anyKey)
                    {
                        Application.Quit();
                    }
                }
            }
        }
    }

    private void setTheEndAlpha(Action<Color, TMP_Text> funcao)
    {
        TMP_Text theEndRenderer = theEnd.GetComponent<TMP_Text>();
        Color color = theEndRenderer.color;
        funcao(color, theEndRenderer);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("Entrou na zona");
    //    movementSpeed = -2;
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Debug.Log("Saiu da zona");
    //    movementSpeed = -1f;
    //}
}
