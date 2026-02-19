#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.Tutorials.Core.Editor;
using System.Linq;
using UnityEditor.Animations;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

/// <summary>
/// Implement your Tutorial callbacks here.
/// </summary>
[CreateAssetMenu(fileName = DefaultFileName, menuName = "Tutorials/" + DefaultFileName + " Instance")]
public class Tutorial10Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial10Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial10Callbacks>(DefaultFileName);
    }

    //b
    public bool B1_Parent()
    {
        var cam = GameObject.Find("Main Camera");
        if (cam == null) return false;

        var player = GameObject.Find("Player");
        if (player == null) return false;

        return cam.transform.parent != null && cam.transform.parent.Equals(player.transform);
    }
    public bool B2_Parent()
    {
        var cam = GameObject.Find("Main Camera");
        if (cam == null) return false;

        return cam.transform.parent == null;
    }
    static CinemachineVirtualCamera GetVcam()
    {
        return CommonTutorialCallbacks.GameObjectComponent<CinemachineVirtualCamera>("Virtual Camera");
    }
    static CinemachineVirtualCamera GetChestVcam()
    {
        return CommonTutorialCallbacks.GameObjectComponent<CinemachineVirtualCamera>("CameraChestZoomed");
    }
    public bool B4_Vcam()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        var cam = GameObject.Find("Main Camera");
        if (cam == null) return false;

        return true;
    }
    public bool B7_Follow()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        var player = GameObject.Find("Player");
        if (player == null) return false;

        return vcam.Follow != null && vcam.Follow.Equals(player.transform);
    }
    public bool B7_FramingTransposer()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        var body = vcam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (body == null) return false;

        Debug.Log(body);

        return body is CinemachineFramingTransposer;
    }
    public bool B7_Zoom()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        return vcam.m_Lens.OrthographicSize.Equals(4);
    }

    //c
    public bool C1_Deadzone()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        var composer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (composer == null) return false;

        return composer.m_DeadZoneWidth > 0 && composer.m_DeadZoneHeight > 0;
    }

    //d
    public bool D1_Confiner()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<CinemachineConfiner2D>("Virtual Camera");
    }
    public bool D2_PolygonCollider()
    {
        return CommonTutorialCallbacks.GameObjectComponent<PolygonCollider2D>("WorldBounds") != null;
    }
    public bool D2_ConfinerLinked()
    {
        var confiner = CommonTutorialCallbacks.GameObjectComponent<CinemachineConfiner2D>("Virtual Camera");
        if (confiner == null) return false;

        var worldBounds = CommonTutorialCallbacks.GameObjectComponent<PolygonCollider2D>("WorldBounds");
        if (worldBounds == null) return false;

        return confiner.m_BoundingShape2D != null && confiner.m_BoundingShape2D.Equals(worldBounds);
    }
    public bool D3_Trigger()
    {
        var worldBounds = CommonTutorialCallbacks.GameObjectComponent<PolygonCollider2D>("WorldBounds");
        if (worldBounds == null) return false;

        return worldBounds.isTrigger;
    }

    //e
    static TimelineAsset GetTimeline()
    {
        return AssetDatabase.LoadAssetAtPath<TimelineAsset>("Assets/Chest/ChestTimeline.playable");
    }
    static PlayableDirector GetDirector()
    {
        return CommonTutorialCallbacks.GameObjectComponent<PlayableDirector>("Chest");
    }
    static CinemachineTrack GetCinemachineTrack()
    {
        var timeline = GetTimeline();
        if (timeline == null) return null;

        return timeline.GetOutputTracks().Where(t => t is CinemachineTrack).Cast<CinemachineTrack>().FirstOrDefault();
    }
    public bool E3_TimelineAsset()
    {
        return GetTimeline() != null;
    }
    public bool E4_TimelineLinked()
    {
        var chest = CommonTutorialCallbacks.GameObjectComponent<TreasureOpen>("Chest");
        if (chest == null) return false;

        var director = CommonTutorialCallbacks.GameObjectComponent<PlayableDirector>("Chest");
        if (director == null) return false;

        return chest.directorToPlay != null && chest.directorToPlay.Equals(director);
    }
    public bool E4_PlayOnAwake()
    {
        var director = GetDirector();
        if (director == null) return false;

        return director.playOnAwake.Equals(false);
    }
    public bool E5_CinemachineTrack()
    {
        var cinemachineTrack = GetCinemachineTrack();
        return cinemachineTrack != null;
    }
    public bool E5_CinemachineTrackBrain()
    {
        var director = GetDirector();
        if (director == null) return false;

        var cinemachineTrack = GetCinemachineTrack();
        if (cinemachineTrack == null) return false;

        var binding = director.GetGenericBinding(cinemachineTrack);
        if (binding == null) return false;

        var brain = CommonTutorialCallbacks.GameObjectComponent<CinemachineBrain>("Main Camera");
        if (brain == null) return false;

        return binding.Equals(brain);
    }
    static TimelineClip GetFirstCinemachineShotClip()
    {
        var cinemachineTrack = GetCinemachineTrack();
        if (cinemachineTrack == null) return null;

        var clip = cinemachineTrack.GetClips().Where(c => c.asset != null && c.asset is CinemachineShot).OrderBy(c => c.start).FirstOrDefault();
        if (clip == null) return null;

        if (clip.start != 0) return null;

        return clip;
    }
    static TimelineClip GetSecondCinemachineShotClip()
    {
        var cinemachineTrack = GetCinemachineTrack();
        if (cinemachineTrack == null) return null;

        var clip = cinemachineTrack.GetClips().Where(c => c.asset != null && c.asset is CinemachineShot).OrderBy(c => c.start).Skip(1).FirstOrDefault(); 
        if (clip == null) return null;

        if (clip.start == 0) return null;

        return clip;
    }
    static TimelineClip GetThirdCinemachineShotClip()
    {
        var cinemachineTrack = GetCinemachineTrack();
        if (cinemachineTrack == null) return null;

        var clip = cinemachineTrack.GetClips().Where(c => c.asset != null && c.asset is CinemachineShot).OrderBy(c => c.start).Skip(2).FirstOrDefault(); 
        if (clip == null) return null;

        if (clip.start == 0) return null;

        return clip;
    }

    static CinemachineShot GetFirstCinemachineShot()
    {
        return GetFirstCinemachineShotClip()?.asset as CinemachineShot;
    }
    static CinemachineShot GetSecondCinemachineShot()
    {
        return GetSecondCinemachineShotClip()?.asset as CinemachineShot;
    }
    static CinemachineShot GetThirdCinemachineShot()
    {
        return GetThirdCinemachineShotClip()?.asset as CinemachineShot;
    }

    public bool E6_CinemachineShot()
    {
        return GetFirstCinemachineShot();
    }
    public bool E6_CinemachineShotLinked()
    {
        var shot = GetFirstCinemachineShot();
        if (shot == null) return false;

        var vcam = GetVcam();
        if (vcam == null) return false;

        var director = GetDirector();
        if (director == null) return false;

        var found = false;
        var linkedCam = director.GetReferenceValue(shot.VirtualCamera.exposedName, out found);
        if (found == false || linkedCam == null) return false;

        return linkedCam.Equals(vcam);
    }
    public bool E7_CinemachineShot()
    {
        var shot = GetSecondCinemachineShot();
        if (shot == null) return false;

        var vcam = GetChestVcam();
        if (vcam == null) return false;

        var director = GetDirector();
        if (director == null) return false;

        bool found;
        var linkedCam = director.GetReferenceValue(shot.VirtualCamera.exposedName, out found);
        if (found == false || linkedCam == null) return false;

        return linkedCam.Equals(vcam);
    }
    public bool E7_Vcam()
    {
        return GetChestVcam() != null;
    }
    public bool E7_Follow()
    {
        var vcam = GetChestVcam();
        if (vcam == null) return false;

        var chest = GameObject.Find("Chest");
        if (chest == null) return false;

        return vcam.Follow != null && vcam.Follow.Equals(chest.transform) && vcam.m_Lens.OrthographicSize <= 2.5f;
    }
    public bool E7_Priority()
    {
        var vcam = GetChestVcam();
        if (vcam == null) return false;

        return vcam.Priority.Equals(0);
    }

    //f
    public bool F1_Blending()
    {
        var firstShot = GetFirstCinemachineShotClip();
        if (firstShot == null) return false;

        var secondShot = GetSecondCinemachineShotClip();
        if (secondShot == null) return false;

        return secondShot.start < firstShot.end;
    }
    public bool F2_Length()
    {
        var firstShot = GetFirstCinemachineShotClip();
        if (firstShot == null) return false;

        return firstShot.duration <= 5 && F1_Blending();
    }
    static AnimationTrack GetAnimationTrack()
    {
        var timeline = GetTimeline();
        if (timeline == null) return null;

        return timeline.GetOutputTracks().Where(t => t is AnimationTrack).Cast<AnimationTrack>().FirstOrDefault();
    }
    static TimelineClip GetFirstAnimationShotClip()
    {
        var animationTrack = GetAnimationTrack();
        if (animationTrack == null) return null;

        var clip = animationTrack.GetClips().Where(c => c.asset != null && c.asset is AnimationPlayableAsset).OrderBy(c => c.start).FirstOrDefault();
        if (clip == null) return null;

        if (clip.start != 0) return null;

        return clip;
    }
    static TimelineClip GetSecondAnimationShotClip()
    {
        var animationTrack = GetAnimationTrack();
        if (animationTrack == null) return null;

        var clip = animationTrack.GetClips().Where(c => c.asset != null && c.asset is AnimationPlayableAsset).OrderBy(c => c.start).Skip(1).FirstOrDefault(); if (clip == null) return null;

        if (clip.start == 0) return null;

        return clip;
    }

    static AnimationPlayableAsset GetFirstAnimationShot()
    {
        return GetFirstAnimationShotClip()?.asset as AnimationPlayableAsset;
    }
    static AnimationPlayableAsset GetSecondAnimationShot()
    {
        return GetSecondAnimationShotClip()?.asset as AnimationPlayableAsset;
    }

    public bool F3_AnimationTrack()
    {
        return GetAnimationTrack() != null;
    }
    public bool F3_AnimationTrackClips()
    {
        var firstClip = GetFirstAnimationShot();
        if (firstClip == null) return false;

        var secondClip = GetSecondAnimationShot();
        if (secondClip == null) return false;

        return firstClip.clip != null && firstClip.clip.name.Contains("Idle") &&
            secondClip.clip != null && secondClip.clip.name.Contains("Open");
    }
    public bool F4_DirectorWrap()
    {
        var director = GetDirector();
        if (director == null) return false;

        return director.extrapolationMode.Equals(DirectorWrapMode.Hold);
    }
    public bool F4_ThirdClip()
    {
        var shot = GetThirdCinemachineShot();
        if (shot == null) return false;

        var vcam = GetVcam();
        if (vcam == null) return false;

        var director = GetDirector();
        if (director == null) return false;

        bool found;
        var linkedCam = director.GetReferenceValue(shot.VirtualCamera.exposedName, out found);
        if (found == false || linkedCam == null) return false;

        return linkedCam.Equals(vcam);
    }
    public bool F5_Debug()
    {
        var brain = CommonTutorialCallbacks.GameObjectComponent<CinemachineBrain>("Main Camera");
        if (brain == null) return false;

        return brain.m_ShowDebugText;
    }
    public bool F6_Noise()
    {
        var vcam = GetChestVcam();
        if (vcam == null) return false;

        return vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() != null;
    }
    public bool F6_NoiseProfile()
    {
        var vcam = GetChestVcam();
        if (vcam == null) return false;

        var noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null) return false;

        return noise.m_NoiseProfile != null && noise.m_NoiseProfile.name.Equals("Handheld_normal_mild");
    }

    //g
    public bool G1_Player()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<EightWayMovement>("Player");
    }
    public bool G1_PlayerPosition()
    {
        var player = GameObject.Find("Player");
        if (player == null) return false;

        return player.transform.position.x.Equals(-6) && player.transform.position.y.Equals(2);
    }
    public bool G2_Vcam()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        var cam = GameObject.Find("Main Camera");
        if (cam == null) return false;

        return true;
    }
    public bool G2_Follow()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        var player = GameObject.Find("Player");
        if (player == null) return false;

        return vcam.Follow != null && vcam.Follow.Equals(player.transform);
    }
    public bool G2_Zoom()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        return vcam.m_Lens.OrthographicSize.Equals(4);
    }
    public bool G3_DollyPointPosition()
    {
        var dolly = CommonTutorialCallbacks.GameObjectComponent<CinemachineSmoothPath>("Dolly Track");
        if (dolly == null) return false;

        return dolly.transform.position == Vector3.zero;
    }
    public bool G3_DollyPoint1()
    {
        var dolly = CommonTutorialCallbacks.GameObjectComponent<CinemachineSmoothPath>("Dolly Track");
        if (dolly == null) return false;

        var points = dolly.m_Waypoints;
        if (points.Length < 1) return false;

        return points[0].position.Equals(new Vector3(-5, 2, -10));
    }
    public bool G3_DollyPoint2()
    {
        var dolly = CommonTutorialCallbacks.GameObjectComponent<CinemachineSmoothPath>("Dolly Track");
        if (dolly == null) return false;

        var points = dolly.m_Waypoints;
        if (points.Length < 2) return false;

        return points[1].position.Equals(new Vector3(5, 4, -10));
    }
    public bool G3_DollyPoint3Plus()
    {
        var dolly = CommonTutorialCallbacks.GameObjectComponent<CinemachineSmoothPath>("Dolly Track");
        if (dolly == null) return false;

        var points = dolly.m_Waypoints;
        if (points.Length < 5) return false;

        //check unique
        var stored = new System.Collections.Generic.List<Vector3>();
        foreach (var point in points)
        {
            var pos = point.position;
            if (stored.Any(p => p.Equals(pos))) return false;
            stored.Add(pos);
        }
        return true;
    }
    public bool G4_AutoDolly()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        var dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        if (dolly == null) return false;

        return dolly.m_AutoDolly.m_Enabled;
    }
    public bool H1_Vcam()
    {
        var vcam = GetVcam();
        if (vcam == null) return false;

        var cam = GameObject.Find("Main Camera");
        if (cam == null) return false;

        return true;
    }
    public bool H2_Balls()
    {
        var targetGroup = CommonTutorialCallbacks.GameObjectComponent<CinemachineTargetGroup>("Target Group");
        if (targetGroup == null) return false;

        var balls = CommonTutorialCallbacks.GameObjectsStartingWith("Ball");
        foreach(var ball in balls)
        {
            if (targetGroup.FindMember(ball.transform) == -1) return false;
        }

        return true;
    }

    //exercise
    public bool Exercise_RenderOrder()
    {
        var player = CommonTutorialCallbacks.GameObjectComponent<SpriteRenderer>("Player");
        if (player == null) return false;

        var chest = CommonTutorialCallbacks.GameObjectComponent<SpriteRenderer>("Chest");
        if (chest == null) return false;

        return player.sortingOrder > chest.sortingOrder;
    }
    static CinemachineVirtualCamera GetVcam2()
    {
        return CommonTutorialCallbacks.GameObjectComponent<CinemachineVirtualCamera>("ZoomedCamera");
    }
    public bool Exercise_Vcam2()
    {
        var vcam = GetVcam2();
        if (vcam == null) return false;

        var player = GameObject.Find("Player");
        if (player == null) return false;

        return vcam.Follow != null && vcam.Follow.Equals(player.transform) &&
            vcam.m_Lens.OrthographicSize.Equals(2) &&
            vcam.m_Lens.Dutch.Equals(180);// &&
            //vcam.Priority.Equals(0);
    }
    public bool Exercise_LinkVcam2()
    {
        var vcam = GetVcam2();
        if (vcam == null) return false;

        var script = CommonTutorialCallbacks.GameObjectComponent<CharacterZoomZoneScript>("CharacterZoomZone");
        if (script == null) return false;

        return script.virtualCameraToActivate != null && script.virtualCameraToActivate.Equals(vcam);
    }
}

#endif