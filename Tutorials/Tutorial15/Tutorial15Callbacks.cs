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
        if (background == null) { Criterion.globalLastKnownError = "Could not find 'Background' GameObject with TilemapRenderer."; return false; }

        if (background.layer != LayerMask.NameToLayer("Walls")) { Criterion.globalLastKnownError = "'Background' layer must be 'Walls'."; return false; }
        return true;
    }
    public bool A2_PlayerLayer()
    {
        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        if (player.layer != LayerMask.NameToLayer("Player")) { Criterion.globalLastKnownError = "'Player' layer must be 'Player'."; return false; }
        return true;
    }
    public bool A4_DisableSpawner()
    {
        var spawner = GetAllObjectsInScene().FirstOrDefault(go => go.name.StartsWith("Spawner"));
        if (spawner == null) { Criterion.globalLastKnownError = "Could not find 'Spawner' GameObject."; return false; }

        if (spawner.activeInHierarchy) { Criterion.globalLastKnownError = "Disable the 'Spawner' GameObject."; return false; }
        return true;
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
        if (!CommonTutorialCallbacks.GameObjectContainsScript<RaycastAtMouseOnClick>("Main Camera")) { Criterion.globalLastKnownError = "'Main Camera' is missing 'RaycastAtMouseOnClick' script."; return false; }
        return true;
    }

    //c
    public bool C2_LayerMask()
    {
        var layerMask = Tutorial12Callbacks.GetValueOfFieldOnComponentByName<LayerMask>("Main Camera", "RaycastAtMouseOnClick", "layerMask");
        //this would check if layer is in there, but we want ONLY this layer for this step Debug.Log(layerMask == (layerMask | (1 << LayerMask.NameToLayer("Enemies"))));
        //Debug.Log(layerMask == (1 << LayerMask.NameToLayer("Enemies")));
        if (layerMask != (1 << LayerMask.NameToLayer("Enemies"))) { Criterion.globalLastKnownError = "'RaycastAtMouseOnClick' LayerMask must be 'Enemies' only."; return false; }
        return true;
    }

    //d
    public bool D1_RaycastToMouseScript()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<RaycastToMouse>("Player");
        if (raycastToMouse == null) { Criterion.globalLastKnownError = "'Player' is missing 'RaycastToMouse' script."; return false; }

        var layerMask = raycastToMouse.layerMask;
        //Debug.Log(layerMask.value+" "+ (((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls"))))+" "+(layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))));
        if (layerMask != ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))) { Criterion.globalLastKnownError = "'RaycastToMouse' LayerMask must be 'Enemies' and 'Walls'."; return false; }
        return true;
    }

    //e
    public bool E1_RaycastCircleScript()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<RaycastWithinCircle>("Player");
        if (raycastToMouse == null) { Criterion.globalLastKnownError = "'Player' is missing 'RaycastWithinCircle' script."; return false; }

        var layerMask = raycastToMouse.layerMask;
        //Debug.Log(layerMask.value+" "+ (((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls"))))+" "+(layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))));
        if (layerMask != ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))) { Criterion.globalLastKnownError = "'RaycastWithinCircle' LayerMask must be 'Enemies' and 'Walls'."; return false; }
        return true;
    }
    //f
    public bool F1_RaycastReflectionScript()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<RaycastWithReflection>("Player");
        if (raycastToMouse == null) { Criterion.globalLastKnownError = "'Player' is missing 'RaycastWithReflection' script."; return false; }

        var layerMask = raycastToMouse.layerMask;
        //Debug.Log(layerMask.value+" "+ (((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls"))))+" "+(layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))));
        if (layerMask != ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))) { Criterion.globalLastKnownError = "'RaycastWithReflection' LayerMask must be 'Enemies' and 'Walls'."; return false; }
        return true;
    }
    public bool F1_DisableRaycastToMouse()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<RaycastToMouse>("Player");
        if (raycastToMouse == null) { Criterion.globalLastKnownError = "'Player' is missing 'RaycastToMouse' script."; return false; }

        if (raycastToMouse.enabled) { Criterion.globalLastKnownError = "Disable the 'RaycastToMouse' script on 'Player'."; return false; }
        return true;
    }

    //g
    public bool G1_StartEnd()
    {
        var tripwire = CommonTutorialCallbacks.GameObjectComponent<TripWire>("TripwireStart");
        if (tripwire == null) { Criterion.globalLastKnownError = "Could not find 'TripwireStart' with 'TripWire' script."; return false; }

        var end = GameObject.Find("TripwireEnd");
        if (end == null) { Criterion.globalLastKnownError = "Could not find 'TripwireEnd' GameObject."; return false; }

        if (tripwire.end != end.transform) { Criterion.globalLastKnownError = "Assign 'TripwireEnd' to 'TripWire' End field."; return false; }
        return true;
    }
    public bool G1_TripwireMask()
    {
        var raycastToMouse = CommonTutorialCallbacks.GameObjectComponent<TripWire>("TripwireStart");
        if (raycastToMouse == null) { Criterion.globalLastKnownError = "Could not find 'TripwireStart' with 'TripWire' script."; return false; }

        var layerMask = raycastToMouse.layerMask;
        //Debug.Log(layerMask.value+" "+ (((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls"))))+" "+(layerMask == ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Walls")))));
        if (layerMask != ((1 << LayerMask.NameToLayer("Enemies")) | (1 << LayerMask.NameToLayer("Player")))) { Criterion.globalLastKnownError = "'TripWire' LayerMask must be 'Enemies' and 'Player'."; return false; }
        return true;
    }
    public bool G2_BoxCollider2D()
    {
        if (CommonTutorialCallbacks.GameObjectComponent<BoxCollider2D>("SecretWall") == null) { Criterion.globalLastKnownError = "'SecretWall' is missing 'BoxCollider2D'."; return false; }
        return true;
    }

    public bool G3_BrokenEvent()
    {
        var e = Tutorial12Callbacks.GetValueOfFieldOnComponentByName<UnityEngine.Events.UnityEvent>("TripwireStart", "TripWire", "onTripWireBroken");
        if (e == null) { Criterion.globalLastKnownError = "Could not find 'TripWire' script or 'onTripWireBroken' event."; return false; }
        if (e.GetPersistentEventCount() != 1) { Criterion.globalLastKnownError = "'onTripWireBroken' should have 1 persistent listener."; return false; }

        var target = e.GetPersistentTarget(0);
        if (target == null) { Criterion.globalLastKnownError = "'onTripWireBroken' target is null."; return false; }

        var secretWall = GameObject.Find("SecretWall");
        if (secretWall == null) { Criterion.globalLastKnownError = "Could not find 'SecretWall'."; return false; }

        if (target != secretWall) { Criterion.globalLastKnownError = "'onTripWireBroken' target must be 'SecretWall'."; return false; }
        return true;
    }
    public bool G3_BrokenEventFunction()
    {
        var e = Tutorial12Callbacks.GetValueOfFieldOnComponentByName<UnityEngine.Events.UnityEvent>("TripwireStart", "TripWire", "onTripWireBroken");
        if (e == null) { Criterion.globalLastKnownError = "Could not find 'TripWire' script or 'onTripWireBroken' event."; return false; }
        if (e.GetPersistentEventCount() != 1) { Criterion.globalLastKnownError = "'onTripWireBroken' should have 1 persistent listener."; return false; }

        if (e.GetPersistentMethodName(0) != "SetActive") { Criterion.globalLastKnownError = "'onTripWireBroken' function must be 'SetActive'."; return false; }
        return true;
    }
    public bool G3_UnBrokenEventFunction()
    {
        var e = Tutorial12Callbacks.GetValueOfFieldOnComponentByName<UnityEngine.Events.UnityEvent>("TripwireStart", "TripWire", "onTripWireUnbroken");
        if (e == null) { Criterion.globalLastKnownError = "Could not find 'TripWire' script or 'onTripWireUnbroken' event."; return false; }
        if (e.GetPersistentEventCount() != 1) { Criterion.globalLastKnownError = "'onTripWireUnbroken' should have 1 persistent listener."; return false; }

        var target = e.GetPersistentTarget(0);
        if (target == null) { Criterion.globalLastKnownError = "'onTripWireUnbroken' target is null."; return false; }

        var secretWall = GameObject.Find("SecretWall");
        if (secretWall == null) { Criterion.globalLastKnownError = "Could not find 'SecretWall'."; return false; }

        if (target != secretWall || e.GetPersistentMethodName(0) != "SetActive") { Criterion.globalLastKnownError = "'onTripWireUnbroken' target must be 'SecretWall' and function 'SetActive'."; return false; }
        return true;
    }
}
#endif