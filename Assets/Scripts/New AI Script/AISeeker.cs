using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AIMain))]
public class AISeeker : MonoBehaviour
{

    [SerializeField] string seekTag;
    [SerializeField] List<GameObject> foundObjects;
    [SerializeField] Transform target;
    [SerializeField] bool keepSeek = true;

    // Start is called before the first frame update
    void Start()
    {
        ChangeSeekTag();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSeekTag();
        if(target == null || keepSeek){
            SeekTarget();
            CompareTarget();
        }
    }

    void SeekTarget(){
        GameObject[] objects = GameObject.FindGameObjectsWithTag(seekTag);
        foundObjects = new List<GameObject>(objects);
    }

    void CompareTarget(){
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        foreach(GameObject obj in foundObjects){
            float distance = Vector3.Distance(transform.position,obj.transform.position);
            if(distance < closestDistance){
                closest = obj;
                closestDistance = distance;
            }
        }
        target = closest.transform;
    }

    void ChangeSeekTag()
    {
        if (gameObject.CompareTag("friendly"))
        {
            seekTag = "enemy";
        }
        else if (gameObject.CompareTag("enemy"))
        {
            seekTag = "friendly";
        }
    }

    public Transform GetTarget(){
        return target;
    }
}
