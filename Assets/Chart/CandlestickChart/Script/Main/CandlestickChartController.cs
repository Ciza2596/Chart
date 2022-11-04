using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CandlestickChart
{
    public class CandlestickChartController : MonoBehaviour
    {
        //private variable
        [SerializeField] private TMP_Text _xAxis_Title_Text;
        [SerializeField] private TMP_Text _xAxis_Subtitle_Text;
        [Space] [SerializeField] private TMP_Text _yAxis_Title_Text;

        [Space] [SerializeField] private CandlestickChart _candlestickChart;

        //public method
        public void SetXAxisHead(string title, string subtitle)
        {
            _xAxis_Title_Text.text = title;
            _xAxis_Subtitle_Text.text = subtitle;
        }

        public void SetYAxisTitle(string yAxisTitle) => _yAxis_Title_Text.text = yAxisTitle;

        public void DrawChart(List<CandlestickChart.ChartData> chartDatas) =>
            _candlestickChart.DrawChart(chartDatas);

        public void ClearChart() => _candlestickChart.ClearChart();
    }
}