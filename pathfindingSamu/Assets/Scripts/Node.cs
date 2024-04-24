using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int[] position {  get; set; }
    public float heuristica { get; set;}
    public int coste { get; set;}
    public Node hijo { get; set;}

    public Node(int[] position, Node hijo)
    {
        this.position = position;
        this.heuristica = Calculator.CheckDistanceToObj(position,GameManager.instance.objectivePos);
        this.coste = 1;
        this.hijo = hijo;
    }
   
}
