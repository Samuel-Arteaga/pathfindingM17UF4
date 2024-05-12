using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject token1, token2, token3;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    public int[] objectivePos = new int[2];

    List<Node> listOberta = new List<Node>();
    List<Node> listTancada = new List<Node>();

    private void Start()
    {
        listOberta.AddRange(next(new Node(startPos, null)));

        // Pintar token3 en las cuatro direcciones adyacentes
        PaintToken3InAllDirections(startPos);
    }
    public Node[] next(Node node)
    {
        Node[] nodes = new Node[4];
        nodes[0] = new Node(new int[] { node.position[0] + 1, node.position[1]},node);
        nodes[1] = new Node(new int[] { node.position[0] - 1, node.position[1]},node);
        nodes[2] = new Node(new int[] { node.position[0], node.position[1] - 1},node);
        nodes[3] = new Node(new int[] { node.position[0], node.position[1] + 1},node);

        return nodes;
    }
    private void Awake()
    {
        instance = this;
        GameMatrix = new int[Calculator.length, Calculator.length];

        for (int i = 0; i < Calculator.length; i++) //fila
            for (int j = 0; j < Calculator.length; j++) //columna
                GameMatrix[i, j] = 0;

        //randomitzar pos final i inicial;
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        startPos[0] = rand1;
        startPos[1] = rand2;
        SetObjectivePoint(startPos);

        GameMatrix[startPos[0], startPos[1]] = 1;
        GameMatrix[objectivePos[0], objectivePos[1]] = 2;

        InstantiateToken(token1, startPos);
        
        InstantiateToken(token2, objectivePos);

        ShowMatrix();
    }
    private void InstantiateToken(GameObject token, int[] position)
    {
        Instantiate(token, Calculator.GetPositionFromMatrix(position), Quaternion.identity);
    }
    private void SetObjectivePoint(int[] startPos) 
    {
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        if (rand1 != startPos[0] || rand2 != startPos[1])
        {
            objectivePos[0] = rand1;
            objectivePos[1] = rand2;
        }
    }
    private void ShowMatrix() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < Calculator.length; i++)
        {
            for (int j = 0; j < Calculator.length; j++)
            {
                matrix += GameMatrix[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }
    //EL VOSTRE EXERCICI COMENÇA AQUI
    private void Update()
    {
        if (!EvaluateWin())
        {
            Node[] adjacentNodes = next(new Node(startPos, null));

            // Inicializar la distancia mínima y el nodo con la distancia mínima
            float minDistance = Mathf.Infinity;
            Node minNode = null;

            // Calcular la distancia mínima entre los nodos adyacentes y el objetivo final
            foreach (Node node in adjacentNodes)
            {
                float distanceToObjective = node.heuristica;
                if (distanceToObjective < minDistance)
                {
                    minDistance = distanceToObjective;
                    minNode = node;
                    if (minDistance == 0)
                    {
                        //Debug.Log("Has ganado!");
                        return;
                    }
                }
            }
            if (minNode != null)
            {
                //listOberta.Add(minNode);
                listTancada.Add(minNode);
                Debug.Log("lista tancada: " + listTancada.Count);
                foreach (Node node in listTancada)
                {
                    Debug.Log("Posición del nodo: " + node.position[0] + ", " + node.position[1]);
                    Debug.Log("Heurística del nodo: " + node.heuristica);
                    Debug.Log("Costo del nodo: " + node.coste);
                    //Debug.Log("Total: "+ node.heuristica + node.coste);
                    Debug.Log("!!-------------------------------------------!!");
                }
                MovePlayerToPosition(minNode.position);
                PaintToken3InAllDirections(minNode.position);
                //listOberta.Add(minNode);
                //Debug.Log("lista Oberta: "+ listOberta.Count);
            }
        }
    }
    private void MovePlayerToPosition(int[] newPosition)
    {
        startPos = newPosition;
    }
    private bool EvaluateWin()
    {
        return false;
    }

    private void PaintToken3InAllDirections(int[] position)
    {
        int[][] directions = new int[][] { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 } };

        foreach (var dir in directions)
        {
            int newX = position[0] + dir[0];
            int newY = position[1] + dir[1];

            if (newX >= 0 && newX < Calculator.length && newY >= 0 && newY < Calculator.length)
            {
                InstantiateToken(token3, new int[] { newX, newY });
                

            }
        }
    }
}
