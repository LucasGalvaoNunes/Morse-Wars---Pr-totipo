using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeralController : MonoBehaviour {
    public bool gameStarted;
    public bool gameOver;
    public Transform vitoria1, vitoria2, start;

    public bool playerOneAccepted;
    public bool playerTwoAccepted;
    public float timer = 3;
    float oldTimer;
    public GameObject textPressPlayerOnex, textPressPlayerTwox, textTempo, porta1, porta2;
    public Coordenadas finalPlayerOne;
    public Coordenadas finalPlayerTwo;


    // Use this for initialization
    void Start () {
        oldTimer = timer;
        gameOver = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                playerOneAccepted = true;
                textPressPlayerOnex.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Joystick2Button1))
            {
                playerTwoAccepted = true;
                textPressPlayerTwox.SetActive(false);
            }
            if (playerOneAccepted && playerTwoAccepted)
            {
                textTempo.SetActive(true);
                timer -= Time.deltaTime;
                textTempo.GetComponent<Text>().text = (Mathf.RoundToInt(timer).ToString() == "0") ? "Vai!" : Mathf.RoundToInt(timer).ToString();
                if (timer < 0)
                {
                    textTempo.SetActive(false);
                    gameStarted = true;
                }
            }
        }
        if (!gameOver)
        {
            if (finalPlayerOne.ItFinal && finalPlayerOne.isConectado)
            {
                vitoria1.position = new Vector2(-4.5f, vitoria1.position.y);
                vitoria1.gameObject.SetActive(true);
                gameStarted = false;
                gameOver = true;
                porta2.SetActive(true);
            }
            if (finalPlayerTwo.ItFinal && finalPlayerTwo.isConectado)
            {
                vitoria2.position = new Vector2(4.5f, vitoria2.position.y);
                vitoria2.gameObject.SetActive(true);
                gameStarted = false;
                gameOver = true;
                porta1.SetActive(true);
            }
        }
        else {
            start.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Joystick1Button9))
            {
                SceneManager.LoadScene(0);
            }
        }
        
    }
}
