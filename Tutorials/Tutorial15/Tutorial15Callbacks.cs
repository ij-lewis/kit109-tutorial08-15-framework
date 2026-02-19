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
using System.Collections.Generic;

/// <summary>
/// Implement your Tutorial callbacks here.
/// </summary>
[CreateAssetMenu(fileName = DefaultFileName, menuName = "Tutorials/" + DefaultFileName + " Instance")]
public class Tutorial15Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial15Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial15Callbacks>(DefaultFileName);
    }

    //a
    public bool A2_WallsLayer()
    {
        //var background = GameObject.Find("Background");
        var background = CommonTutorialCallbacks.GameObjectFindWithComponent<UnityEngine.Tilemaps.TilemapRenderer>("Background");
        if (background == null) return false;

        return background.layer.Equals(LayerMask.NameToLayer("Walls"));
    }
    public bool A2_PlayerLayer()
    {
        var background = GameObject.Find("Player");
        if (background == null) return false;

        return background.layer.Equals(LayerMask.NameToLayer("Player"));
    }
    public bool A4_DisableSpawner()
    {
        var spawner = GetAllObjectsInScene().FirstOrDefault(go => go.name.StartsWith("Spawner"));
        if (spawner == null) return false;

        return spawner.activeInHierarchy == false;
    }
    //https://answers.unity.com/questions/890636/find-an-inactive-game-object.html
    private static List<GameObject> GetAllObjectsInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.hideFlags != HideFlags.None)
                continue;
            if (PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.NotAPrefab)
                continue;
            objectsInScene.Add(go);
        }
        return objectsInScene;
    }

    //b
    public bool B3_RaycastMouseScript()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<RaycastAtMouseOnClick>("Main Camera");
    }

    //c
    public bool C2_LayerMask()
    {
        var layerMask = Tutorial12Callbacks.GetValueOfFieldOnComponentByName<LayerMask>("Main Camera", "RaycastAtMouseOnClick", "layerMask");
        //this would check if layer is in there, but we want ONLY this layer for this step Debug.Log(layerMask == (layerMask | (1 << LayerMask.NameToLayer("Enemies"))));
        //Debug.Log(layerMask == (1 << LayerMask.NameToLayer("Enemies")));
        return layerMask == (1 << LayerMask.NameToLayer("Enemies"));
    }

    //d
    public bool D1_RaycastToMouseScript()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<RaycastToMouse>("Player");
        if (raycastToMouse == null) return false;

        var layerMask = raycastToMouse.layerMask;
        //Debug.Log(layerMask.value+" "+ (((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls"))))+" "+(layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))));
        return layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")));
    }

    //e
    public bool E1_RaycastCircleScript()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<RaycastWithinCircle>("Player");
        if (raycastToMouse == null) return false;

        var layerMask = raycastToMouse.layerMask;
        //Debug.Log(layerMask.value+" "+ (((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls"))))+" "+(layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))));
        return layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")));
    }
    //f
    public bool F1_RaycastReflectionScript()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<RaycastWithReflection>("Player");
        if (raycastToMouse == null) return false;

        var layerMask = raycastToMouse.layerMask;
        //Debug.Log(layerMask.value+" "+ (((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls"))))+" "+(layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))));
        return layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")));
    }
    public bool F1_DisableRaycastToMouse()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<RaycastToMouse>("Player");
        if (raycastToMouse == null) return false;

        return raycastToMouse.enabled == false;
    }

    //g
    public bool G1_StartEnd()
    {
        var tripwire = CommonTutorialCallbacks.GameObjectComponent<TripWire>("TripwireStart");
        if (tripwire == null) return false;

        var end = GameObject.Find("TripwireEnd");
        if (end == null) return false;

        return tripwire.end == end.transform;
    }
    public bool G1_TripwireMask()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<TripWire>("TripwireStart");
        if (raycastToMouse == null) return false;

        var layerMask = raycastToMouse.layerMask;
        //Debug.Log(layerMask.value+" "+ (((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls"))))+" "+(layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))));
        return layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Player")));
    }
    public bool G2_BoxCollider2D()
    {
        return CommonTutorialCallbacks.GameObjectComponent<BoxCollider2D>("SecretWall") != null;
    }

    public bool G3_BrokenEvent()
    {
        var e = Tutorial12Callbacks.GetValueOfFieldOnComponentByName<UnityEngine.Events.UnityEvent>("TripwireStart", "TripWire", "onTripWireBroken");
        if (e == null) return false;
        if (e.GetPersistentEventCount() != 1) return false;

        var target = e.GetPersistentTarget(0);
        if (target == null) return false;

        var secretWall = GameObject.Find("SecretWall");
        if (secretWall == null) return false;

        return target == secretWall;
    }
    public bool G3_BrokenEventFunction()
    {
        var e = Tutorial12Callbacks.GetValueOfFieldOnComponentByName<UnityEngine.Events.UnityEvent>("TripwireStart", "TripWire", "onTripWireBroken");
        if (e == null) return false;
        if (e.GetPersistentEventCount() != 1) return false;

        return e.GetPersistentMethodName(0) == "SetActive";
    }
    public bool G3_UnBrokenEventFunction()
    {
        var e = Tutorial12Callbacks.GetValueOfFieldOnComponentByName<UnityEngine.Events.UnityEvent>("TripwireStart", "TripWire", "onTripWireUnbroken");
        if (e == null) return false;
        if (e.GetPersistentEventCount() != 1) return false;

        var target = e.GetPersistentTarget(0);
        if (target == null) return false;

        var secretWall = GameObject.Find("SecretWall");
        if (secretWall == null) return false;

        return target == secretWall && e.GetPersistentMethodName(0) == "SetActive";
    }
}
#endif