using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LoadingCircle : MonoBehaviour
    {
        private RectTransform rectComponent;
        private float rotateSpeed = 200f;

        private void Awake()
        {
            rectComponent = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        }
    }
}
