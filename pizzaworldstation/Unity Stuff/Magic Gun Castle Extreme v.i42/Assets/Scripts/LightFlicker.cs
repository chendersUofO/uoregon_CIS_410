using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

		public float minIntensity = 0.25f;
		public float maxIntensity = 0.5f;
		public float offset = 1;
		Light light;
		float random;

		void Start()
		{
			light = GetComponent<Light> ();
			random = Random.Range(0.0f, 65535.0f);
		}

		void Update()
		{
			float noise = Mathf.PerlinNoise(random, Time.time * offset);
			light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
		}
	}//using UnityEngine;
