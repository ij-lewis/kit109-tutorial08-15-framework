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
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Implement your Tutorial callbacks here.
/// </summary>
[CreateAssetMenu(fileName = DefaultFileName, menuName = "Tutorials/" + DefaultFileName + " Instance")]
public class Tutorial14Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial14Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial14Callbacks>(DefaultFileName);
    }

    //b
    public bool B1_PostProcessingLayerComponent()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<PostProcessLayer>("Main Camera");
    }
    public bool B2_CameraLayer()
    {
        var cam = GameObject.Find("Main Camera");
        if (cam == null) return false;

        return cam.layer == LayerMask.NameToLayer("PostProcessingEffects");
    }
    public bool B2_PostProcessingLayerLayer()
    {
        var pp = CommonTutorialCallbacks.GameObjectComponent<PostProcessLayer>("Main Camera");
        if (pp == null) return false;

        //mask == (mask | (1 << layer));
        return pp.volumeLayer == (pp.volumeLayer | (1 << LayerMask.NameToLayer("PostProcessingEffects")));
    }
    public bool B3_PostProcessingVolumeComponent()
    {
        var pv = CommonTutorialCallbacks.GameObjectComponent<PostProcessVolume>("Main Camera");
        if (pv == null) return false;

        return pv.isGlobal;
    }
    static PostProcessProfile GlobalProfile()
    {
        return AssetDatabase.LoadAssetAtPath<PostProcessProfile>("Assets/Profiles/Global.asset");
    }
    public bool B4_GlobalProfile()
    {
        return GlobalProfile() != null;
    }
    public bool B5_VignetteEffect()
    {
        var profile = GlobalProfile();
        if (profile == null) return false;

        return profile.HasSettings<Vignette>();
    }
    public bool B5_VignetteEffectIntensity()
    {
        var profile = GlobalProfile();
        if (profile == null) return false;

        if (profile.HasSettings<Vignette>() == false) return false;

        var settings = profile.GetSetting<Vignette>();
        return
            settings.intensity.overrideState.Equals(true) &&
            settings.intensity.value.Equals(0.5f);
    }

    //c
    public bool C1_ColourGrading()
    {
        var profile = GlobalProfile();
        if (profile == null) return false;

        return profile.HasSettings<ColorGrading>();
    }
    public bool C3_ColourGradingCurve()
    {
        var profile = GlobalProfile();
        if (profile == null) return false;

        if (profile.HasSettings<ColorGrading>() == false) return false;

        var settings = profile.GetSetting<ColorGrading>();
        //0,0|0.6350574,0|0.7011494,1|0.7729885,0
        //Debug.Log(string.Join("|", settings.hueVsSatCurve.value.curve.keys.ToList().Select(k => k.time + "," + k.value)));

        var curve = settings.hueVsSatCurve.value.curve;

        return
            settings.hueVsSatCurve.overrideState.Equals(true) &&
            //curve.keys.Length >= 4 &&
            curve.keys.Count(k => k.value == 0) >= 2 &&
            curve.keys.Count(k => k.value == 1) == 1 &&
            curve.keys.Count(k => k.time > 0.45f) >= 3;
            /*
            curve.keys.Any(k => k.time == 0 && k.value == 0) &&
            curve.keys.Any(k => k.time >= 0.5f && k.time < 0.65f && k.value == 0) &&
            curve.keys.Any(k => k.time >= 0.5f && k.time < 0.75f && k.value == 1) &&
            curve.keys.Any(k => k.time >= 0.65f && k.value < 0.85f && k.value == 0)*/;
    }
    public bool C4_Bloom()
    {
        var profile = GlobalProfile();
        if (profile == null) return false;

        return profile.HasSettings<Bloom>();
    }

    //d
    public bool D1_Volume()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<PostProcessVolume>("DarkArea");
    }
    public bool D1_VolumePosition()
    {
        var t = CommonTutorialCallbacks.GameObjectComponent<Transform>("DarkArea");
        if (t == null) return false;

        return t.position != Vector3.zero && t.position.z.Equals(0);
    }
    public bool D1_VolumeLayer()
    {
        var cam = GameObject.Find("DarkArea");
        if (cam == null) return false;

        return cam.layer == LayerMask.NameToLayer("PostProcessingEffects");
    }
    static PostProcessProfile DarkAreaProfile()
    {
        return AssetDatabase.LoadAssetAtPath<PostProcessProfile>("Assets/Profiles/DarkArea Profile.asset");
    }
    public bool D2_DarkAreaProfile()
    {
        return DarkAreaProfile() != null;
    }
    public bool D2_Global()
    {
        var pv = CommonTutorialCallbacks.GameObjectComponent<PostProcessVolume>("DarkArea");
        if (pv == null) return false;

        return pv.isGlobal;
    }
    public bool D3_DarkAreaColorGrading()
    {
        var profile = DarkAreaProfile();
        if (profile == null) return false;

        if (profile.HasSettings<ColorGrading>() == false) return false;

        var settings = profile.GetSetting<ColorGrading>();
        return
            settings.temperature.overrideState.Equals(true) &&
            settings.temperature.value.Equals(-40) &&
            settings.gamma.overrideState.Equals(true) &&
            settings.gamma.value.w < -0.4f;
    }
    public bool D4_NotGlobal()
    {
        var pv = CommonTutorialCallbacks.GameObjectComponent<PostProcessVolume>("DarkArea");
        if (pv == null) return false;

        return pv.isGlobal.Equals(false);
    }
    public bool D4_Trigger()
    {
        var pp = CommonTutorialCallbacks.GameObjectComponent<PostProcessLayer>("Main Camera");
        if (pp == null) return false;

        var player = GameObject.Find("Player");
        if (player == null) return false;

        return pp.volumeTrigger == player.transform;
    }
    public bool D5_BlendDistance()
    {
        var pv = CommonTutorialCallbacks.GameObjectComponent<PostProcessVolume>("DarkArea");
        if (pv == null) return false;

        return pv.blendDistance.Equals(0.5f);
    }
    
    //e
    public bool E1_ScareRadius()
    {
        var radius = GameObject.Find("ScareRadius");
        if (radius == null) return false;

        var ai = GameObject.Find("ComplexAlmostHumanLikeAI");
        if (ai == null) return false;

        return
            radius.transform.parent == ai.transform &&
            radius.transform.localPosition == Vector3.zero;
    }
    public bool E1_RadiusSphere()
    {
        var sphere = CommonTutorialCallbacks.GameObjectComponent<SphereCollider>("ScareRadius");
        return sphere != null && sphere.radius.Equals(2);
    }
    public bool E1_RadiusLayer()
    {
        var cam = GameObject.Find("ScareRadius");
        if (cam == null) return false;

        return cam.layer == LayerMask.NameToLayer("PostProcessingEffects");
    }
    public bool E1_RadiusVolume()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<PostProcessVolume>("ScareRadius");
    }
    static PostProcessProfile ScareProfile()
    {
        return AssetDatabase.LoadAssetAtPath<PostProcessProfile>("Assets/Profiles/ScareRadius Profile.asset");
    }
    public bool E2_ScareProfile()
    {
        return ScareProfile() != null;
    }
    public bool E2_ScareRadiusChromaticAberration()
    {
        var profile = ScareProfile();
        if (profile == null) return false;

        if (profile.HasSettings<ChromaticAberration>() == false) return false;

        var settings = profile.GetSetting<ChromaticAberration>();
        return
            settings.intensity.overrideState.Equals(true) &&
            settings.intensity.value.Equals(0.5f);
    }

    //f
    public bool F1_PulseScript()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<ChromaticAberrationPulse>("ScareRadius");
    }
}
#endif