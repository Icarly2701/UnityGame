using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    GameObject go;
    public PlayerMove playerMove;
    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("MainCharacter");
        playerMove = go.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    
}
