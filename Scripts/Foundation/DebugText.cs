using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    private static List<string> texts = new List<string>();
    public static Text debug_text;

    void Awake() {
        debug_text = GameObject.Find("DebugText").GetComponent<Text>();
    }

    public static void SetText(int idx, string text) {
        if(idx < 0) return;

        if(idx == texts.Count) {
            texts.Insert(idx, text);
        } 
        else if (idx > texts.Count) {
            for(int i = texts.Count; i <= idx; i++) {
                texts.Insert(i,"");
            }
            texts[idx] = text;
        }
        else {
            texts[idx] = text;
        }
        UpdateDebugText();
    }

    private static void UpdateDebugText() {
        if(debug_text != null) {
            string result = "";
            foreach(string t in texts) {
                result += t + "\n";
            }
            debug_text.text = result;
        }
    }
}
