using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKVA
{
    public class InteractableBox : MonoBehaviour
    {
        public bool isColored;
        public Color boxColor;
        Material material;
        void Start()
        {
            if(isColored)
            {
                material = GetComponent<Renderer>().material;
                material.color = boxColor;
            }
        }
    }
}
