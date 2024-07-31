using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System;
using E2C;
using System.Diagnostics;
using TMPro;
using Random = UnityEngine.Random;
public class DogeCoinGetData : MonoBehaviour
{
    public E2ChartData e2ChartData;
    public string apiUrl = "https://api.coingecko.com/api/v3/coins/dogecoin/market_chart?vs_currency=usd&days=30";

    public List<DateTime> dates = new List<DateTime>();
    public List<float> prices = new List<float>();

    public GameObject panelLoading;

    [SerializeField]
    private InformationValue informationValueArea1;
    [SerializeField]
    private InformationValue informationValueArea2;
    [SerializeField]
    private InformationValue informationValueArea3;
    [SerializeField]
    private InformationValue informationValueArea4;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUIPriceNowTxt;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUIPriceNowMarket;

    void Start()
    {
        StartCoroutine(FetchDogecoinData());
    }

    IEnumerator FetchDogecoinData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
            }
            else
            {
                ProcessData(webRequest.downloadHandler.text);
            }
        }
    }

    void ProcessData(string jsonData)
    {
        JObject parsedData = JObject.Parse(jsonData);
        JArray pricesArray = (JArray)parsedData["prices"];

        dates.Clear();
        e2ChartData.series[0].dataY.Clear();
        prices.Clear();
        e2ChartData.categoriesX.Clear();

        for (int i = 0; i < pricesArray.Count; i++)
        {
            long unixTime = (long)pricesArray[i][0];
            DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
            float price = (float)pricesArray[i][1];

            dates.Add(date);
            e2ChartData.categoriesX.Add(date.ToString("HH:mm"));
            prices.Add(price);
            e2ChartData.series[0].dataY.Add(price);
           
        }
      
        informationValueArea4.InittilizerData("Prev Close", prices[prices.Count - 1].ToString());
        informationValueArea3.InittilizerData("Open ", prices[0].ToString());
        prices.Sort();
        informationValueArea1.InittilizerData("Low ", prices[0].ToString());
        informationValueArea2.InittilizerData("High ", prices[prices.Count - 1].ToString());
        textMeshProUGUIPriceNowTxt.text = "$" + prices[Random.Range(0, prices.Count - 1)].ToString();
        textMeshProUGUIPriceNowMarket.text = "$" + prices[prices.Count - 1].ToString();
        e2ChartData.gameObject.SetActive(true);
        LeanTween.delayedCall(1f, () =>
        {
            panelLoading.SetActive(false);
        });
       
    }

  
}
