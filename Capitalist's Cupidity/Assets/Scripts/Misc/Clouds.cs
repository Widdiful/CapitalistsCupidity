using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

    public float speed;
    public ParticleSystem rain;
    public Light sun;

    public AudioSource normalMusic;
    public AudioSource sadMusic;

    private Renderer renderer;
    [Range(0, 1)]
    public float happinessPercentage;

    void Start() {
        renderer = GetComponent<Renderer>();
        transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
    }

	void Update () {
        //happinessPercentage = Director.Instance.getGlobalHappiness() / 100f;
        if (float.IsNaN(happinessPercentage)) happinessPercentage = 1;
        transform.Rotate(new Vector3(0, speed * Time.timeScale, 0));

        ParticleSystem.EmissionModule emission = rain.emission;
        emission.rateOverTime = 500.0f * (1 - happinessPercentage);
        if (happinessPercentage >= 0.5f) emission.rateOverTime = 0;

        sun.color = Color.HSVToRGB(0, 0, (happinessPercentage * 0.75f) + 0.25f);
        Color color = renderer.materials[0].color;
        color.a = ((1 - happinessPercentage) * 0.75f) + 0.25f;
        renderer.materials[0].color = color;

        normalMusic.volume = happinessPercentage;
        sadMusic.volume = 1 - happinessPercentage;
	}
}
