﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;


/*
- Creator:    Two TV Games (@gallighanmaker)
- Script:     Day And Night 2D System
- Unity:      2019 LTS Version
- Email:      leandrovieira92@gmail.com
- Github:     https://github.com/leandrovieiraa
*/

public enum DayCycles // Enum with day and night cycles, you can change or modify with whatever you want
{
    Sunrise = 0,
    Day = 1,
    Sunset = 2,
    Night = 3,
    Midnight = 4
}

public class DayNightSystem2D : MonoBehaviour
{
    [Header("Controllers")]
    
    [Tooltip("Global light 2D component, we need to use this object to place light in all map objects")]
    public UnityEngine.Rendering.Universal.Light2D globalLight; // global light
    
    [Tooltip("This is a current cycle time, you can change for private float but we keep public only for debug")]
    public float cycleCurrentTime = 0; // current cycle time
    
    [Tooltip("This is a cycle max time in seconds, if current time reach this value we change the state of the day and night cyles")]
    public float cycleMaxTime = 30; // duration of cycle

    [Tooltip("Enum with multiple day cycles to change over time, you can add more types and modify whatever you want to fits on your project")]
    public DayCycles dayCycle = DayCycles.Sunset; // default cycle

    [Header("Cycle Colors")]
    
    [Tooltip("Sunrise color, you can adjust based on best color for this cycle")]
    public Color sunrise; // Eg: 6:00 at 10:00
    
    [Tooltip("(Mid) Day color, you can adjust based on best color for this cycle")]
    public Color day; // Eg: 10:00 at 16:00
    
    [Tooltip("Sunset color, you can adjust based on best color for this cycle")]
    public Color sunset; // Eg: 16:00 20:00
    
    [Tooltip("Night color, you can adjust based on best color for this cycle")]
    public Color night; // Eg: 20:00 at 00:00
    
    [Tooltip("Midnight color, you can adjust based on best color for this cycle")]
    public Color midnight; // Eg: 00:00 at 06:00

    [Header("Objects")]
    [Tooltip("Objects to turn on and off based on day night cycles, you can use this example for create some custom stuffs")]
    public List<UnityEngine.Rendering.Universal.Light2D> mapLights = new List<UnityEngine.Rendering.Universal.Light2D>(); 

    bool light2dActive = false;

    void Start() 
    {
        dayCycle = DayCycles.Sunset; // start with sunrise state
        globalLight.color = sunset; // start global color at sunrise

    }

     void Update()
     {
        // Update cycle time
        cycleCurrentTime += Time.deltaTime;
        float _cycleMaxTime = cycleMaxTime * (dayCycle == DayCycles.Night || dayCycle == DayCycles.Midnight ? 1.5f : 1.0f);

        // Check if cycle time reach cycle duration time
        if (cycleCurrentTime >= _cycleMaxTime)
        {
            cycleCurrentTime = 0; // back to 0 (restarting cycle time)
            dayCycle++; // change cycle state
        }

        // If reach final state we back to sunrise (Enum id 0)
        if(dayCycle > DayCycles.Midnight)
        {
            dayCycle = 0;
        }

        // percent it's an value between current and max time to make a color lerp smooth
        float percent = cycleCurrentTime / _cycleMaxTime;

        // Sunrise state (you can do a lot of stuff based on every cycle state, like enable animals only in sunrise )
        if(dayCycle == DayCycles.Sunrise)
        {
            ControlLightMaps(false); // disable map light (keep enable only at night)
            globalLight.color = Color.Lerp(sunrise, day, percent);
        }

        // Mid Day state
        if(dayCycle == DayCycles.Day)
        {
            globalLight.color = Color.Lerp(day, sunset, percent);
        }

        // Sunset state
        if(dayCycle == DayCycles.Sunset)
        {
            ControlLightMaps(true); // enable map lights (disable only in day states)
            globalLight.color = Color.Lerp(sunset, night, percent);
        }

        // Night state
        if(dayCycle == DayCycles.Night)
        {
            globalLight.color = Color.Lerp(night, midnight, percent);        
        }

        // Midnight state
        if(dayCycle == DayCycles.Midnight)
        {
            globalLight.color = Color.Lerp(midnight, day, percent);     
        }
     }

     void ControlLightMaps(bool status)
     {
        // loop in light array of objects to enable/disable
        light2dActive = status;
        if(mapLights.Count > 0)
            foreach(UnityEngine.Rendering.Universal.Light2D _light in mapLights) {
                if (_light != null && !_light.IsDestroyed()) {
                    _light.gameObject.SetActive(status);
                }
            }
     }

     public void addMapLight(Light2D light2d)
     {
        light2d.gameObject.SetActive(light2dActive);
        mapLights.Add(light2d);
     }

     public void removeMapLight(Light2D light2d)
     {
        light2d.gameObject.SetActive(false);
        mapLights.Remove(light2d);
     }
}
