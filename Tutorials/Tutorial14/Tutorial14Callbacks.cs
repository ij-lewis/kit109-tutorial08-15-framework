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
        if (!CommonTutorialCallbacks.GameObjectContainsScript<PostProcessLayer>("Main Camera")) { Criterion.globalLastKnownError = "'Main Camera' is missing 'PostProcessLayer' component."; return false; }
        return true;
    }
    public bool B2_CameraLayer()
    {
        var cam = GameObject.Find("Main Camera");
        if (cam == null) { Criterion.globalLastKnownError = "Could not find 'Main Camera'."; return false; }

        if (cam.layer != LayerMask.NameToLayer("PostProcessingEffects")) { Criterion.globalLastKnownError = "'Main Camera' Layer must be 'PostProcessingEffects'."; return false; }
        return true;
    }
    public bool B2_PostProcessingLayerLayer()
    {
        var pp = CommonTutorialCallbacks.GameObjectComponent<PostProcessLayer>("Main Camera");
        if (pp == null) { Criterion.globalLastKnownError = "'Main Camera' is missing 'PostProcessLayer' component."; return false; }

        //mask == (mask | (1 << layer));
        if (pp.volumeLayer != (pp.volumeLayer | (1 << LayerMask.NameToLayer("PostProcessingEffects")))) { Criterion.globalLastKnownError = "'PostProcessLayer' Volume Layer must include 'PostProcessingEffects'."; return false; }
        return true;
    }
    public bool B3_PostProcessingVolumeComponent()
    {
        var pv = CommonTutorialCallbacks.GameObjectComponent<PostProcessVolume>("Main Camera");
        if (pv == null) { Criterion.globalLastKnownError = "'Main Camera' is missing 'PostProcessVolume' component."; return false; }

        if (!pv.isGlobal) { Criterion.globalLastKnownError = "'PostProcessVolume' Is Global must be checked."; return false; }
        return true;
    }
    static PostProcessProfile GlobalProfile()
    {
        return AssetDatabase.LoadAssetAtPath<PostProcessProfile>("Assets/Profiles/Global.asset");
    }
    public bool B4_GlobalProfile()
    {
        if (GlobalProfile() == null) { Criterion.globalLastKnownError = "Could not find 'Global.asset' in 'Assets/Profiles'."; return false; }
        return true;
    }
    public bool B5_VignetteEffect()
    {
        var profile = GlobalProfile();
        if (profile == null) { Criterion.globalLastKnownError = "Could not find 'Global' profile."; return false; }

        if (!profile.HasSettings<Vignette>()) { Criterion.globalLastKnownError = "'Global' profile must handle 'Vignette' effect."; return false; }
        return true;
    }
    public bool B5_VignetteEffectIntensity()
    {
        var profile = GlobalProfile();
        if (profile == null) { Criterion.globalLastKnownError = "Could not find 'Global' profile."; return false; }

        if (profile.HasSettings<Vignette>() == false) { Criterion.globalLastKnownError = "'Global' profile is missing 'Vignette' effect."; return false; }

        var settings = profile.GetSetting<Vignette>();
        if (!settings.intensity.overrideState.Equals(true) || !Mathf.Approximately(settings.intensity.value, 0.5f)) { Criterion.globalLastKnownError = "'Vignette' Intensity must be 0.5."; return false; }
        return true;
    }

    //c
    public bool C1_ColourGrading()
    {
        var profile = GlobalProfile();
        if (profile == null) { Criterion.globalLastKnownError = "Could not find 'Global' profile."; return false; }

        if (!profile.HasSettings<ColorGrading>()) { Criterion.globalLastKnownError = "'Global' profile must handle 'ColorGrading' effect."; return false; }
        return true;
    }
    public bool C3_ColourGradingCurve()
    {
        var profile = GlobalProfile();
        if (profile == null) { Criterion.globalLastKnownError = "Could not find 'Global' profile."; return false; }

        if (profile.HasSettings<ColorGrading>() == false) { Criterion.globalLastKnownError = "'Global' profile is missing 'ColorGrading' effect."; return false; }

        var settings = profile.GetSetting<ColorGrading>();
        //0,0|0.6350574,0|0.7011494,1|0.7729885,0
        //Debug.Log(string.Join("|", settings.hueVsSatCurve.value.curve.keys.ToList().Select(k => k.time + "," + k.value)));

        var curve = settings.hueVsSatCurve.value.curve;

        if (!settings.hueVsSatCurve.overrideState.Equals(true)) { Criterion.globalLastKnownError = "'ColorGrading' Hue vs Saturation override must be enabled."; return false; }

        //curve.keys.Length >= 4 &&
        if (curve.keys.Count(k => k.value == 0) < 2) { Criterion.globalLastKnownError = "'ColorGrading' Curve needs at least 2 points at value 0."; return false; }
        if (curve.keys.Count(k => k.value == 1) != 1) { Criterion.globalLastKnownError = "'ColorGrading' Curve needs exactly 1 point at value 1."; return false; }
        if (curve.keys.Count(k => k.time > 0.45f) < 3) { Criterion.globalLastKnownError = "'ColorGrading' Curve needs at least 3 points in the second half."; return false; }
        
        return true;
    }
    public bool C4_Bloom()
    {
        var profile = GlobalProfile();
        if (profile == null) { Criterion.globalLastKnownError = "Could not find 'Global' profile."; return false; }

        if (!profile.HasSettings<Bloom>()) { Criterion.globalLastKnownError = "'Global' profile must handle 'Bloom' effect."; return false; }
        return true;
    }

    //d
    public bool D1_Volume()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<PostProcessVolume>("DarkArea")) { Criterion.globalLastKnownError = "'DarkArea' is missing 'PostProcessVolume' component."; return false; }
        return true;
    }
    public bool D1_VolumePosition()
    {
        var t = CommonTutorialCallbacks.GameObjectComponent<Transform>("DarkArea");
        if (t == null) { Criterion.globalLastKnownError = "Could not find 'DarkArea' GameObject."; return false; }

        if (t.position == Vector3.zero || !Mathf.Approximately(t.position.z, 0)) { Criterion.globalLastKnownError = "'DarkArea' should be positioned away from origin, but with Z=0."; return false; }
        return true;
    }
    public bool D1_VolumeLayer()
    {
        var cam = GameObject.Find("DarkArea");
        if (cam == null) { Criterion.globalLastKnownError = "Could not find 'DarkArea' GameObject."; return false; }

        if (cam.layer != LayerMask.NameToLayer("PostProcessingEffects")) { Criterion.globalLastKnownError = "'DarkArea' Layer must be 'PostProcessingEffects'."; return false; }
        return true;
    }
    static PostProcessProfile DarkAreaProfile()
    {
        return AssetDatabase.LoadAssetAtPath<PostProcessProfile>("Assets/Profiles/DarkArea Profile.asset");
    }
    public bool D2_DarkAreaProfile()
    {
        if (DarkAreaProfile() == null) { Criterion.globalLastKnownError = "Could not find 'DarkArea Profile.asset' in 'Assets/Profiles'."; return false; }
        return true;
    }
    public bool D2_Global()
    {
        var pv = CommonTutorialCallbacks.GameObjectComponent<PostProcessVolume>("DarkArea");
        if (pv == null) { Criterion.globalLastKnownError = "'DarkArea' is missing 'PostProcessVolume' component."; return false; }

        if (!pv.isGlobal) { Criterion.globalLastKnownError = "'DarkArea' PostProcessVolume should be global (for now)."; return false; }
        return true;
    }
    public bool D3_DarkAreaColorGrading()
    {
        var profile = DarkAreaProfile();
        if (profile == null) { Criterion.globalLastKnownError = "Could not find 'DarkArea Profile'."; return false; }

        if (profile.HasSettings<ColorGrading>() == false) { Criterion.globalLastKnownError = "'DarkArea Profile' is missing 'ColorGrading' effect."; return false; }

        var settings = profile.GetSetting<ColorGrading>();
        
        if (!settings.temperature.overrideState.Equals(true) || !Mathf.Approximately(settings.temperature.value, -40)) { Criterion.globalLastKnownError = "'ColorGrading' Temperature must be -40."; return false; }
        if (!settings.gamma.overrideState.Equals(true) || settings.gamma.value.w >= -0.4f) { Criterion.globalLastKnownError = "'ColorGrading' Gamma W must be less than -0.4."; return false; }
        
        return true;
    }
    public bool D4_NotGlobal()
    {
        var pv = CommonTutorialCallbacks.GameObjectComponent<PostProcessVolume>("DarkArea");
        if (pv == null) { Criterion.globalLastKnownError = "'DarkArea' is missing 'PostProcessVolume' component."; return false; }

        if (pv.isGlobal) { Criterion.globalLastKnownError = "Uncheck 'Is Global' on 'DarkArea' PostProcessVolume."; return false; }
        return true;
    }
    public bool D4_Trigger()
    {
        var pp = CommonTutorialCallbacks.GameObjectComponent<PostProcessLayer>("Main Camera");
        if (pp == null) { Criterion.globalLastKnownError = "'Main Camera' is missing 'PostProcessLayer' component."; return false; }

        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player'."; return false; }

        if (pp.volumeTrigger != player.transform) { Criterion.globalLastKnownError = "Assign 'Player' to 'PostProcessLayer' Trigger."; return false; }
        return true;
    }
    public bool D5_BlendDistance()
    {
        var pv = CommonTutorialCallbacks.GameObjectComponent<PostProcessVolume>("DarkArea");
        if (pv == null) { Criterion.globalLastKnownError = "'DarkArea' is missing 'PostProcessVolume'."; return false; }

        if (!Mathf.Approximately(pv.blendDistance, 0.5f)) { Criterion.globalLastKnownError = "'DarkArea' Blend Distance must be 0.5."; return false; }
        return true;
    }
    
    //e
    public bool E1_ScareRadius()
    {
        var radius = GameObject.Find("ScareRadius");
        if (radius == null) { Criterion.globalLastKnownError = "Could not find 'ScareRadius' GameObject."; return false; }

        var ai = GameObject.Find("ComplexAlmostHumanLikeAI");
        if (ai == null) { Criterion.globalLastKnownError = "Could not find 'ComplexAlmostHumanLikeAI' GameObject."; return false; }

        if (radius.transform.parent != ai.transform) { Criterion.globalLastKnownError = "'ScareRadius' must be a child of 'ComplexAlmostHumanLikeAI'."; return false; }
        if (radius.transform.localPosition != Vector3.zero) { Criterion.globalLastKnownError = "'ScareRadius' Position must be (0, 0, 0)."; return false; }
        return true;
    }
    public bool E1_RadiusSphere()
    {
        var sphere = CommonTutorialCallbacks.GameObjectComponent<SphereCollider>("ScareRadius");
        if (sphere == null) { Criterion.globalLastKnownError = "'ScareRadius' is missing SphereCollider."; return false; }
        if (!Mathf.Approximately(sphere.radius, 2)) { Criterion.globalLastKnownError = "'ScareRadius' Radius must be 2."; return false; }
        return true;
    }
    public bool E1_RadiusLayer()
    {
        var cam = GameObject.Find("ScareRadius");
        if (cam == null) { Criterion.globalLastKnownError = "Could not find 'ScareRadius' GameObject."; return false; }

        if (cam.layer != LayerMask.NameToLayer("PostProcessingEffects")) { Criterion.globalLastKnownError = "'ScareRadius' Layer must be 'PostProcessingEffects'."; return false; }
        return true;
    }
    public bool E1_RadiusVolume()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<PostProcessVolume>("ScareRadius")) { Criterion.globalLastKnownError = "'ScareRadius' is missing 'PostProcessVolume' component."; return false; }
        return true;
    }
    static PostProcessProfile ScareProfile()
    {
        return AssetDatabase.LoadAssetAtPath<PostProcessProfile>("Assets/Profiles/ScareRadius Profile.asset");
    }
    public bool E2_ScareProfile()
    {
        if (ScareProfile() == null) { Criterion.globalLastKnownError = "Could not find 'ScareRadius Profile.asset' in 'Assets/Profiles'."; return false; }
        return true;
    }
    public bool E2_ScareRadiusChromaticAberration()
    {
        var profile = ScareProfile();
        if (profile == null) { Criterion.globalLastKnownError = "Could not find 'ScareRadius Profile'."; return false; }

        if (profile.HasSettings<ChromaticAberration>() == false) { Criterion.globalLastKnownError = "'ScareRadius Profile' is missing 'ChromaticAberration' effect."; return false; }

        var settings = profile.GetSetting<ChromaticAberration>();
        if (!settings.intensity.overrideState.Equals(true) || !Mathf.Approximately(settings.intensity.value, 0.5f)) { Criterion.globalLastKnownError = "'ChromaticAberration' Intensity must be 0.5."; return false; }
        return true;
    }

    //f
    public bool F1_PulseScript()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<ChromaticAberrationPulse>("ScareRadius")) { Criterion.globalLastKnownError = "'ScareRadius' is missing 'ChromaticAberrationPulse' script."; return false; }
        return true;
    }
}
#endif