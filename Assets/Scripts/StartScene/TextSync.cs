using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSync : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text drfText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = drfText.text;
    }
}
