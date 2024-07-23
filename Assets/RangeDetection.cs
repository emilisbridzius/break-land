using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetection : MonoBehaviour
{
    public BoxCollider mCollider;
    public EnemyBehaviour mController;

    void Start()
    {
        mCollider = GetComponent<BoxCollider>();
        mController = GetComponentInParent<EnemyBehaviour>();
    }

    void Update()
    {
        
    }
}
