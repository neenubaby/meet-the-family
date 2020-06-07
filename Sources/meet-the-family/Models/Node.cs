using System;
using System.Collections;
using System.Collections.Generic;

public class Node
{
    public string Name;
    public Gender Gender;
    public Dictionary<Parent, Node> Parents;
    public Node Spouse;
    public List<Node> Children;
    public Node()
    {
        Parents = new Dictionary<Parent, Node>();
        Children = new List<Node>();
    }
}