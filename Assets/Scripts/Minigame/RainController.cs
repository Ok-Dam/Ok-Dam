using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeatherController : MonoBehaviour
{
    public ParticleSystem rainEffect;
    public ParticleSystem snowEffect;
    public GameObject heatWaveEffect;
    public GameObject coldEffect;
    public float weatherChangeInterval = 20f; // 날씨 변화 주기 (초)

    private void Start()
    {
        StartCoroutine(ChangeWeatherCycle());
    }

    private IEnumerator ChangeWeatherCycle()
    {
        while (true)
        {
            // 날씨 변경
            int randomWeather = Random.Range(0, 4); // 0: 비, 1: 눈, 2: 더위, 3: 추위
            switch (randomWeather)
            {
                case 0: // 비
                    rainEffect.Play();
                    snowEffect.Stop();
                    heatWaveEffect.SetActive(false);
                    coldEffect.SetActive(false);
                    break;
                case 1: // 눈
                    snowEffect.Play();
                    rainEffect.Stop();
                    heatWaveEffect.SetActive(false);
                    coldEffect.SetActive(false);
                    break;
                case 2: // 더위
                    heatWaveEffect.SetActive(true);
                    rainEffect.Stop();
                    snowEffect.Stop();
                    coldEffect.SetActive(false);
                    break;
                case 3: // 추위
                    coldEffect.SetActive(true);
                    rainEffect.Stop();
                    snowEffect.Stop();
                    heatWaveEffect.SetActive(false);
                    break;
            }
            yield return new WaitForSeconds(weatherChangeInterval);
        }
    }
}
