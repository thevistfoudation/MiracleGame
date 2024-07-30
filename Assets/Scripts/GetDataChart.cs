using E2C;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetDataChart : MonoBehaviour
{
    private E2ChartData E2ChartData;
    private string url = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=1&interval=minute";

    public List<DateTime> timestamps = new List<DateTime>();
    public List<float> prices = new List<float>();


    void Start()
    {
        StartCoroutine(GetBitcoinPrices());
    }

    // Coroutine để lấy dữ liệu từ API
    IEnumerator GetBitcoinPrices()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Thêm User-Agent header
            request.SetRequestHeader("User-Agent", "UnityApp");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
            else
            {
                // Xử lý dữ liệu JSON
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);
                BitcoinData bitcoinData = JsonUtility.FromJson<BitcoinData>(jsonResponse);

                foreach (var price in bitcoinData.prices)
                {
                    DateTime timestamp = DateTimeOffset.FromUnixTimeMilliseconds((long)price[0]).DateTime;
                    float priceValue = price[1];

                    timestamps.Add(timestamp);
                    prices.Add(priceValue);
                }

                foreach (var timestamp in timestamps)
                {
                    Debug.Log("Timestamp: " + timestamp);
                }

                foreach (var price in prices)
                {
                    Debug.Log("Price: " + price);
                }
            }
        }
    }
    [Serializable]
    private class BitcoinData
    {
        public float[][] prices;
    }

}