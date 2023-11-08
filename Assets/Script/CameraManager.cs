using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject target;
    public float height;
   public float distance;
    Vector3 pos;

	// Use this for initialization
	void Start () {
        pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        //pos.x = Mathf.Lerp(pos.x, target.transform.position.x, Time.deltaTime);
        pos.x = target.transform.position.x;
        pos.y = Mathf.Lerp(pos.y, target.transform.position.y + height, Time.deltaTime);
        //pos.z = Mathf.Lerp(pos.z, target.transform.position.z - distance, Time.deltaTime);
        pos.z = target.transform.position.z - distance;
        transform.position = pos;
    }
}
