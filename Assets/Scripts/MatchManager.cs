using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public GameObject aimIcon;
    public GameObject player;
    [SerializeField] bool ended;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null){
            Debug.Log("Destroyed");
            player = this.gameObject;
            ended = true;
        }
    }

    public void SpawnPlayer(){
        ended = false;
    }
}

