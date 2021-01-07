using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
        ResManager.Instance.LoadAsync(ResPath.testCube, (item) =>
        {
            var go = Instantiate(item as GameObject);
            go.name = "loadSuccess";
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
