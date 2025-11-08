using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDebugger : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("PileDebugger Awake on: " + gameObject.name);
    }

    void Start()
    {
        Debug.Log("PileDebugger Start on: " + gameObject.name);
        // Log the hierarchy of this object
        LogGameObjectInfo(gameObject, 0);
    }

    void OnEnable()
    {
        Debug.Log("PileDebugger OnEnable on: " + gameObject.name);
    }

    void LogGameObjectInfo(GameObject obj, int depth)
    {
        if (obj == null)
        {
            Debug.LogError("Null GameObject passed to LogGameObjectInfo");
            return;
        }

        string indent = new string(' ', depth * 2);
        Debug.Log($"{indent}Checking object: {obj.name} (Active: {obj.activeInHierarchy})");

        var collider = obj.GetComponent<Collider2D>();
        var cardSelector = obj.GetComponent<CardSelector>();
        var cardDisplay = obj.GetComponent<CardDisplay>();

        Debug.Log($"{indent}{obj.name}:");
        Debug.Log($"{indent}  Layer: {LayerMask.LayerToName(obj.layer)}");
        Debug.Log($"{indent}  Has Collider: {collider != null} {(collider != null ? $"(enabled: {collider.enabled})" : "")}");
        Debug.Log($"{indent}  Has CardSelector: {cardSelector != null}");
        Debug.Log($"{indent}  Has CardDisplay: {cardDisplay != null}");
        Debug.Log($"{indent}  Child count: {obj.transform.childCount}");

        // Recursively log children
        foreach (Transform child in obj.transform)
        {
            LogGameObjectInfo(child.gameObject, depth + 1);
        }
    }
}
