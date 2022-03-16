using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int visited = -1;
    public int x = 0;
    public int y = 0;
    [SerializeField] private GameObject _highlight;

    void OnMouseOver()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);    
    }
}
