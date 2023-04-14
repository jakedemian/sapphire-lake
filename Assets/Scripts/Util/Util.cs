using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {
    public static float GetPercentToTarget(Vector2 a, Vector2 b, Vector2 x) {
        Vector2 aToX = x - a;
        if (aToX.x < 0f || aToX.y < 0f) {
            return 0f;
        }

        return Vector2.Distance(a, x) / (Vector2.Distance(a, b));
    }

    public static Transform GetChildWithTag(string tag, Transform transform) {
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            if (child.CompareTag(tag)) {
                return child;
            }
        }

        return null;
    }

    public static IEnumerator DoActionAfterDelay(float delaySeconds, Action callback) {
        yield return new WaitForSeconds(delaySeconds);
        callback();
    }

    public static IEnumerator DoActionEndOfFrame(Action callback) {
        yield return new WaitForEndOfFrame();
        callback();
    }

    public static Dictionary<string, int> MergeDictionaries(Dictionary<string, int> dict1, Dictionary<string, int> dict2) {
        Dictionary<string, int> result = new Dictionary<string, int>(dict1);

        foreach (KeyValuePair<string, int> item in dict2) {
            if (result.ContainsKey(item.Key)) {
                result[item.Key] += item.Value;
            } else {
                result.Add(item.Key, item.Value);
            }
        }
        return result;
    }
}



