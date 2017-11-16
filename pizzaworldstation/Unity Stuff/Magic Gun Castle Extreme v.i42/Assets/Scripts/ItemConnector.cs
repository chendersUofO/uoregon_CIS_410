﻿using UnityEngine;

public class ItemConnector : MonoBehaviour {
    public string[] Tags;
    public bool IsDefault;

    void OnDrawGizmos() {
        var scale = 1.0f;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * scale);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * scale);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.right * scale);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.125f);
    }
}