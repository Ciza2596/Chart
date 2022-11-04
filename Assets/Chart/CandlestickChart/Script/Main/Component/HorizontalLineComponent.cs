using TMPro;
using UnityEngine;

namespace CandlestickChart
{
    public class HorizontalLineComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title_Text;
        [SerializeField] private Transform _transform;
        [SerializeField] private RectTransform _lineRectTransform;

        public void SetTitle(string title)
            => _title_Text.text = title;

        public void SetLocalPosition(Vector3 position) =>
            _transform.localPosition = position;

        public void SetLineSizeDelta(Vector2 sizeDelta) =>
            _lineRectTransform.sizeDelta = sizeDelta;
    }
}