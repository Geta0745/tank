using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInfo : MonoBehaviour
{
    public TMP_Text speedText;
    public TMP_Text statusText;
    public AIMain ai;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = ai.GetMovement();
        string statusTurn = "None";
        string statusMove = "None";
        speedText.text = "Move Factor : " + movement.y.ToString() + " Turn Factor : " + movement.x.ToString();

        if(movement.x > 0.1f && movement.x <= 0.8f){
            statusTurn = "Left Turn";
        }else if(movement.x < -0.1 && movement.x > -0.8f){
            statusTurn = "Right Turn";
        }else if(movement.x == 1){
            statusTurn = "Full Left Turn";
        }else if(movement.x == -1f){
            statusTurn = "Full Right Turn";
        }

        if(movement.y > 0){
            statusMove = "Forward";
        }else if(movement.y < 0){
            statusMove = "Backward";
        }

        statusText.text = "Movement State : " + statusMove + " Turn State : " + statusTurn +" Obstacle Status : " + ai.CheckObstacle();
    }
}
