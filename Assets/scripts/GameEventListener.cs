using UnityEngine;
using UnityEngine.Events; 

public class GameEventListener : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    private GameEvent gameEvent; 
    [SerializeField]
    private UnityEvent response; 
    #pragma warning restore 0649
    private void OnEnable() 
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable() 
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised() 
    {
        response.Invoke();
    }
}
