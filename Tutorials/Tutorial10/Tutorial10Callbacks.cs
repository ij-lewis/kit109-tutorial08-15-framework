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
        if (cam == null) { Criterion.globalLastKnownError = "Could not find 'Main Camera'."; return false; }

        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        if (cam.transform.parent == null || !cam.transform.parent.Equals(player.transform)) { Criterion.globalLastKnownError = "'Main Camera' needs to be a child of 'Player'. Drag 'Main Camera' onto 'Player' in the Hierarchy."; return false; }
        return true;
    }
    public bool B2_Parent()
    {
        var cam = GameObject.Find("Main Camera");
        if (cam == null) { Criterion.globalLastKnownError = "Could not find 'Main Camera'."; return false; }

        if (cam.transform.parent != null) { Criterion.globalLastKnownError = "'Main Camera' should not be a child of any object. Drag it out to the root of the Hierarchy."; return false; }
        return true;
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
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera' with CinemachineVirtualCamera component."; return false; }

        var cam = GameObject.Find("Main Camera");
        if (cam == null) { Criterion.globalLastKnownError = "Could not find 'Main Camera'."; return false; }

        return true;
    }
    public bool B7_Follow()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        if (vcam.Follow == null || !vcam.Follow.Equals(player.transform)) { Criterion.globalLastKnownError = "'Virtual Camera' Follow target is not set to 'Player'."; return false; }
        return true;
    }
    public bool B7_FramingTransposer()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var body = vcam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (body == null) { Criterion.globalLastKnownError = "'Virtual Camera' Body component is missing."; return false; }

        Debug.Log(body);

        if (!(body is CinemachineFramingTransposer)) { Criterion.globalLastKnownError = "'Virtual Camera' Body is not set to 'Framing Transposer'."; return false; }
        return true;
    }
    public bool B7_Zoom()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        if (!vcam.m_Lens.OrthographicSize.Equals(4)) { Criterion.globalLastKnownError = "'Virtual Camera' Orthographic Size is not 4."; return false; }
        return true;
    }

    //c
    public bool C1_Deadzone()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var composer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (composer == null) { Criterion.globalLastKnownError = "'Virtual Camera' is missing Framing Transposer."; return false; }

        if (composer.m_DeadZoneWidth <= 0 || composer.m_DeadZoneHeight <= 0) { Criterion.globalLastKnownError = "Dead Zone Width and Height must be greater than 0."; return false; }
        return true;
    }

    //d
    public bool D1_Confiner()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<CinemachineConfiner2D>("Virtual Camera")) { Criterion.globalLastKnownError = "'Virtual Camera' is missing 'CinemachineConfiner2D' extension."; return false; }
        return true;
    }
    public bool D2_PolygonCollider()
    {
        if (CommonTutorialCallbacks.GameObjectComponent<PolygonCollider2D>("WorldBounds") == null) { Criterion.globalLastKnownError = "'WorldBounds' GameObject is missing a PolygonCollider2D."; return false; }
        return true;
    }
    public bool D2_ConfinerLinked()
    {
        var confiner = CommonTutorialCallbacks.GameObjectComponent<CinemachineConfiner2D>("Virtual Camera");
        if (confiner == null) { Criterion.globalLastKnownError = "'Virtual Camera' is missing CinemachineConfiner2D."; return false; }

        var worldBounds = CommonTutorialCallbacks.GameObjectComponent<PolygonCollider2D>("WorldBounds");
        if (worldBounds == null) { Criterion.globalLastKnownError = "'WorldBounds' is missing PolygonCollider2D."; return false; }

        if (confiner.m_BoundingShape2D == null || !confiner.m_BoundingShape2D.Equals(worldBounds)) { Criterion.globalLastKnownError = "CinemachineConfiner2D Bounding Shape 2D is not set to 'WorldBounds'."; return false; }
        return true;
    }
    public bool D3_Trigger()
    {
        var worldBounds = CommonTutorialCallbacks.GameObjectComponent<PolygonCollider2D>("WorldBounds");
        if (worldBounds == null) { Criterion.globalLastKnownError = "'WorldBounds' is missing PolygonCollider2D."; return false; }

        if (!worldBounds.isTrigger) { Criterion.globalLastKnownError = "'WorldBounds' PolygonCollider2D 'Is Trigger' should be checked."; return false; }
        return true;
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
        if (GetTimeline() == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Chest/ChestTimeline.playable'."; return false; }
        return true;
    }
    public bool E4_TimelineLinked()
    {
        var chest = CommonTutorialCallbacks.GameObjectComponent<TreasureOpen>("Chest");
        if (chest == null) { Criterion.globalLastKnownError = "'Chest' is missing 'TreasureOpen' script."; return false; }

        var director = CommonTutorialCallbacks.GameObjectComponent<PlayableDirector>("Chest");
        if (director == null) { Criterion.globalLastKnownError = "'Chest' is missing 'PlayableDirector' component."; return false; }

        if (chest.directorToPlay == null || !chest.directorToPlay.Equals(director)) { Criterion.globalLastKnownError = "'TreasureOpen' script 'Director to Play' field is not assigned to the Chest's Director."; return false; }
        return true;
    }
    public bool E4_PlayOnAwake()
    {
        var director = GetDirector();
        if (director == null) { Criterion.globalLastKnownError = "Could not find PlayableDirector on 'Chest'."; return false; }

        if (director.playOnAwake != false) { Criterion.globalLastKnownError = "'Play On Awake' should be unchecked on the Chest's PlayableDirector."; return false; }
        return true;
    }
    public bool E5_CinemachineTrack()
    {
        var cinemachineTrack = GetCinemachineTrack();
        if (cinemachineTrack == null) { Criterion.globalLastKnownError = "Timeline is missing a Cinemachine Track."; return false; }
        return true;
    }
    public bool E5_CinemachineTrackBrain()
    {
        var director = GetDirector();
        if (director == null) { Criterion.globalLastKnownError = "Could not find PlayableDirector on 'Chest'."; return false; }

        var cinemachineTrack = GetCinemachineTrack();
        if (cinemachineTrack == null) { Criterion.globalLastKnownError = "Timeline is missing a Cinemachine Track."; return false; }

        var binding = director.GetGenericBinding(cinemachineTrack);
        if (binding == null) { Criterion.globalLastKnownError = "Cinemachine Track is not bound to anything in the Director."; return false; }

        var brain = CommonTutorialCallbacks.GameObjectComponent<CinemachineBrain>("Main Camera");
        if (brain == null) { Criterion.globalLastKnownError = "'Main Camera' is missing CinemachineBrain."; return false; }

        if (!binding.Equals(brain)) { Criterion.globalLastKnownError = "Cinemachine Track should be bound to the 'Main Camera' (CinemachineBrain)."; return false; }
        return true;
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
        if (GetFirstCinemachineShot() == null) { Criterion.globalLastKnownError = "Cinemachine Track is missing a Shot clip starting at time 0."; return false; }
        return true;
    }
    public bool E6_CinemachineShotLinked()
    {
        var shot = GetFirstCinemachineShot();
        if (shot == null) { Criterion.globalLastKnownError = "Cinemachine Track is missing the first Shot clip."; return false; }

        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var director = GetDirector();
        if (director == null) { Criterion.globalLastKnownError = "Could not find PlayableDirector on 'Chest'."; return false; }

        var found = false;
        var linkedCam = director.GetReferenceValue(shot.VirtualCamera.exposedName, out found);
        if (found == false || linkedCam == null || !linkedCam.Equals(vcam))
        {
             Criterion.globalLastKnownError = "First Shot clip is not linked to 'Virtual Camera'. Check the exposed reference.";
             return false;
        }
        return true;
    }
    public bool E7_CinemachineShot()
    {
        var shot = GetSecondCinemachineShot();
        if (shot == null) { Criterion.globalLastKnownError = "Cinemachine Track is missing the second Shot clip."; return false; }

        var vcam = GetChestVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'CameraChestZoomed'."; return false; }

        var director = GetDirector();
        if (director == null) { Criterion.globalLastKnownError = "Could not find PlayableDirector on 'Chest'."; return false; }

        bool found;
        var linkedCam = director.GetReferenceValue(shot.VirtualCamera.exposedName, out found);
        if (found == false || linkedCam == null || !linkedCam.Equals(vcam))
        {
             Criterion.globalLastKnownError = "Second Shot clip is not linked to 'CameraChestZoomed'.";
             return false;
        }
        return true;
    }
    public bool E7_Vcam()
    {
        if (GetChestVcam() == null) { Criterion.globalLastKnownError = "Could not find 'CameraChestZoomed' Virtual Camera."; return false; }
        return true;
    }
    public bool E7_Follow()
    {
        var vcam = GetChestVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'CameraChestZoomed'."; return false; }

        var chest = GameObject.Find("Chest");
        if (chest == null) { Criterion.globalLastKnownError = "Could not find 'Chest' GameObject."; return false; }

        if (vcam.Follow == null || !vcam.Follow.Equals(chest.transform)) { Criterion.globalLastKnownError = "'CameraChestZoomed' should Follow 'Chest'."; return false; }
        if (vcam.m_Lens.OrthographicSize > 2.51f) { Criterion.globalLastKnownError = "'CameraChestZoomed' Orthographic Size must be <= 2.5."; return false; }
        return true;
    }
    public bool E7_Priority()
    {
        var vcam = GetChestVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'CameraChestZoomed'."; return false; }

        if (vcam.Priority != 0) { Criterion.globalLastKnownError = "'CameraChestZoomed' Priority should be 0."; return false; }
        return true;
    }

    //f
    public bool F1_Blending()
    {
        var firstShot = GetFirstCinemachineShotClip();
        if (firstShot == null) { Criterion.globalLastKnownError = "First shot missing."; return false; }

        var secondShot = GetSecondCinemachineShotClip();
        if (secondShot == null) { Criterion.globalLastKnownError = "Second shot missing."; return false; }

        if (secondShot.start >= firstShot.end) { Criterion.globalLastKnownError = "Shots do not overlap. Second shot must start before First shot ends."; return false; }
        return true;
    }
    public bool F2_Length()
    {
        var firstShot = GetFirstCinemachineShotClip();
        if (firstShot == null) { Criterion.globalLastKnownError = "First shot missing."; return false; }

        if (firstShot.duration > 5) { Criterion.globalLastKnownError = "First shot duration must be <= 5 seconds."; return false; }
        return F1_Blending();
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
        if (GetAnimationTrack() == null) { Criterion.globalLastKnownError = "Timeline is missing an Animation Track."; return false; }
        return true;
    }
    public bool F3_AnimationTrackClips()
    {
        var firstClip = GetFirstAnimationShot();
        if (firstClip == null) { Criterion.globalLastKnownError = "Animation Track missing first clip."; return false; }

        var secondClip = GetSecondAnimationShot();
        if (secondClip == null) { Criterion.globalLastKnownError = "Animation Track missing second clip."; return false; }

        if (firstClip.clip == null || !firstClip.clip.name.Contains("Idle")) { Criterion.globalLastKnownError = "First Animation Clip should be 'Idle'."; return false; }
        if (secondClip.clip == null || !secondClip.clip.name.Contains("Open")) { Criterion.globalLastKnownError = "Second Animation Clip should be 'Open'."; return false; }
        return true;
    }
    public bool F4_DirectorWrap()
    {
        var director = GetDirector();
        if (director == null) { Criterion.globalLastKnownError = "Could not find PlayableDirector."; return false; }

        if (director.extrapolationMode != DirectorWrapMode.Hold) { Criterion.globalLastKnownError = "Director Wrap Mode should be 'Hold'."; return false; }
        return true;
    }
    public bool F4_ThirdClip()
    {
        var shot = GetThirdCinemachineShot();
        if (shot == null) { Criterion.globalLastKnownError = "Cinemachine Track is missing the third Shot clip."; return false; }

        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var director = GetDirector();
        if (director == null) { Criterion.globalLastKnownError = "Could not find PlayableDirector."; return false; }

        bool found;
        var linkedCam = director.GetReferenceValue(shot.VirtualCamera.exposedName, out found);
        if (found == false || linkedCam == null || !linkedCam.Equals(vcam))
        {
             Criterion.globalLastKnownError = "Third Shot clip is not linked to 'Virtual Camera'.";
             return false;
        }
        return true;
    }
    public bool F5_Debug()
    {
        var brain = CommonTutorialCallbacks.GameObjectComponent<CinemachineBrain>("Main Camera");
        if (brain == null) { Criterion.globalLastKnownError = "Main Camera missing CinemachineBrain."; return false; }

        if (!brain.m_ShowDebugText) { Criterion.globalLastKnownError = "CinemachineBrain 'Show Debug Text' should be checked."; return false; }
        return true;
    }
    public bool F6_Noise()
    {
        var vcam = GetChestVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'CameraChestZoomed'."; return false; }

        if (vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() == null) { Criterion.globalLastKnownError = "'CameraChestZoomed' is missing Noise component (Basic Multi Channel Perlin)."; return false; }
        return true;
    }
    public bool F6_NoiseProfile()
    {
        var vcam = GetChestVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'CameraChestZoomed'."; return false; }

        var noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null) { Criterion.globalLastKnownError = "'CameraChestZoomed' is missing Noise component."; return false; }

        if (noise.m_NoiseProfile == null || !noise.m_NoiseProfile.name.Equals("Handheld_normal_mild")) { Criterion.globalLastKnownError = "Noise Profile should be set to 'Handheld_normal_mild'."; return false; }
        return true;
    }

    //g
    public bool G1_Player()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<EightWayMovement>("Player")) { Criterion.globalLastKnownError = "Player GameObject is missing 'EightWayMovement' script."; return false; }
        return true;
    }
    public bool G1_PlayerPosition()
    {
        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        if (!player.transform.position.x.Equals(-6) || !player.transform.position.y.Equals(2)) { Criterion.globalLastKnownError = "Player position should be (-6, 2)."; return false; }
        return true;
    }
    public bool G2_Vcam()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var cam = GameObject.Find("Main Camera");
        if (cam == null) { Criterion.globalLastKnownError = "Could not find 'Main Camera'."; return false; }

        return true;
    }
    public bool G2_Follow()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        if (vcam.Follow == null || !vcam.Follow.Equals(player.transform)) { Criterion.globalLastKnownError = "'Virtual Camera' should Follow 'Player'."; return false; }
        return true;
    }
    public bool G2_Zoom()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        if (vcam.m_Lens.OrthographicSize != 4) { Criterion.globalLastKnownError = "'Virtual Camera' Orthographic Size must be 4."; return false; }
        return true;
    }
    public bool G3_DollyPointPosition()
    {
        var dolly = CommonTutorialCallbacks.GameObjectComponent<CinemachineSmoothPath>("Dolly Track");
        if (dolly == null) { Criterion.globalLastKnownError = "Could not find 'Dolly Track' GameObject with CinemachineSmoothPath."; return false; }

        if (dolly.transform.position != Vector3.zero) { Criterion.globalLastKnownError = "'Dolly Track' Position should be (0, 0, 0)."; return false; }
        return true;
    }
    public bool G3_DollyPoint1()
    {
        var dolly = CommonTutorialCallbacks.GameObjectComponent<CinemachineSmoothPath>("Dolly Track");
        if (dolly == null) { Criterion.globalLastKnownError = "Could not find 'Dolly Track'."; return false; }

        var points = dolly.m_Waypoints;
        if (points.Length < 1) { Criterion.globalLastKnownError = "Dolly Track missing valid Waypoints."; return false; }

        if (!points[0].position.Equals(new Vector3(-5, 2, -10))) { Criterion.globalLastKnownError = "Waypoint 0 Position should be (-5, 2, -10)."; return false; }
        return true;
    }
    public bool G3_DollyPoint2()
    {
        var dolly = CommonTutorialCallbacks.GameObjectComponent<CinemachineSmoothPath>("Dolly Track");
        if (dolly == null) { Criterion.globalLastKnownError = "Could not find 'Dolly Track'."; return false; }

        var points = dolly.m_Waypoints;
        if (points.Length < 2) { Criterion.globalLastKnownError = "Dolly Track missing Waypoint 1."; return false; }

        if (!points[1].position.Equals(new Vector3(5, 4, -10))) { Criterion.globalLastKnownError = "Waypoint 1 Position should be (5, 4, -10)."; return false; }
        return true;
    }
    public bool G3_DollyPoint3Plus()
    {
        var dolly = CommonTutorialCallbacks.GameObjectComponent<CinemachineSmoothPath>("Dolly Track");
        if (dolly == null) { Criterion.globalLastKnownError = "Could not find 'Dolly Track'."; return false; }

        var points = dolly.m_Waypoints;
        if (points.Length < 5) { Criterion.globalLastKnownError = "Dolly Track needs at least 5 Waypoints."; return false; }

        //check unique
        var stored = new System.Collections.Generic.List<Vector3>();
        foreach (var point in points)
        {
            var pos = point.position;
            if (stored.Any(p => p.Equals(pos))) { Criterion.globalLastKnownError = "Dolly Track Waypoints must be unique positions."; return false; }
            stored.Add(pos);
        }
        return true;
    }
    public bool G4_AutoDolly()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        if (dolly == null) { Criterion.globalLastKnownError = "'Virtual Camera' missing Tracked Dolly component."; return false; }

        if (!dolly.m_AutoDolly.m_Enabled) { Criterion.globalLastKnownError = "'Auto Dolly' should be enabled."; return false; }
        return true;
    }
    public bool H1_Vcam()
    {
        var vcam = GetVcam();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'Virtual Camera'."; return false; }

        var cam = GameObject.Find("Main Camera");
        if (cam == null) { Criterion.globalLastKnownError = "Could not find 'Main Camera'."; return false; }

        return true;
    }
    public bool H2_Balls()
    {
        var targetGroup = CommonTutorialCallbacks.GameObjectComponent<CinemachineTargetGroup>("Target Group");
        if (targetGroup == null) { Criterion.globalLastKnownError = "Could not find 'Target Group' GameObject with CinemachineTargetGroup."; return false; }

        var balls = CommonTutorialCallbacks.GameObjectsStartingWith("Ball");
        if (balls.Count == 0) { Criterion.globalLastKnownError = "No 'Ball' GameObjects found in scene."; return false; }

        foreach(var ball in balls)
        {
            if (targetGroup.FindMember(ball.transform) == -1) { Criterion.globalLastKnownError = $"Ball '{ball.name}' is not assigned to the Target Group."; return false; }
        }

        return true;
    }

    //exercise
    public bool Exercise_RenderOrder()
    {
        var player = CommonTutorialCallbacks.GameObjectComponent<SpriteRenderer>("Player");
        if (player == null) { Criterion.globalLastKnownError = "Player missing SpriteRenderer."; return false; }

        var chest = CommonTutorialCallbacks.GameObjectComponent<SpriteRenderer>("Chest");
        if (chest == null) { Criterion.globalLastKnownError = "Chest missing SpriteRenderer."; return false; }

        if (player.sortingOrder <= chest.sortingOrder) { Criterion.globalLastKnownError = "Player Sorting Order must be higher than Chest Sorting Order."; return false; }
        return true;
    }
    static CinemachineVirtualCamera GetVcam2()
    {
        return CommonTutorialCallbacks.GameObjectComponent<CinemachineVirtualCamera>("ZoomedCamera");
    }
    public bool Exercise_Vcam2()
    {
        var vcam = GetVcam2();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'ZoomedCamera'."; return false; }

        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player'."; return false; }

        if (vcam.Follow == null || !vcam.Follow.Equals(player.transform)) { Criterion.globalLastKnownError = "'ZoomedCamera' should follow 'Player'."; return false; }
        if (vcam.m_Lens.OrthographicSize != 2) { Criterion.globalLastKnownError = "'ZoomedCamera' Ortho Size should be 2."; return false; }
        if (vcam.m_Lens.Dutch != 180) { Criterion.globalLastKnownError = "'ZoomedCamera' Dutch should be 180."; return false; }
            //vcam.Priority.Equals(0);
        return true;
    }
    public bool Exercise_LinkVcam2()
    {
        var vcam = GetVcam2();
        if (vcam == null) { Criterion.globalLastKnownError = "Could not find 'ZoomedCamera'."; return false; }

        var script = CommonTutorialCallbacks.GameObjectComponent<CharacterZoomZoneScript>("CharacterZoomZone");
        if (script == null) { Criterion.globalLastKnownError = "Could not find 'CharacterZoomZone' GameObject/Script."; return false; }

        if (script.virtualCameraToActivate == null || !script.virtualCameraToActivate.Equals(vcam)) { Criterion.globalLastKnownError = "'CharacterZoomZone' script should trigger 'ZoomedCamera'."; return false; }
        return true;
    }
}

#endif