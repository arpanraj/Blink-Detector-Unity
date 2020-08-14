using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkCounter : MonoBehaviour
{
    int blink = 0;
    Text txt;
    bool activate = true;
    #pragma warning disable 0649
    [SerializeField]
    float responseDelay = 0f;
    #pragma warning restore 0649
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
        yield return new WaitForSeconds(responseDelay);
        activate = true;
    }
}
