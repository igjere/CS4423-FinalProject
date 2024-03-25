using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SetText : MonoBehaviour
{

    TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "Your text here";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
