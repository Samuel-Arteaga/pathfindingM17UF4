using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int[] position {  get; set; }
    public float heuristica { get; set;}
    public int coste { get; set;}
    public Node hijo { get; set;}

    public Node(int[] position, Node padre)
    {
        this.position = position;
        this.heuristica = CalculateManhattanDistance(position, GameManager.instance.objectivePos); 
        this.coste = (padre != null) ? padre.coste + 1 : 0; // Inicializa el costo normal y (el costo del padre más 1)
        this.hijo = null;
    }

    private float CalculateManhattanDistance(int[] point1, int[] point2)
    {
        int dx = Mathf.Abs(point1[0] - point2[0]); 
        int dy = Mathf.Abs(point1[1] - point2[1]); 
        return dx + dy; 
    }


}
