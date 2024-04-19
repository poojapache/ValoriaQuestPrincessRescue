using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public bool flag;
    private void Update()
    {
        if(flag) Destroy(this.gameObject);
    }

}
