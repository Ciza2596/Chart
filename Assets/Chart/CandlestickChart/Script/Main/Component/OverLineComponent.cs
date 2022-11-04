using System;
using TMPro;
using UnityEngine;

namespace CandlestickChart
{
    public class OverLineComponent : MonoBehaviour
    {
        //private variable
        [SerializeField] private Transform _transform;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _info_CanvasGroup;
        [Space] 
        [SerializeField] private TMP_Text _topValue_Value_Text;
        [SerializeField] private TMP_Text _bottomValue_Value_Text;
        private Action<float> _setSelectedLinePosX;


        public void Init(Vector2 localPosition, Vector2 sizeDelta, Vector2 rightDirection, int topValue, int bottomValue, Action<float> setSelectedLinePosX)
        {
            _info_CanvasGroup.alpha = 0;
            
            
            _transform.localPosition = localPosition;
            _rectTransform.sizeDelta = sizeDelta;
            _transform.right = rightDirection;

            _topValue_Value_Text.text = topValue.ToString();
            _bottomValue_Value_Text.text = bottomValue.ToString();

            _setSelectedLinePosX = setSelectedLinePosX;
        }

        public void OnEnter()
        {
            var position = _transform.position;
            _setSelectedLinePosX?.Invoke(position.x);

            _info_CanvasGroup.alpha = 1;
            transform.SetAsLastSibling();
        }

        public void OnExit() =>
            _info_CanvasGroup.alpha = 0;
    }
}