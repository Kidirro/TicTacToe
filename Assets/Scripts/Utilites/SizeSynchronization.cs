using System;
using TMPro;
using UnityEngine;

namespace Utilites
{
    public class SizeSynchronization: MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _target;

        [SerializeField]
        private TextMeshProUGUI _currentText;
        
        private void OnEnable()
        {
            _currentText.fontSize = _target.fontSize;
        }
    }
}