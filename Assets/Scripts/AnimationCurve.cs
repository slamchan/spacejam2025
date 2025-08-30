using System.Collections;
using UnityEngine;

/// <summary>
/// Increases or decreases light intensity based on an ease-in-ease-out curve.
/// </summary>
[RequireComponent(typeof(Light))]
public class SmoothLight : MonoBehaviour
{
    float m_MaxIntensity;
    AnimationCurve m_LightCurve;
    float m_CurrentTime;
    public float m_Direction;
    Light m_Light;

    void Start()
    {
        m_Light = GetComponent<Light>();
        m_MaxIntensity = m_Light.intensity;
        m_LightCurve = AnimationCurve.EaseInOut(0, 0, 1, m_MaxIntensity);

        //Initialize the current time to represent the ratio between 0 and max intensity
        var currentIntensity = Mathf.Clamp(m_Light.intensity, 0.0f, m_MaxIntensity);
        m_CurrentTime = currentIntensity / m_MaxIntensity;
    }

    //Use this method to bring the light back to the maximum intensity over one second.
    public IEnumerator TurnUp()
    {
        //Increase the intensity until we reach MaxIntensity or a TurnDown call is made.
        m_Direction = 1.0f;
        while (m_Direction > 0.0f && m_CurrentTime < m_MaxIntensity)
        {
            m_CurrentTime += Time.deltaTime;
            m_Light.intensity = m_LightCurve.Evaluate(m_CurrentTime);
            yield return null;
        }
    }

    //Use this method to bring the light back to zero intensity over one second.
    public IEnumerator TurnDown()
    {
        //Decrease the intensity until we reach MaxIntensity or a TurnDown call is made.
        m_Direction = -1.0f;
        while (m_Direction < 0.0f && m_CurrentTime > 0)
        {
            m_CurrentTime -= Time.deltaTime;
            m_Light.intensity = m_LightCurve.Evaluate(m_CurrentTime);
            yield return null;
        }
    }
}