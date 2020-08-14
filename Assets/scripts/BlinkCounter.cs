using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkCounter : MonoBehaviour
{
    int blink = 0;
    Text txt;
    bool activate = true;
    void Start () {
         txt = gameObject.GetComponent<Text>(); 
    }

    public void OnBlinked() {
        if(!activate) {
            return;
        }
        StartCoroutine(DelayedEffect());
    }

    IEnumerator DelayedEffect() {
        activate = false;
        blink++;
        txt.text = blink.ToString();
        yield return new WaitForSeconds(1);
        activate = true;
    }
}
