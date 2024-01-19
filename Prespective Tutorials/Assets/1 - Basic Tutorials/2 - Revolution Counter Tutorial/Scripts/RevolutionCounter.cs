using UnityEngine;

public class RevolutionCounter : MonoBehaviour
{
    public int Count = 0;
    private bool init = false;

    public void CountRevolution()
    {
        if (init)
        {
            Count++;
            Debug.Log("Counted " + Count);
        }
        else
        {
            Count = 0;
            init = true;
            Debug.Log("Start counting...");
        }
        
    }
}
