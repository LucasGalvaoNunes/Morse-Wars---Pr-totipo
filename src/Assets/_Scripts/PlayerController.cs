using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    bool isJoystick = false;
    Transform playerTransform;                  // Tranform of player
    public GameController gameController;       // Script of Tower selected
    public GameObject tower;                           // GameObject of Tower Selected
    GameObject radius;
    GameObject Origem;
    GameObject floor;
    public List<GameObject> ligacao = new List<GameObject>();
    public List<GameObject> ligacaoAux = new List<GameObject>();
    List<string> indexHistoricoLigacao = new List<string>();
    public Transform areaPlayer;
    public float velocidadeArea = 4;
    public float areaInit, areaFim;
    public int horizontalCont;
    public int verticalCont;
    public GeralController geral;


    public bool isPlayerOne;            // if is player one
    bool freeMove;                          // If cursor free move
    bool editMove;
    bool addMove;
    int op;
    bool nextHorizontal = true;
    bool nextVertical = true;
    public bool isTower = true;

    public float initPositionY, finPositionY, initPositionX, finPositionX, cursorVel; // Screen end points and velocity of cursor/

    // Use this for initialization
    void Start()
    {
        op = 0;
        playerTransform = gameObject.GetComponent<Transform>();
        freeMove = true;
        editMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (geral.gameStarted) {
            if (freeMove)
            {

                if (Input.GetKeyDown((isPlayerOne) ? KeyCode.Joystick1Button7 : KeyCode.Joystick2Button7))
                {

                    if (op > 1)
                    {
                        if (floor != null)
                        {
                            floor.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        addMove = false;
                        op = 0;
                    }
                    else
                    {
                        addMove = true;
                        op++;
                    }
                    Debug.Log(op);
                }
            }
            ActiveTower();
        }
        
    }
    /**
     * Move cursor of player in game
     */
    
    void ActiveTower()
    {
        if(tower != null)
        {
            if (freeMove)   // Se existe uma torre e ele esta em movimento livre e aperta X habilita a edicao.
            {
                if (Input.GetKeyDown((isPlayerOne) ? KeyCode.Joystick1Button1 : KeyCode.Joystick2Button1))
                {
                    if (tower.GetComponent<Coordenadas>().isConectado)
                    {
                        Coordenadas coor = tower.GetComponent<Coordenadas>();
                        gameController.GetTowerCoordenates(coor.column, coor.line, coor.tamanhoX, coor.tamanhoY);
                        playerTransform.position = tower.transform.position;
                        Origem = tower;
                        isTower = true;
                        freeMove = false;
                        editMove = true;
                    }
                    
                }
            }

            //Se ele esta em edit e aperta bolinha sai do modo edicao
            if(editMove)
            {
                
                if (Input.GetKeyDown((isPlayerOne) ? KeyCode.Joystick1Button1 : KeyCode.Joystick2Button1))
                {
                    if (isTower)
                    {
                        Coordenadas coor = tower.GetComponent<Coordenadas>();
                        gameController.GetTowerCoordenates(coor.column, coor.line, coor.tamanhoX, coor.tamanhoY);
                        playerTransform.position = tower.transform.position;
                        Origem = tower;
                        isTower = true;
                        freeMove = false;
                        editMove = true;
                        if (Origem != null)
                        {
                            ligacaoAux.Add(tower);
                            ligacao = ligacaoAux;
                            for (int i = 0; i < ligacao.Count; i++)
                            {
                                tower.GetComponent<SpriteRenderer>().color = Color.green;
                                ligacao[i].GetComponent<Coordenadas>().isConectado = true;
                            }
                        }
                    }
                    
                }
                if (Input.GetKeyDown((isPlayerOne) ? KeyCode.Joystick1Button4 : KeyCode.Joystick2Button4))
                {
                    //Apaga historico
                }

                EditMove();
            }
        }
    }
    void EditMove()
    {

        var horizontal = (isPlayerOne) ? Input.GetAxis("HorizontalArrowPlayer1") : Input.GetAxis("HorizontalArrowPlayer2");
        var vertical = (isPlayerOne) ? Input.GetAxis("VerticalArrowPlayer1") : Input.GetAxis("VerticalArrowPlayer2");

        if (horizontal > 0 && nextHorizontal)
        {
            verticalCont = 0;
            horizontalCont++;
            Coordenadas coordenadas;
            if (isTower)
            {
                coordenadas = tower.GetComponent<Coordenadas>();
            }
            else
            {
                coordenadas = radius.GetComponent<Coordenadas>();
            }
            if(gameController.VerificaPossibilidadeContrucao(coordenadas.column + 1, coordenadas.line))
            {
                playerTransform.position = new Vector2(playerTransform.position.x + 1, playerTransform.position.y);
                nextHorizontal = false;
            }
            
        }else if(horizontal < 0 && nextHorizontal)
        {
            
            Coordenadas coordenadas;
            if (isTower)
            {
                coordenadas = tower.GetComponent<Coordenadas>();
            }
            else
            {
                coordenadas = radius.GetComponent<Coordenadas>();
            }
            if (gameController.VerificaPossibilidadeContrucao(coordenadas.column - 1, coordenadas.line))
            {
                verticalCont = 0;
                horizontalCont--;
                playerTransform.position = new Vector2(playerTransform.position.x - 1, playerTransform.position.y);
                nextHorizontal = false;
            }
                
        }
        else if (horizontal == 0)
        {
            nextHorizontal = true;
        }

        if (vertical > 0 && nextVertical)
        {
            Coordenadas coordenadas;
            if (isTower)
            {
                coordenadas = tower.GetComponent<Coordenadas>();
            }
            else
            {
                coordenadas = radius.GetComponent<Coordenadas>();
            }
            if (gameController.VerificaPossibilidadeContrucao(coordenadas.column, coordenadas.line - 1))
            {
                verticalCont++;
                horizontalCont = 0;
                playerTransform.position = new Vector2(playerTransform.position.x, playerTransform.position.y + 1);
                nextVertical = false;
            }
               
        }
        else if (vertical < 0 && nextVertical)
        {
            Coordenadas coordenadas;
            if (isTower)
            {
                coordenadas = tower.GetComponent<Coordenadas>();
            }
            else
            {
                coordenadas = radius.GetComponent<Coordenadas>();
            }
            if (gameController.VerificaPossibilidadeContrucao(coordenadas.column, coordenadas.line + 1))
            {
                verticalCont--;
                horizontalCont = 0;
                playerTransform.position = new Vector2(playerTransform.position.x, playerTransform.position.y - 1);
                nextVertical = false;
            }
               
        }
        else if (vertical == 0)
        {
            nextVertical = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tagFloor = isPlayerOne ? "Floor - Player1" : "Floor - Player2";
        if (addMove)
        {
            if (Input.GetKeyDown((isPlayerOne) ? KeyCode.Joystick1Button1 : KeyCode.Joystick2Button1))
            {
                if (collision.gameObject.tag == tagFloor)
                {
                    Debug.Log("Clickou certo!");
                    Coordenadas coor = collision.gameObject.GetComponent<Coordenadas>();
                    gameController.AddItemInCordanate(op, coor.column, coor.line, collision.gameObject.transform.position);
                }

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag1 = isPlayerOne ? "1 - Player1" : "1 - Player2";
        string tag2 = isPlayerOne ? "2 - Player1" : "2 - Player2";
        string tagFloor = isPlayerOne ? "Floor - Player1" : "Floor - Player2";
        if (freeMove)
        {
            string tag = isPlayerOne ? "2 - Player1" : "2 - Player2";
            if (collision.gameObject.tag == tag)
            {
                tower = collision.gameObject;
                isTower = true;
            }
        }
        if (editMove)
        {
           
            if (collision.gameObject.tag == tag2)
            {
                isTower = true;
                tower = collision.gameObject;
                if(collision.gameObject == Origem)
                {
                    for (int i = 0; i < ligacaoAux.Count; i++)
                    {
                        if(!ligacaoAux[i].GetComponent<Coordenadas>().isConectado)
                            ligacaoAux[i].GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                    ligacaoAux = new List<GameObject>();
                }
            }
            
            else if (collision.gameObject.tag == tag1)
            {
                radius = collision.gameObject;
                if (!radius.GetComponent<Coordenadas>().isConectado)
                {
                    isTower = false;
                    ligacaoAux.Add(radius);
                }
            }
            else
            {
                verticalCont = 0;
                horizontalCont = 0;
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag2 = isPlayerOne ? "2 - Player1" : "2 - Player2";

        if (freeMove)
        {
            if(collision.gameObject.tag == tag2)
            {
                tower = null;
            }
        }
        if (editMove)
        {
            if (collision.gameObject.tag == tag2)
            {
                isTower = false;
            }
        }

    }
}
