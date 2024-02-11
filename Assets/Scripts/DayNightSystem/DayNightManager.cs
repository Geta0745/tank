using System.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightManager : DayNightSystem
{
    [Header("Day Night Manager"),SerializeField] Volume volume;
    [SerializeField,Range(0.25f,0.5f)] float fullDayPercent = 0.4f;
    [ReadOnly] public string fullDayTime;
    [SerializeField,Range(0.51f,0.95f)] float startNightPercent = 0.6f;
    [ReadOnly] public string fullNightTime;
    [ReadOnly,Header("Timer For Control Display Night")] public string percentBeforeNextDay;
    [ReadOnly] public float darkIntensity;
    [Header("Directional Light"),SerializeField] Light sun;
    [Header("Post Processing Stuff"),SerializeField] FilmGrain filmGrain;
    [SerializeField] Vignette vignette;
    [SerializeField] ColorAdjustments colorAdjustments;
    [SerializeField,Range(0.4f,0.99f)] float minDark = 0.4f;
    // Rule : 
    // 0% = 12 AM (เที่ยงคืน)
    // 50% = 12 PM (เที่ยงวัน)
    // 25% = 6 AM (6 โมงเช้า)

    // 75% = 6 PM (6โมงเย็น)
    protected override void Update()
    {
        base.Update();
        fullDayTime = calTime(maxFullDay*fullDayPercent);
        fullNightTime = calTime(maxFullDay*startNightPercent);
        // Calculate the percentage of the day (normalized between 0 and 1)
        float percentageOfDay = currentTime / maxFullDay;
        //display only
        percentBeforeNextDay = (percentageOfDay * 100).ToString("F2") + " %";
        // Calculate the intensity based on the percentage of the day
        darkIntensity = 0f;

        if (percentageOfDay <= fullDayPercent)
        {
            darkIntensity = Mathf.Lerp(1, 0, percentageOfDay / fullDayPercent);
        }
        else if(percentageOfDay >= startNightPercent)
        {
            darkIntensity = Mathf.Lerp(0, 1, (percentageOfDay - startNightPercent) / fullDayPercent);
        }
        // Set the intensity of Post Processing
        if (volume.profile.TryGet(out filmGrain))
        {
            filmGrain.intensity.value = darkIntensity;
        }
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = darkIntensity;
        }
        if (volume.profile.TryGet(out colorAdjustments))
        {
            float finalColorNumber = Mathf.Lerp(1f,minDark,darkIntensity);
            colorAdjustments.colorFilter.value = Color.HSVToRGB(0f,0f,finalColorNumber);
        }
        if(sun != null){
            sun.intensity = Mathf.Lerp(1f,0f,darkIntensity);
        }
    }
}
