using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CandlestickChart
{
    public class CandlestickChart : MonoBehaviour
    {
        //private variable
        [Range(0.5f, 15)] [SerializeField] private float _shadow_Width = 2f;
        [Range(2, 15)] [SerializeField] private float _realBody_Width = 7f;

        [Space] [Range(1, 10)] [SerializeField]
        private float _horizontalLine_Width = 2f;

        [Space] [SerializeField] private Transform _selectedLineTransform;

        [Space] [SerializeField] private RectTransform _lineRectTransform;
        [SerializeField] private Transform _lineContainerTransform;
        [SerializeField] private RectTransform _lineContainerRectTransform;
        [SerializeField] private GameObject _markerDownPrefab;
        [SerializeField] private GameObject _markerUpPrefab;

        [Space] [SerializeField] private Transform _horizontalLineContainerTransform;
        [SerializeField] private GameObject _horizontalLinePrefab;


        private List<ChartData> _chartDatas;
        private int _columnNumber = 10;
        private int _horizontalLineNumber = 2;

        private List<OverLineComponent> _lines = new List<OverLineComponent>();

        private float _maxX;
        private float _maxY;

        private float _minX;
        private float _minY;


        //public method
        public void DrawChart(List<ChartData> chartDatas, int columnNumber = 100, int horizontalNumber = 2)
        {
            Assert.IsTrue(horizontalNumber > 1,
                          $"[CandlestickChart::DrawChart] HorizontalNumber is less 2.Current number{horizontalNumber}");

            _chartDatas = chartDatas;
            _columnNumber = columnNumber;
            _horizontalLineNumber = horizontalNumber;

            ClearChart();
            UpdateChart();
        }

        public void ClearChart()
        {
            foreach (var line in _lines)
                Destroy(line.gameObject);
        }


        //private method
        private void InitMaxMinXY()
        {
            var maxValue = -100000000;
            _maxX = maxValue;
            _maxY = maxValue;

            var minValue = 100000000;
            _minX = minValue;
            _minY = minValue;

            foreach (var chartData in _chartDatas)
            {
                _maxX = Mathf.Max(_maxX, chartData.X_TimePoint);
                _maxY = Mathf.Max(_maxY, chartData.Y_Max);

                _minX = Mathf.Min(_minX, chartData.X_TimePoint);
                _minY = Mathf.Min(_minY, chartData.Y_Min);
            }
        }

        private void UpdateChart()
        {
            InitMaxMinXY();

            var sizeDelta = _lineContainerRectTransform.sizeDelta;
            var width = sizeDelta.x;
            var height = sizeDelta.y;

            var tf_FactorA = height                     / (_maxY - _minY);
            var tf_FactorB = -_minY / (_maxY - _minY) * height;

            CreateHorizontalLines(_horizontalLineNumber, height, tf_FactorA, tf_FactorB);


            for (int i = 0; i < _columnNumber; i++)
            {
                var chartData = _chartDatas[i];

                var x_TimePoint = chartData.X_TimePoint;
                var y_Max = chartData.Y_Max;
                var y_Min = chartData.Y_Min;
                var y_Open = chartData.Y_Open;
                var y_Close = chartData.Y_Close;


                //get maximum point
                var maxPoint = new Vector3(x_TimePoint * width / _maxX           - width  / 2,
                                           y_Max       * tf_FactorA + tf_FactorB - height / 2, 0);
                //get the minimum point
                var minPoint = new Vector3(x_TimePoint * width / _maxX           - width  / 2,
                                            y_Min      * tf_FactorA + tf_FactorB - height / 2, 0);
                //get the open point
                var openPoint = new Vector3(x_TimePoint * width / _maxX           - width  / 2,
                                            y_Open      * tf_FactorA + tf_FactorB - height / 2, 0);
                //get the close point
                var closePoint = new Vector3(x_TimePoint * width / _maxX           - width  / 2,
                                             y_Close     * tf_FactorA + tf_FactorB - height / 2, 0);


                // create maxMiniLine
                var prefab = y_Close >= y_Open ? _markerUpPrefab : _markerDownPrefab;

                var direction = (maxPoint - minPoint) / 2;
                var localPosition = minPoint + direction;

                CreateOverLineComponent(prefab, y_Max, y_Min, direction, _shadow_Width, localPosition);


                // create openCloseLine
                var topValue = y_Close;
                var bottomValue = y_Open;

                direction = (closePoint - openPoint) / 2;
                localPosition = openPoint + direction;

                if (y_Close < y_Open)
                {
                    topValue = y_Open;
                    bottomValue = y_Close;

                    direction = (openPoint - closePoint) / 2;
                    localPosition = closePoint + direction;
                }

                CreateOverLineComponent(prefab, topValue, bottomValue, direction, _realBody_Width, localPosition);
            }
        }

        private void CreateHorizontalLines(int horizontalLineNumber, float height, float tf_FactorA,
                                           float tf_FactorB)
        {
            for (var i = 0; i <= horizontalLineNumber; i++)
            {
                var horizontalLine = Instantiate(_horizontalLinePrefab, Vector3.zero, Quaternion.Euler(0, 0, 0),
                                                 _horizontalLineContainerTransform);

                var horizontalLineComponent = horizontalLine.GetComponent<HorizontalLineComponent>();


                var horizontalValue = (float)i / horizontalLineNumber * (_maxY                 - _minY) + _minY;
                var localPosition = new Vector3(0, (horizontalValue) * tf_FactorA + tf_FactorB - height / 2, 0);
                horizontalLineComponent.SetLocalPosition(localPosition);


                var width = _lineRectTransform.sizeDelta.x;
                var sizeDelta = new Vector2(width, _horizontalLine_Width);
                horizontalLineComponent.SetLineSizeDelta(sizeDelta);


                var title = "$" + Mathf.Round(horizontalValue);
                horizontalLineComponent.SetTitle(title);

                horizontalLine.transform.right = transform.right;
            }
        }

        private void CreateOverLineComponent(GameObject prefab, float topValue, float bottomValue,
                                             Vector3 direction, float width, Vector3 localPosition)
        {
            var overLine = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0),
                                       _lineContainerTransform);
            var overlineComponent = overLine.GetComponent<OverLineComponent>();


            var sizeDelta = new Vector2(2 * direction.magnitude, width);
            overlineComponent.Init(localPosition, sizeDelta, direction, (int)topValue, (int)bottomValue,
                                   UpdateSelectedLine);
        }


        private void UpdateSelectedLine(float posX)
        {
            var position = _selectedLineTransform.position;
            _selectedLineTransform.position = new Vector3(posX, position.y, position.z);
        }

        //model
        public struct ChartData
        {
            //public variable
            public float X_TimePoint;

            public float Y_Open;
            public float Y_Close;

            public float Y_Min;
            public float Y_Max;
            
            //public method
            public ChartData(float x_TimePoint, float y_Open, float y_Close, float yMin, float y_Max)
            {
                X_TimePoint = x_TimePoint;
                Y_Open = y_Open;
                Y_Close = y_Close;
                Y_Min = yMin;
                Y_Max = y_Max;
            }
        }
    }
}