using UnityEngine;

public class SpeedParticleScript : MonoBehaviour
{

    private ParticleSystem speedParticle;

    private void Awake()
    {
        speedParticle = GetComponent<ParticleSystem>();
        speedParticle.Stop();
    }
    public void WalkSpeedParticles()
    {
        var emission = speedParticle.emission;
        emission.enabled = true;
        emission.rateOverTime = 25;

        var main = speedParticle.main;
        main.startSpeed = 10f;

        main.startColor = Color.lightCyan;
        if (!speedParticle.isPlaying)
            speedParticle.Play();
    }

    public void MidSpeedParticles()
    {
        var emission = speedParticle.emission;
        emission.enabled = true;
        emission.rateOverTime = 60;

        var main = speedParticle.main;
        main.startSpeed = 20f;
        main.startColor = Color.yellow;

        if (!speedParticle.isPlaying)
            speedParticle.Play();
    }

    public void FastSpeedParticles()
    {
        var emission = speedParticle.emission;
        emission.enabled = true;
        emission.rateOverTime = 80;
        var main = speedParticle.main;
        main.startSpeed = 30f;
        main.startColor = Color.red;
        if (!speedParticle.isPlaying)
            speedParticle.Play();
    }

    public void StopShowingParticles()
    {
        speedParticle.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
