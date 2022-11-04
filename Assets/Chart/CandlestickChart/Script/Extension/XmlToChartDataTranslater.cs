using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace CandlestickChart
{
    public class XmlToChartDataTranslater : MonoBehaviour
    {
        //private variable
        [SerializeField] private TextAsset _txtXmlAsset;

        [SerializeField] private CandlestickChart _candlestickChart;


        //unity callback
        private void OnEnable()
        {
            var charData = GetChartDatas(_txtXmlAsset);
            _candlestickChart.DrawChart(charData);
        }


        //private method
        private List<CandlestickChart.ChartData> GetChartDatas(TextAsset txtXmlAsset)
        {
            var doc = XDocument.Parse(txtXmlAsset.text);


            //get the xml data points
            var allDict = doc.Element("data").Elements("point");
            var chartDatas = new List<CandlestickChart.ChartData>();
            var x_key = "x";
            var yo_key = "yo";
            var xc_key = "yc";
            var xm_key = "ym";
            var xM_key = "yM";
            foreach (var dict in allDict)
            {
                var x_TimePointString = GetXElement(dict, x_key);
                var y_OpenString = GetXElement(dict, yo_key);
                var y_CloseString = GetXElement(dict, xc_key);
                var y_MiniString = GetXElement(dict, xm_key);
                var y_MaxString = GetXElement(dict, xM_key);

                var x_TimePoint = GetXElementFloat(x_TimePointString, x_key);
                var y_Open = GetXElementFloat(y_OpenString, yo_key);
                var y_Close = GetXElementFloat(y_CloseString, xc_key);
                var y_Mini = GetXElementFloat(y_MiniString, xm_key);
                var y_Max = GetXElementFloat(y_MaxString, xM_key);


                var chartData = new CandlestickChart.ChartData(x_TimePoint, y_Open, y_Close, y_Mini, y_Max);
                chartDatas.Add(chartData);
            }

            return chartDatas;
        }


        private XElement GetXElement(XElement dict, string xElementKey)
        {
            return dict.Elements(xElementKey).ElementAt(0);
        }

        private float GetXElementFloat(XElement xElement, string elementKey)
        {
            var valueString = xElement.ToString().Replace("<" + elementKey + ">", "")
                                      .Replace("</"           + elementKey + ">", "");
            return float.Parse(valueString);
        }
    }
}