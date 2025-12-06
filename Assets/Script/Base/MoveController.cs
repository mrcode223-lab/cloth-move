using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    // Start is called before the first frame update
    public int speed;
   
    protected virtual void Move(Vector3 directoin)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + directoin * speed * Time.deltaTime, 0.1f);
    }
    // Update is called once per frame
}
