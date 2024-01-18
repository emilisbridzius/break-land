using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneVision : MonoBehaviour
{
    [SerializeField] string targetTag = "Targetable";
    [SerializeField] public LayerMask obstructionLayer;

    RaycastHit hit;

    void Update()
    {
        // Cast a ray from the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~obstructionLayer))
        {
            // Check if the hit object has the specified tag
            if (hit.collider.CompareTag(targetTag))
            {
                // Enforce your condition on the object with the specified tag
                EnforceCondition(hit.collider.gameObject);
            }
        }

    }

    void EnforceCondition(GameObject target)
    {
        hit.collider.gameObject.TryGetComponent(out MeshRenderer mesh);
        mesh.material.color = Color.white;
    }
}