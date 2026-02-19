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
        if (trail == null) { Criterion.globalLastKnownError = "'Trail' GameObject is missing 'TrailRenderer'."; return false; }

        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        if (trail.transform.parent != player.transform) { Criterion.globalLastKnownError = "'Trail' must be a child of 'Player'."; return false; }
        return true;
    }
    public bool B1_TrailPosition()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) return false;

        if (!B1_Trail()) return false;
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail.transform.localPosition.y >= 0) { Criterion.globalLastKnownError = "'Trail' local Y position should be less than 0 (e.g. -0.5)."; return false; }
        return true;
    }
    public bool B1_TrailRenderOrder()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) { Criterion.globalLastKnownError = "'Trail' GameObject is missing 'TrailRenderer'."; return false; }

        if (trail.sortingOrder != 1) { Criterion.globalLastKnownError = "'Trail' Sorting Order should be 1."; return false; }
        return true;
    }
    public bool B3_Color()//b4 but meh cbf changing now
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) { Criterion.globalLastKnownError = "'Trail' GameObject is missing 'TrailRenderer'."; return false; }

        if (trail.colorGradient.alphaKeys.Count() != 2 ||
            trail.colorGradient.alphaKeys[0].alpha != 1 ||
            trail.colorGradient.alphaKeys[1].alpha != 0)
        {
             Criterion.globalLastKnownError = "'Trail' Color Gradient should fade from Alpha 1 to 0.";
             return false;
        }
        return true;
    }
    public bool B5_Time()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) { Criterion.globalLastKnownError = "'Trail' GameObject is missing 'TrailRenderer'."; return false; }

        if (trail.time != 1) { Criterion.globalLastKnownError = "'Trail' Time should be 1."; return false; }
        return true;
    }
    public bool B5_Width()
    {
        var trail = CommonTutorialCallbacks.GameObjectComponent<TrailRenderer>("Trail");
        if (trail == null) { Criterion.globalLastKnownError = "'Trail' GameObject is missing 'TrailRenderer'."; return false; }

        //Debug.Log(trail.widthMultiplier + " " + string.Join("|",trail.widthCurve.keys.ToList().Select(k => k.time + "," + k.value)));

        if (!trail.widthMultiplier.Equals(0.2f)) { Criterion.globalLastKnownError = "'Trail' Width Multiplier should be 0.2."; return false; }
        if (trail.widthCurve.keys.Count() < 2 || trail.widthCurve.keys[0].value != 1 || trail.widthCurve.keys.Last().value != 0)
        {
             Criterion.globalLastKnownError = "'Trail' Width Curve should go from 1 to 0.";
             return false;
        }
        return true;
    }

    //c
    public bool C1_ParticleSystem()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "'ExplosionParticles' GameObject is missing 'ParticleSystem'."; return false; }
        return true;
    }
    public bool C1_ParticleSystem_OrderInLayer()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) { Criterion.globalLastKnownError = "'ExplosionParticles' GameObject is missing 'ParticleSystemRenderer'."; return false; }

        if (psr.sortingOrder != 1) { Criterion.globalLastKnownError = "'ExplosionParticles' Sorting Order should be 1."; return false; }
        return true;
    }
    public bool C1_ParticleSystem_Rotation()
    {
        var t = CommonTutorialCallbacks.GameObjectComponent<Transform>("ExplosionParticles");
        if (t == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (t.localEulerAngles.x != 0) { Criterion.globalLastKnownError = "'ExplosionParticles' X Rotation should be 0."; return false; }
        return true;
    }
    public bool C7_Stars()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) { Criterion.globalLastKnownError = "'ExplosionParticles' GameObject is missing 'ParticleSystemRenderer'."; return false; }

        if (psr.sharedMaterial == null || !psr.sharedMaterial.name.Contains("Star")) { Criterion.globalLastKnownError = "'ExplosionParticles' Material should be a Star material."; return false; }
        return true;
    }
    //d
    public bool D1_Reset()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles' (scene or prefab)."; return false; }

        bool c1 = C1_ParticleSystem();
        bool c2 = C1_ParticleSystem_OrderInLayer();
        bool c3 = C1_ParticleSystem_Rotation();
        
        if (!c1 || !c2 || !c3) { Criterion.globalLastKnownError = "Reset 'ExplosionParticles' transform/renderer settings."; return false; }
        return true;
    }
    public bool D2_Looping()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (ps.main.loop) { Criterion.globalLastKnownError = "'ExplosionParticles' Looping should be disabled."; return false; }
        return true;
    }
    public bool D2_StartLifetime()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (ps.main.startLifetime.mode != ParticleSystemCurveMode.TwoConstants ||
            !Mathf.Approximately(ps.main.startLifetime.constantMin, 0.5f) ||
            !Mathf.Approximately(ps.main.startLifetime.constantMax, 1f))
        {
             Criterion.globalLastKnownError = "'ExplosionParticles' Start Lifetime must be Random Between Two Constants (0.5 to 1).";
             return false;
        }
        return true;
    }
    public bool D2_StartSpeed()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (ps.main.startSpeed.mode != ParticleSystemCurveMode.Constant ||
            !Mathf.Approximately(ps.main.startSpeed.constant, 2))
        {
             Criterion.globalLastKnownError = "'ExplosionParticles' Start Speed must be Constant (2).";
             return false;
        }
        return true;
    }
    public bool D2_StartSize()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (ps.main.startSize.mode != ParticleSystemCurveMode.TwoConstants ||
            !Mathf.Approximately(ps.main.startSize.constantMin, 0.05f) ||
            !Mathf.Approximately(ps.main.startSize.constantMax, 0.15f))
        {
             Criterion.globalLastKnownError = "'ExplosionParticles' Start Size must be Random Between Two Constants (0.05 to 0.15).";
             return false;
        }
        return true;
    }
    public bool D3_TimeAndDistance()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (ps.emission.rateOverTime.constant > 0 || ps.emission.rateOverDistance.constant > 0) { Criterion.globalLastKnownError = "'ExplosionParticles' Emission Rate Time/Distance must be 0."; return false; }
        return true;
    }
    public bool D3_Burst()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (ps.emission.burstCount != 1) { Criterion.globalLastKnownError = "'ExplosionParticles' should have 1 Burst."; return false; }
        var burst = ps.emission.GetBurst(0);
        if (burst.time != 0 || burst.count.constant != 10) { Criterion.globalLastKnownError = "'ExplosionParticles' Burst should be at Time 0 with Count 10."; return false; }
        return true;
    }
    public bool D4_Shape()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (ps.shape.shapeType != ParticleSystemShapeType.Circle) { Criterion.globalLastKnownError = "'ExplosionParticles' Shape must be 'Circle'."; return false; }
        return true;
    }
    public bool D4_Radius()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (!Mathf.Approximately(ps.shape.radius, 0.5f)) { Criterion.globalLastKnownError = "'ExplosionParticles' Shape Radius must be 0.5."; return false; }
        return true;
    }
    public bool D4_RadiusThickness()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (!Mathf.Approximately(ps.shape.radiusThickness, 0.5f)) { Criterion.globalLastKnownError = "'ExplosionParticles' Shape Radius Thickness must be 0.5."; return false; }
        return true;
    }
    public bool D5_ForceOverLifetimeEnabled()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (!ps.forceOverLifetime.enabled) { Criterion.globalLastKnownError = "'ExplosionParticles' Force over Lifetime should be enabled."; return false; }
        return true;
    }
    public bool D5_ForceOverLifetimeAmount()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (!ps.forceOverLifetime.enabled) { Criterion.globalLastKnownError = "'ExplosionParticles' Force over Lifetime should be enabled."; return false; }
        if (ps.forceOverLifetime.y.constant != -9.8f) { Criterion.globalLastKnownError = "'ExplosionParticles' Force over Lifetime Y must be -9.8."; return false; }
        return true;
    }
    public bool D5_ForceOverLifetimeSpace()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (ps.forceOverLifetime.space != ParticleSystemSimulationSpace.World) { Criterion.globalLastKnownError = "'ExplosionParticles' Force over Lifetime Space should be 'World'."; return false; }
        return true;
    }
    public bool D6_SizeOverLifetimeEnabled()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (!ps.sizeOverLifetime.enabled) { Criterion.globalLastKnownError = "'ExplosionParticles' Size over Lifetime should be enabled."; return false; }
        return true;
    }
    public bool D6_SizeOverLifetimeCurve()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (!ps.sizeOverLifetime.enabled || ps.sizeOverLifetime.size.curve.keys.Last().value != 0) { Criterion.globalLastKnownError = "'ExplosionParticles' Size over Lifetime Curve must decrease to 0."; return false; }
        return true;
    }
    public bool D7_RenderMode()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (psr.renderMode != ParticleSystemRenderMode.Stretch || psr.lengthScale != 2) { Criterion.globalLastKnownError = "'ExplosionParticles' Render Mode should be Stretch (Scale 2)."; return false; }
        return true;
    }
    public bool D7_Material()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (psr.sharedMaterial == null || !psr.sharedMaterial.name.Contains("Sprites")) { Criterion.globalLastKnownError = "'ExplosionParticles' Material should be 'Sprites (Default-Particle)'."; return false; }
        return true;
    }
    public bool D7_OrderInLayer()
    {
        var psr = CommonTutorialCallbacks.GameObjectComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) CommonTutorialCallbacks.PrefabComponent<ParticleSystemRenderer>("ExplosionParticles");
        if (psr == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles'."; return false; }

        if (psr.sortingOrder != 1) { Criterion.globalLastKnownError = "'ExplosionParticles' Sorting Order must be 1."; return false; }
        return true;
    }
    public bool D9_ExplosionOnDestroy()
    {
        var script = CommonTutorialCallbacks.PrefabComponent<ExplosionOnDestroy>("Bullet");
        if (script == null) { Criterion.globalLastKnownError = "'Bullet' prefab is missing 'ExplosionOnDestroy' script."; return false; }

        if (script.explosionPrefab == null || !script.explosionPrefab.name.Contains("ExplosionParticles")) { Criterion.globalLastKnownError = "'ExplosionOnDestroy' Explosion Prefab should be 'ExplosionParticles'."; return false; }
        return true;
    }
    public bool D10_StopAction()
    {
        var ps = CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles' prefab."; return false; }

        if (ps.main.stopAction != ParticleSystemStopAction.Destroy) { Criterion.globalLastKnownError = "'ExplosionParticles' Stop Action must be 'Destroy'."; return false; }
        return true;
    }
    public bool D10_Duration()
    {
        var ps = CommonTutorialCallbacks.PrefabComponent<ParticleSystem>("ExplosionParticles");
        if (ps == null) { Criterion.globalLastKnownError = "Could not find 'ExplosionParticles' prefab."; return false; }

        if (ps.main.duration != 1) { Criterion.globalLastKnownError = "'ExplosionParticles' Duration must be 1."; return false; }
        return true;
    }

    //e
    public bool E1_ParticleSystem()
    {
        var fireworks = GameObject.Find("Firework");
        if (fireworks == null) { Criterion.globalLastKnownError = "Could not find 'Firework' GameObject."; return false; }

        if (!CommonTutorialCallbacks.GameObjectContainsScript<ParticleSystem>("FireworkExplosion")) { Criterion.globalLastKnownError = "Could not find 'FireworkExplosion' with ParticleSystem."; return false; }
        
        var explosion = GameObject.Find("FireworkExplosion");
        if (explosion.transform.parent != fireworks.transform) { Criterion.globalLastKnownError = "'FireworkExplosion' must be a child of 'Firework'."; return false; }
        return true;
    }
    public bool E1_SubEmitter()
    {
        var fireworks = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("Firework");
        if (fireworks == null) { Criterion.globalLastKnownError = "'Firework' is missing ParticleSystem."; return false; }

        var explosion = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("FireworkExplosion");
        if (explosion == null) { Criterion.globalLastKnownError = "'FireworkExplosion' is missing ParticleSystem."; return false; }

        if (fireworks.subEmitters.subEmittersCount != 1 || fireworks.subEmitters.GetSubEmitterSystem(0) != explosion) { Criterion.globalLastKnownError = "'Firework' must have 1 Sub Emitter set to 'FireworkExplosion'."; return false; }
        return true;
    }
    public bool E2_SubEmitterDeath()
    {
        if (!E1_SubEmitter()) return false;
        var fireworks = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("Firework");
        if (fireworks.subEmitters.GetSubEmitterType(0) != ParticleSystemSubEmitterType.Death) { Criterion.globalLastKnownError = "Sub Emitter Type must be 'Death'."; return false; }
        return true;
    }
    public bool E2_Burst()
    {
        var ps = CommonTutorialCallbacks.GameObjectComponent<ParticleSystem>("FireworkExplosion");
        if (ps == null) { Criterion.globalLastKnownError = "'FireworkExplosion' is missing ParticleSystem."; return false; }

        if (ps.emission.burstCount != 1 || ps.emission.GetBurst(0).count.constant != 30) { Criterion.globalLastKnownError = "'FireworkExplosion' needs 1 Burst of 30 particles."; return false; }
        return true;
    }
}
#endif