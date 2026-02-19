#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.Tutorials.Core.Editor;
using System.Linq;
using UnityEditor.Animations;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.Reflection;

/// <summary>
/// Implement your Tutorial callbacks here.
/// </summary>
[CreateAssetMenu(fileName = DefaultFileName, menuName = "Tutorials/" + DefaultFileName + " Instance")]
public class Tutorial13Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial13Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial13Callbacks>(DefaultFileName);
    }

    //b
    public bool B1_Trail()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) return false;

        var player = GameObject.Find("Player");
        if (player == null) return false;

        return trail.transform.parent == player.transform;
    }
    public bool B1_TrailPosition()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) return false;

        return B1_Trail() && trail.transform.localPosition.y < 0;
    }
    public bool B1_TrailRenderOrder()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) return false;

        return trail.sortingOrder.Equals(1);
    }
    public bool B3_Color()//b4 but meh cbf changing now
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) return false;

        return
            trail.colorGradient.alphaKeys.Count() == 2 &&
            trail.colorGradient.alphaKeys[0].alpha == 1 &&
            trail.colorGradient.alphaKeys[1].alpha == 0 &&
            trail.colorGradient.alphaKeys[1].time < 1;
    }
    public bool B5_Time()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) return false;

        return trail.time.Equals(1);
    }
    public bool B5_Width()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) return false;

        //Debug.Log(trail.widthMultiplier + " " + string.Join("|",trail.widthCurve.keys.ToList().Select(k => k.time + "," + k.value)));

        return
            trail.widthMultiplier.Equals(0.2f) &&
            trail.widthCurve.keys.Count() == 2 &&
            trail.widthCurve.keys[0].value.Equals(1) &&
            trail.widthCurve.keys[0].time.Equals(0) &&
            trail.widthCurve.keys[1].value.Equals(0) &&
            trail.widthCurve.keys[1].time.Equals(1);
    }

    //c
    public bool C1_ParticleSystem()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return true;
    }
    public bool C1_ParticleSystem_OrderInLayer()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) return false;

        return psr.sortingOrder.Equals(1);
    }
    public bool C1_ParticleSystem_Rotation()
    {
        var t = CommonTutorialCallbacks.GameObjectComponent<Transform>("ExplosionParticles");
        if (t == null) return false;

        return t.localEulerAngles.x.Equals(0);
    }
    public bool C7_Stars()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) return false;

        return psr.sharedMaterial != null && psr.sharedMaterial.name.Contains("Star");
    }
    //d
    public bool D1_Reset()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) return false;

        return
            C1_ParticleSystem() &&
            C1_ParticleSystem_OrderInLayer() &&
            C1_ParticleSystem_Rotation();/* &&
            psr.sharedMaterial != null && psr.sharedMaterial.name.Contains("Star") == false;*/ //reset doesnt actually reset the material anyway...
    }
    public bool D2_Looping()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return ps.main.loop.Equals(false);
    }
    public bool D2_StartLifetime()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.main.startLifetime.mode.Equals(ParticleSystemCurveMode.TwoConstants) &&
            ps.main.startLifetime.constantMin.Equals(0.5f) &&
            ps.main.startLifetime.constantMax.Equals(1f);
    }
    public bool D2_StartSpeed()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.main.startSpeed.mode.Equals(ParticleSystemCurveMode.Constant) &&
            ps.main.startSpeed.constant.Equals(2);
    }
    public bool D2_StartSize()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.main.startSize.mode.Equals(ParticleSystemCurveMode.TwoConstants) &&
            ps.main.startSize.constantMin.Equals(0.05f) &&
            ps.main.startSize.constantMax.Equals(0.15f);
    }
    public bool D3_TimeAndDistance()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.emission.rateOverDistance.mode.Equals(ParticleSystemCurveMode.Constant) &&
            ps.emission.rateOverDistance.constant.Equals(0) &&
            ps.emission.rateOverTime.mode.Equals(ParticleSystemCurveMode.Constant) &&
            ps.emission.rateOverTime.constant.Equals(0);
    }
    public bool D3_Burst()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.emission.burstCount.Equals(1) &&
            ps.emission.GetBurst(0).time.Equals(0) &&
            ps.emission.GetBurst(0).count.mode.Equals(ParticleSystemCurveMode.Constant) &&
            ps.emission.GetBurst(0).count.constant.Equals(10);
    }
    public bool D4_Shape()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.shape.shapeType.Equals(ParticleSystemShapeType.Circle);
    }
    public bool D4_Radius()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.shape.radius.Equals(0.5f);
    }
    public bool D4_RadiusThickness()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.shape.radiusThickness.Equals(0.5f);
    }
    public bool D5_ForceOverLifetimeEnabled()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.forceOverLifetime.enabled;
    }
    public bool D5_ForceOverLifetimeAmount()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            D5_ForceOverLifetimeEnabled() &&
            ps.forceOverLifetime.x.mode.Equals(ParticleSystemCurveMode.Constant) &&
            ps.forceOverLifetime.x.constant.Equals(0) &&
            ps.forceOverLifetime.y.mode.Equals(ParticleSystemCurveMode.Constant) &&
            ps.forceOverLifetime.y.constant.Equals(-9.8f);
    }
    public bool D5_ForceOverLifetimeSpace()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            D5_ForceOverLifetimeEnabled() &&
            ps.forceOverLifetime.space.Equals(ParticleSystemSimulationSpace.World);
    }
    public bool D6_SizeOverLifetimeEnabled()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.sizeOverLifetime.enabled;
    }
    public bool D6_SizeOverLifetimeCurve()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            D6_SizeOverLifetimeEnabled() &&
            ps.sizeOverLifetime.size.mode.Equals(ParticleSystemCurveMode.Curve) &&
            ps.sizeOverLifetime.size.curve != null &&
            ps.sizeOverLifetime.size.curve.keys.Length >= 2 &&
            ps.sizeOverLifetime.size.curve.keys.Last().value.Equals(0);
    }
    public bool D7_RenderMode()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) return false;

        return
            psr.renderMode.Equals(ParticleSystemRenderMode.Stretch) &&
            psr.lengthScale.Equals(2);
    }
    public bool D7_Material()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) return false;

        return
            psr.sharedMaterial != null && psr.sharedMaterial.name.Contains("Sprites");
    }
    public bool D7_OrderInLayer()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) return false;

        return
            psr.sortingOrder.Equals(1);
    }
    public bool D9_ExplosionOnDestroy()
    {
        var script = CommonTutorialCallbacks.PrefabComponent<ExplosionOnDestroy>("Bullet");
        if (script == null) return false;

        return script.explosionPrefab != null && script.explosionPrefab.name.Contains("ExplosionParticles");
    }
    public bool D10_StopAction()
    {
        var ps = CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.main.stopAction.Equals(ParticleSystemStopAction.Destroy);
    }
    public bool D10_Duration()
    {
        var ps = CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) return false;

        return
            ps.main.duration.Equals(1);
    }

    //e
    public bool E1_ParticleSystem()
    {
        var fireworks = GameObject.Find("Firework");
        if (fireworks == null) return false;

        return
            CommonTutorialCallbacks.GameObjectContainsScript<ParticleSystem>("FireworkExplosion") &&
            GameObject.Find("FireworkExplosion").transform.parent == fireworks.transform;
    }
    public bool E1_SubEmitter()
    {
        var fireworks = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("Firework");
        if (fireworks == null) return false;

        var explosion = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("FireworkExplosion");
        if (explosion == null) return false;

        return
            fireworks.subEmitters.subEmittersCount.Equals(1) &&
            fireworks.subEmitters.GetSubEmitterSystem(0) == explosion;
    }
    public bool E2_SubEmitterDeath()
    {
        var fireworks = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("Firework");
        if (fireworks == null) return false;

        return E1_SubEmitter() &&
            fireworks.subEmitters.GetSubEmitterType(0).Equals(ParticleSystemSubEmitterType.Death);
    }
    public bool E2_Burst()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("FireworkExplosion");
        if (ps == null) return false;

        return
            ps.emission.burstCount.Equals(1) &&
            ps.emission.GetBurst(0).time.Equals(0) &&
            ps.emission.GetBurst(0).count.mode.Equals(ParticleSystemCurveMode.Constant) &&
            ps.emission.GetBurst(0).count.constant.Equals(30);
    }
}
#endif