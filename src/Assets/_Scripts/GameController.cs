using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public bool isPlayerOne;

    public bool gameStarted = false;
    public int mapaX = 9;
    public int mapaY = 5;

    int[,] mapa;
    int[,] auxMapa;
    int[,] localConstruivel;

    public List<GameObject> repetidor = new List<GameObject>();
    public List<GameObject> nuvens = new List<GameObject>();

    public GameObject[] radius;
    public GameObject[] anthen;
    public List<GameObject> vizinhos = new List<GameObject>();
    public List<GameObject> ligacoes = new List<GameObject>();
    bool finalizaVizinhos;

    // Use this for initialization
    void Start () {
        CriaAMatriz();
    }
    private void Update()
    {
        
    }
    public void GetTowerCoordenates(int column, int line, int tamanhoX, int tamanhoY)
    {
        localConstruivel = new int[mapaY, mapaX];
        foreach (GameObject o in radius)
        {
            Coordenadas coordenadas = o.GetComponent<Coordenadas>();
            if (!coordenadas.isConectado)
            {
                var colum = coordenadas.column;
                var linha = coordenadas.line;
                mapa[linha, colum] = 1;
            }
        }
        foreach (GameObject o in anthen)
        {
            var colum = o.GetComponent<Coordenadas>().column;
            var linha = o.GetComponent<Coordenadas>().line;
            mapa[linha, colum] = 2;
            localConstruivel[linha, colum] = 1;
            Debug.Log("Teste");
        }
        auxMapa = mapa; //Auxiliar do mapa para apagar raios que ja foram verificado seus vizinhos
        //Chama a funcao para ver os vizinhos
        verificaVizinhos(column, line, tamanhoX, tamanhoY);
    }
    
    void verificaVizinhoDoVizinho(List<GameObject> vizinhosDosVizinhos)
    {
        if(vizinhosDosVizinhos.Count > 0)
        {
            for( int i = 0; i < vizinhosDosVizinhos.Count; i++)
            {
                Coordenadas coor = vizinhosDosVizinhos[i].GetComponent<Coordenadas>();
                verificaVizinhos(coor.column, coor.line, coor.tamanhoX, coor.tamanhoY);
            }
        }
       
    }

    void verificaVizinhos(int column, int line, int tamanhoX, int tamanhoY)
    {
        bool achei = false; //Marcador para apagar da lista os vizinhos dos vizinhos que se encontraram


        //For para ver na horizontal
        for (int i = 1; i <= tamanhoX; i++)
        {
            // Verifica se esta dentro do tamanho da matriz
            if(column - tamanhoX >= 0)
            {
                if (auxMapa[line, column - tamanhoX] == 1)
                {
                    GameObject achado = ProcuraGOPosicao(radius, column - tamanhoY, line);
                    if(achado != null)
                    {
                        if (!achado.GetComponent<Coordenadas>().isConectado)
                        {
                            achei = true;
                            achado.SetActive(true);
                            vizinhos.Add(achado);
                            localConstruivel[line, column - tamanhoX] = 1;
                            auxMapa[line, column - tamanhoX] = 0;
                        }
                    }
                    
                }
                else
                {
                    achei = false;
                }
            }
            else
            {
                achei = false;
            }
            if (column + tamanhoX <= mapaX - 1)
            {
                if (auxMapa[line, column + tamanhoX] == 1)
                {
                    GameObject achado = ProcuraGOPosicao(radius, column + tamanhoX, line);
                    if (!achado.GetComponent<Coordenadas>().isConectado)
                    {
                        achei = true;
                        achado.SetActive(true);
                        vizinhos.Add(achado);
                        localConstruivel[line, column + tamanhoX] = 1;
                        auxMapa[line, column + tamanhoX] = 0;
                    }
                }
                else
                {
                    achei = false;
                }
            }
            else
            {
                achei = false;
            }
        }
        //For para ver na vertical
        for (int i = 1; i <= tamanhoY; i++)
        {
            if(line - tamanhoY >= 0)
            {
                if (auxMapa[line - tamanhoY, column] == 1)
                {
                    GameObject achado = ProcuraGOPosicao(radius, column, line - tamanhoY);
                    if (!achado.GetComponent<Coordenadas>().isConectado)
                    {
                        achei = true;
                        achado.SetActive(true);
                        vizinhos.Add(achado);
                        localConstruivel[line - tamanhoY, column] = 1;
                        auxMapa[line - tamanhoY, column] = 0;
                    }
                        
                }
                else
                {
                    achei = false;
                }
            }
            else
            {
                achei = false;
            }
            if (line + tamanhoY <= mapaY - 1)
            {
                if (auxMapa[line + tamanhoY, column] == 1)
                {
                    GameObject achado = ProcuraGOPosicao(radius, column, line + tamanhoY);
                    if (!achado.GetComponent<Coordenadas>().isConectado)
                    {
                        achei = true;
                        achado.SetActive(true);
                        vizinhos.Add(achado);
                        localConstruivel[line + tamanhoY, column] = 1;
                        auxMapa[line + tamanhoY, column] = 0;
                    }
                        
                }
                else
                {
                    achei = false;
                }
            }
            else
            {
                achei = false;
            }
        }

        foreach(GameObject ativar in ligacoes)
        {
            ativar.SetActive(true);
        }
        //for para ver na diagonal
        if (!achei)
        {
            vizinhos.Remove(ProcuraGOPosicao(vizinhos, column, line));
        }
        if (vizinhos.Count > 0)
        {
            finalizaVizinhos = false;
        }
        else
        {
            finalizaVizinhos = true;
        }
        if (!finalizaVizinhos)
        {
            verificaVizinhoDoVizinho(vizinhos);
        }
    }

    public void DisableActivedRadiusAll()
    {
        foreach (GameObject ativar in radius)
        {
            if(ativar.activeSelf && !ativar.GetComponent<Coordenadas>().isConectado)
                ativar.SetActive(false);
        }
    }
    public void EnableActivedRadiusAll()
    {
        foreach (GameObject ativar in radius)
        {
            if (!ativar.activeSelf && !ativar.GetComponent<Coordenadas>().isConectado)
                ativar.SetActive(true);
        }
    }
    public bool VerificaPossibilidadeContrucao(int column, int line)
    {
        if(column >= 0 && column <= mapaX - 1 && line <= mapaY - 1 && line >= 0)
        {
            if (localConstruivel[line, column] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else{
            return false;
        }
       
    }
    GameObject ProcuraGOPosicao(GameObject[] go, int coluna, int linha)
    {
       foreach(GameObject gameObj in go)
        {
            if (gameObj.GetComponent<Coordenadas>().column == coluna && gameObj.GetComponent<Coordenadas>().line == linha)
            {
                return gameObj;
            }
        }
        return null;
    }
    GameObject ProcuraGOPosicao(List<GameObject> go, int coluna, int linha)
    {
        foreach (GameObject gameObj in go)
        {
            if (gameObj.GetComponent<Coordenadas>().column == coluna && gameObj.GetComponent<Coordenadas>().line == linha)
            {
                return gameObj;
            }
        }
        return null;
    }
    public void AddItemInCordanate(int item, int coluna, int linha, Vector2 position)
    {
        switch (item)
        {
            case 1:
                GameObject objRaio = ProcuraGOPosicao(radius, coluna, linha);
                GameObject objAntena = ProcuraGOPosicao(anthen, coluna, linha);
                if (objRaio == null && objAntena == null)
                {
                    for(int i = 0; i < repetidor.Count; i++)
                    {
                        if (repetidor[i].activeSelf == false)
                        {
                            repetidor[i].SetActive(true);
                            repetidor[i].transform.position = position;
                            repetidor[i].GetComponent<Coordenadas>().column = coluna;
                            repetidor[i].GetComponent<Coordenadas>().line = linha;
                            repetidor[i].GetComponent<Coordenadas>().tamanhoX = 1;
                            repetidor[i].GetComponent<Coordenadas>().tamanhoY = 1;
                            GetTowerCoordenates(coluna, linha, 1, 1);
                            break;
                        }
                    }
                }
                else
                {
                    Debug.Log("Nao pode por ai");
                }
                break;
            case 2:
                break;
        }
    }
    public void CriaAMatriz()
    {
        mapa = new int[mapaY, mapaX];

        finalizaVizinhos = false;
        radius = GameObject.FindGameObjectsWithTag(isPlayerOne ? "1 - Player1" : "1 - Player2");
        anthen = GameObject.FindGameObjectsWithTag(isPlayerOne ? "2 - Player1" : "2 - Player2");

        foreach (GameObject o in radius)
        {
            Coordenadas coordenadas = o.GetComponent<Coordenadas>();
            if (!coordenadas.isConectado)
            {
                var column = coordenadas.column;
                var linha = coordenadas.line;
                mapa[linha, column] = 1;
            }

        }
        foreach (GameObject o in anthen)
        {
            var column = o.GetComponent<Coordenadas>().column;
            var linha = o.GetComponent<Coordenadas>().line;
            mapa[linha, column] = 2;
        }
        DisableActivedRadiusAll();
    }
}
