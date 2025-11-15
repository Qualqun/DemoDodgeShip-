using TMPro;
using UnityEngine;

public class MaxScore : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = Utilities.maxScore.ToString();
    }
}
