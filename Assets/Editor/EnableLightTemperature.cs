#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class EnableLightTemperature : MonoBehaviour
{
    [MenuItem("Tools/Enable Light Temperature")]
    static void Enable()
    {
        GraphicsSettings.lightsUseLinearIntensity = true;
        GraphicsSettings.lightsUseColorTemperature = true;
        Debug.Log("Light Temperature включена. Проверьте Inspector источника света.");
    }
}
#endif