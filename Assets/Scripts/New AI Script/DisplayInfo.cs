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
        string statusTurn = "None";
        string statusMove = "None";
        speedText.text = "Move Factor : " + ai.movement.y.ToString() + " Turn Factor : " + ai.movement.x.ToString() +" ;";

        if(ai.movement.x > 0.1f && ai.movement.x <= 0.8f){
            statusTurn = "Left Turn";
        }else if(ai.movement.x < -0.1 && ai.movement.x > -0.8f){
            statusTurn = "Right Turn";
        }else if(ai.movement.x == 1){
            statusTurn = "Full Left Turn";
        }else if(ai.movement.x == -1f){
            statusTurn = "Full Right Turn";
        }

        if(ai.movement.y > 0){
            statusMove = "Forward";
        }else if(ai.movement.y < 0){
            statusMove = "Backward";
        }

        statusText.text = "Movement State : " + statusMove + " Turn State : " + statusTurn;
    }
}
