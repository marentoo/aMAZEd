//using UnityEngine;
//using System.Collections;

//public class DirectionalLightSettings : MonoBehaviour
//{
//    void Start()
//    {
//        StartCoroutine(FindAndSetDirectionalLightColor());
//    }

//    private IEnumerator FindAndSetDirectionalLightColor()
//    {
//        Light directionalLight = null;

//        // Wait until the Directional Light is found (up to a maximum wait time)
//        float maxWaitTime = 10f;
//        float waitTime = 0f;
//        while (directionalLight == null && waitTime < maxWaitTime)
//        {
//            // Attempt to find the Directional Light in the scene
//            foreach (Light light in FindObjectsOfType<Light>())
//            {
//                if (light.type == LightType.Directional)
//                {
//                    directionalLight = light;
//                    break;
//                }
//            }

//            if (directionalLight == null)
//            {
//                // Wait for a bit before trying again
//                yield return new WaitForSeconds(0.5f);
//                waitTime += 0.5f;
//            }
//        }

//        // If the Directional Light is found, set its color to black
//        if (directionalLight != null)
//        {
//            directionalLight.color = Color.black;
//        }
//        else
//        {
//            Debug.LogError("No Directional Light found in the scene.");
//        }
//    }
//}
