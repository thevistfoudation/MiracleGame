using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Import TextMeshPro namespace for better text rendering
using Newtonsoft.Json.Linq;
using E2C;


public class BitcoinPriceChart : MonoBehaviour
{
    public E2ChartData e2ChartData;
    public List<float> prices = new List<float>();
    public List<string> times = new List<string>();

    void Start()
    {
        StartCoroutine(UpdateChartData());
        InvokeRepeating("UpdateChartData", 0f, 60f); // Update every 60 seconds
    }

    IEnumerator UpdateChartData()
    {
        string url = GetCoinGeckoApiUrl();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching data: " + webRequest.error);
            }
            else
            {
                ProcessData(webRequest.downloadHandler.text);
            }
        }
    }

    string GetCoinGeckoApiUrl()
    {
        // Get timestamp for midnight today in UTC
        System.DateTime midnight = System.DateTime.UtcNow.Date;
        long midnightTimestamp = ((System.DateTimeOffset)midnight).ToUnixTimeSeconds();
        long currentTimestamp = ((System.DateTimeOffset)System.DateTime.UtcNow).ToUnixTimeSeconds();

        return $"https://api.coingecko.com/api/v3/coins/bitcoin/market_chart/range?vs_currency=usd&from={midnightTimestamp}&to={currentTimestamp}";
    }

    void ProcessData(string json)
    {
        var parsedData = JObject.Parse(json);
        var pricesArray = parsedData["prices"];
        prices.Clear();
        e2ChartData.series[0].dataY.Clear();
        times.Clear();
        e2ChartData.categoriesX.Clear();

        foreach (var pricePoint in pricesArray)
        {
            float price = pricePoint[1].Value<float>();
           
            
            prices.Add(price);
            e2ChartData.series[0].dataY.Add(price);
            System.DateTime date = System.DateTimeOffset.FromUnixTimeMilliseconds(pricePoint[0].Value<long>()).DateTime;
            times.Add(date.ToString("HH:mm"));
            e2ChartData.categoriesX.Add(date.ToString("HH:mm"));
        }
    }
}
