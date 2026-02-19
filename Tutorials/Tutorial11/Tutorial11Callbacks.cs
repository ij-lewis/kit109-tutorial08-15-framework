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
public class Tutorial11Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial11Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial11Callbacks>(DefaultFileName);
    }

    //b
    public bool B2_SeekScript()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<Seek>("Seeker")) { Criterion.globalLastKnownError = "'Seeker' GameObject is missing 'Seek' script."; return false; }
        return true;
    }
    public bool B2_SeekTarget()
    {
        var seekScript = CommonTutorialCallbacks.GameObjectComponent<Seek>("Seeker");
        if (seekScript == null) { Criterion.globalLastKnownError = "'Seeker' GameObject is missing 'Seek' script."; return false; }

        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        if (seekScript.target != player.transform) { Criterion.globalLastKnownError = "'Seek' script target is not set to 'Player'."; return false; }
        return true;
    }

    //c
    public bool C2_FleeScript()
    {
        var fleer = GameObject.Find("Fleer");
        if (fleer == null) { Criterion.globalLastKnownError = "Could not find 'Fleer' GameObject."; return false; }

        //have to use string get component here because flee doesnt exist in base proj
        var fleeScript = fleer.GetComponent("Flee");
        if (fleeScript == null) { Criterion.globalLastKnownError = "'Fleer' GameObject is missing 'Flee' script."; return false; }
        return true;
    }
    public bool C4_FleeInherit()
    {
        var fleer = CommonTutorialCallbacks.GetPrefab("AI/Fleer");
        if (fleer == null) { Criterion.globalLastKnownError = "Could not find 'Fleer' prefab in 'Assets/AI'."; return false; }

        var fleeScript = fleer.GetComponent("Flee");
        if (fleeScript == null) { Criterion.globalLastKnownError = "'Fleer' prefab is missing 'Flee' script."; return false; }

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var fleeType = fleeScript.GetType();
        if (fleeType.BaseType != typeof(Mover)) { Criterion.globalLastKnownError = "'Flee' script should inherit from 'Mover'."; return false; }
        return true;
    }
    public bool C6_FleeTarget()
    {
        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        var fleer = GameObject.Find("Fleer");
        if (fleer == null) { Criterion.globalLastKnownError = "Could not find 'Fleer' GameObject."; return false; }

        var fleeScript = fleer.GetComponent("Flee");
        if (fleeScript == null) { Criterion.globalLastKnownError = "'Fleer' GameObject is missing 'Flee' script."; return false; }

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var fleeType = fleeScript.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = fleeType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) { Criterion.globalLastKnownError = "'Flee' script is missing public 'target' field."; return false; }

        if (targetField.FieldType != typeof(Transform) || (object)targetField.GetValue(fleeScript) != player.transform)
        {
             Criterion.globalLastKnownError = "'Flee' script target should be type 'Transform' and set to 'Player'.";
             return false;
        }
        return true;
    }

    //d
    public bool D2_PursuitScript()
    {
        var pursuer = GameObject.Find("Pursuer");
        if (pursuer == null) { Criterion.globalLastKnownError = "Could not find 'Pursuer' GameObject."; return false; }

        //have to use string get component here because flee doesnt exist in base proj
        var pursuitScript = pursuer.GetComponent("Pursuit");
        if (pursuitScript == null) { Criterion.globalLastKnownError = "'Pursuer' GameObject is missing 'Pursuit' script."; return false; }
        return true;
    }
    public bool D3_PursuitVars()
    {
        var pursuer = GameObject.Find("Pursuer");
        if (pursuer == null) { Criterion.globalLastKnownError = "Could not find 'Pursuer' GameObject."; return false; }

        var pursuitScript = pursuer.GetComponent("Pursuit");
        if (pursuitScript == null) { Criterion.globalLastKnownError = "'Pursuer' GameObject is missing 'Pursuit' script."; return false; }

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var pursuitType = pursuitScript.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = pursuitType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) { Criterion.globalLastKnownError = "'Pursuit' script is missing 'target' field."; return false; }

        var fixedUpdateFunc = pursuitType.GetMethod("FixedUpdate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (fixedUpdateFunc == null) { Criterion.globalLastKnownError = "'Pursuit' script is missing 'FixedUpdate' method."; return false; }

        if (!((targetField.FieldType == typeof(Transform) || targetField.FieldType == typeof(Rigidbody2D) || targetField.FieldType == typeof(Mover)) && fixedUpdateFunc.ReturnType == typeof(void)))
        {
             Criterion.globalLastKnownError = "'Pursuit' targets should be Transform/Rigidbody2D/Mover and FixedUpdate should return void.";
             return false;
        }
        return true;
    }
    public bool D5_PursuitTargetVars()
    {
        var playerRB = CommonTutorialCallbacks.GameObjectComponent<Rigidbody2D>("Player");
        if (playerRB == null) { Criterion.globalLastKnownError = "Player GameObject is missing Rigidbody2D."; return false; }

        var pursuer = GameObject.Find("Pursuer");
        if (pursuer == null) { Criterion.globalLastKnownError = "Could not find 'Pursuer' GameObject."; return false; }

        var pursuitScript = pursuer.GetComponent("Pursuit");
        if (pursuitScript == null) { Criterion.globalLastKnownError = "'Pursuer' GameObject is missing 'Pursuit' script."; return false; }

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var pursuitType = pursuitScript.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = pursuitType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) { Criterion.globalLastKnownError = "'Pursuit' script is missing 'target' field."; return false; }

        if (targetField.FieldType != typeof(Rigidbody2D) || (object)targetField.GetValue(pursuitScript) != playerRB)
        {
             Criterion.globalLastKnownError = "'Pursuit' script target should be type 'Rigidbody2D' and set to 'Player'.";
             return false;
        }
        return true;
    }
    public bool D6_PredictionTime()
    {
        var pursuer = GameObject.Find("Pursuer");
        if (pursuer == null) { Criterion.globalLastKnownError = "Could not find 'Pursuer' GameObject."; return false; }

        var pursuitScript = pursuer.GetComponent("Pursuit");
        if (pursuitScript == null) { Criterion.globalLastKnownError = "'Pursuer' GameObject is missing 'Pursuit' script."; return false; }

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var pursuitType = pursuitScript.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var predictionField = pursuitType.GetField("predictionTime", BindingFlags.Instance | BindingFlags.Public);
        if (predictionField == null) { Criterion.globalLastKnownError = "'Pursuit' script is missing public float 'predictionTime'."; return false; }

        if (predictionField.FieldType != typeof(float)) { Criterion.globalLastKnownError = "'predictionTime' must be a float."; return false; }
        return true;
    }

    //e
    public bool E1_WandererScript()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<Wandering>("Wanderer")) { Criterion.globalLastKnownError = "'Wanderer' GameObject is missing 'Wandering' script."; return false; }
        return true;
    }

    //f
    const string ComplexAlmostHumanLikeAI = "ComplexAlmostHumanLikeAI";
    public bool F1_Pursuit()
    {
        var playerRB = CommonTutorialCallbacks.GameObjectComponent<Rigidbody2D>("Player");
        if (playerRB == null) { Criterion.globalLastKnownError = "Player GameObject is missing Rigidbody2D."; return false; }

        var script = CommonTutorialCallbacks.GameObjectComponentByName("Pursuit", ComplexAlmostHumanLikeAI);
        if (script == null) { Criterion.globalLastKnownError = $"'ComplexAlmostHumanLikeAI' is missing 'Pursuit' script."; return false; }

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var scriptType = script.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = scriptType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) { Criterion.globalLastKnownError = "'Pursuit' script is missing 'target' field."; return false; }

        if (targetField.FieldType != typeof(Rigidbody2D) || (object)targetField.GetValue(script) != playerRB)
        {
             Criterion.globalLastKnownError = "'Pursuit' target should be 'Player' (Rigidbody2D).";
             return false;
        }
        return true;
    }
    public bool F1_Flee()
    {
        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find 'Player' GameObject."; return false; }

        var script = CommonTutorialCallbacks.GameObjectComponentByName("Flee", ComplexAlmostHumanLikeAI);
        if (script == null) { Criterion.globalLastKnownError = $"'ComplexAlmostHumanLikeAI' is missing 'Flee' script."; return false; }

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var scriptType = script.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = scriptType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) { Criterion.globalLastKnownError = "'Flee' script is missing 'target' field."; return false; }

        if (targetField.FieldType != typeof(Transform) || (object)targetField.GetValue(script) != player.transform)
        {
             Criterion.globalLastKnownError = "'Flee' target should be 'Player' (Transform).";
             return false;
        }
        return true;
    }
    public bool F1_Wander()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScriptByName("Wandering", ComplexAlmostHumanLikeAI)) { Criterion.globalLastKnownError = $"'ComplexAlmostHumanLikeAI' is missing 'Wandering' script."; return false; }
        return true;
    }
    public bool F1_AllThreeDisabled()
    {

        bool p = F1_Pursuit(); // checks existence and target
        bool f = F1_Flee();
        bool w = F1_Wander();
        if (!p || !f || !w) return false; // Error already set by sub-calls if false
        
        var compP = CommonTutorialCallbacks.GameObjectComponentByName("Pursuit", ComplexAlmostHumanLikeAI);
        var compF = CommonTutorialCallbacks.GameObjectComponentByName("Flee", ComplexAlmostHumanLikeAI);
        var compW = CommonTutorialCallbacks.GameObjectComponentByName("Wandering", ComplexAlmostHumanLikeAI);

        if (compP.enabled || compF.enabled || compW.enabled)
        {
             Criterion.globalLastKnownError = "Pursuit, Flee, and Wandering scripts should be disabled (unchecked).";
             return false;
        }
        return true;
    }
    public bool F1_Animator()
    {
        if (CommonTutorialCallbacks.GameObjectComponent<Animator>(ComplexAlmostHumanLikeAI) == null) { Criterion.globalLastKnownError = $"'ComplexAlmostHumanLikeAI' is missing an Animator."; return false; }
        return true;
    }

    static RuntimeAnimatorController GetAnimatorController()
    {
        return AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/AI/ComplexAIMachine.controller");
    }
    static AnimatorState GetState(string name)
    {
        var player = GetAnimatorController();
        if (player == null) return null;

        var ac = player as AnimatorController;
        bool any = ac.layers.First().stateMachine.states.Any(s => s.state.name.Equals(name));
        if (any == false) return null;

        var state = ac.layers.First().stateMachine.states.FirstOrDefault(s => s.state.name.Equals(name));
        return state.state;
    }
    public bool F2_AnimController()
    {
        if (GetAnimatorController() == null) { Criterion.globalLastKnownError = "Could not find 'Assets/AI/ComplexAIMachine.controller'."; return false; }
        return true;
    }
    public bool F2_AnimatorLinked()
    {
        var a = CommonTutorialCallbacks.GameObjectComponent<Animator>(ComplexAlmostHumanLikeAI);
        if (a == null) { Criterion.globalLastKnownError = $"'ComplexAlmostHumanLikeAI' is missing an Animator."; return false; }

        var ac = a.runtimeAnimatorController;
        if (ac == null) { Criterion.globalLastKnownError = $"'ComplexAlmostHumanLikeAI' Animator is missing a Controller."; return false; }

        if (ac != GetAnimatorController()) { Criterion.globalLastKnownError = "Animator Controller should be 'ComplexAIMachine'."; return false; }
        return true;
    }
    public bool F2_States()
    {
        if (GetState("Wandering") == null) { Criterion.globalLastKnownError = "Missing 'Wandering' state."; return false; }
        if (GetState("Fleeing") == null) { Criterion.globalLastKnownError = "Missing 'Fleeing' state."; return false; }
        if (GetState("Pursuing") == null) { Criterion.globalLastKnownError = "Missing 'Pursuing' state."; return false; }
        if (GetState("Stop and Cry") == null) { Criterion.globalLastKnownError = "Missing 'Stop and Cry' state."; return false; }
        return true;
    }
    public bool F2_Transitions()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) { Criterion.globalLastKnownError = "One or more states are missing."; return false; }

        if (!wandering.transitions.Any(t => t.destinationState == pursuing)) { Criterion.globalLastKnownError = "Missing transition: Wandering -> Pursuing."; return false; }
        if (!wandering.transitions.Any(t => t.destinationState == fleeing)) { Criterion.globalLastKnownError = "Missing transition: Wandering -> Fleeing."; return false; }
        if (!fleeing.transitions.Any(t => t.destinationState == stopAndCry)) { Criterion.globalLastKnownError = "Missing transition: Fleeing -> Stop and Cry."; return false; }
        if (!pursuing.transitions.Any(t => t.destinationState == stopAndCry)) { Criterion.globalLastKnownError = "Missing transition: Pursuing -> Stop and Cry."; return false; }
        if (!stopAndCry.transitions.Any(t => t.destinationState == wandering)) { Criterion.globalLastKnownError = "Missing transition: Stop and Cry -> Wandering."; return false; }
        return true;
    }
    public bool F2_ExitTime()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        //var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null) { Criterion.globalLastKnownError = "One or more states missing."; return false; }

        if (!wandering.transitions.All(t => t.hasExitTime == false && t.duration == 0)) { Criterion.globalLastKnownError = "Wandering transitions should have Exit Time unchecked and Duration 0."; return false; }
        if (!fleeing.transitions.All(t => t.hasExitTime == false && t.duration == 0)) { Criterion.globalLastKnownError = "Fleeing transitions should have Exit Time unchecked and Duration 0."; return false; }
        if (!pursuing.transitions.All(t => t.hasExitTime == false && t.duration == 0)) { Criterion.globalLastKnownError = "Pursuing transitions should have Exit Time unchecked and Duration 0."; return false; }
        return true;
    }
    public bool F2_ExitTimeStopAndCry()
    {
        var stopAndCry = GetState("Stop and Cry");
        if (stopAndCry == null) { Criterion.globalLastKnownError = "Missing 'Stop and Cry' state."; return false; }

        if (!stopAndCry.transitions.Any(t => t.duration == 1)) { Criterion.globalLastKnownError = "'Stop and Cry' transitions should have Duration 1."; return false; }
        return true;
    }

    public bool F3_PlayerIsNear()
    {
        var controller = GetAnimatorController();
        if (controller == null) { Criterion.globalLastKnownError = "Animator Controller not found."; return false; }

        var ac = controller as AnimatorController;
        if (!ac.parameters.Any(p => p.name.Equals("PlayerIsNear") && p.type == AnimatorControllerParameterType.Bool)) { Criterion.globalLastKnownError = "Missing Bool Parameter 'PlayerIsNear'."; return false; }
        return true;
    }
    public bool F3_IsScaredOfPlayer()
    {
        var controller = GetAnimatorController();
        if (controller == null) { Criterion.globalLastKnownError = "Animator Controller not found."; return false; }

        var ac = controller as AnimatorController;
        if (!ac.parameters.Any(p => p.name.Equals("IsScaredOfPlayer") && p.type == AnimatorControllerParameterType.Bool)) { Criterion.globalLastKnownError = "Missing Bool Parameter 'IsScaredOfPlayer'."; return false; }
        return true;
    }
    public bool F3_WanderingToPursuing()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        bool hasCorrectTransition = wandering.transitions.Any(t => t.destinationState == pursuing &&
                    t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.If) &&
                    t.conditions.Any(c => c.parameter == "IsScaredOfPlayer" && c.mode == AnimatorConditionMode.IfNot));
        
        if (!hasCorrectTransition) { Criterion.globalLastKnownError = "Wandering -> Pursuing transition incorrect. Conditions: PlayerIsNear=true, IsScaredOfPlayer=false."; return false; }
        return true;
    }
    public bool F3_WanderingToFleeing()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        bool hasCorrectTransition = wandering.transitions.Any(t => t.destinationState == fleeing &&
                    t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.If) &&
                    t.conditions.Any(c => c.parameter == "IsScaredOfPlayer" && c.mode == AnimatorConditionMode.If));

        if (!hasCorrectTransition) { Criterion.globalLastKnownError = "Wandering -> Fleeing transition incorrect. Conditions: PlayerIsNear=true, IsScaredOfPlayer=true."; return false; }
        return true;
    }
    public bool F3_PursuingToStopAndCry()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        bool hasCorrectTransition = pursuing.transitions.Any(t => t.destinationState == stopAndCry &&
                    t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.IfNot));

        if (!hasCorrectTransition) { Criterion.globalLastKnownError = "Pursuing -> Stop and Cry transition incorrect. Conditions: PlayerIsNear=false."; return false; }
        return true;
    }
    public bool F3_FleeingToStopAndCry()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        bool hasCorrectTransition = fleeing.transitions.Any(t => t.destinationState == stopAndCry &&
                    t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.IfNot));
        
        if (!hasCorrectTransition) { Criterion.globalLastKnownError = "Fleeing -> Stop and Cry transition incorrect. Conditions: PlayerIsNear=false."; return false; }
        return true;
    }

    static AnimationClip GetClip(string name)
    {
        return AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/AI/" + name + ".anim");
    }
    public bool F5_WanderingClip()
    {
        if (GetClip("WanderingClip") == null) { Criterion.globalLastKnownError = "Could not find 'Assets/AI/WanderingClip.anim'."; return false; }
        return true;
    }
    public bool F5_PursuingClip()
    {
        if (GetClip("PursuingClip") == null) { Criterion.globalLastKnownError = "Could not find 'Assets/AI/PursuingClip.anim'."; return false; }
        return true;
    }
    public bool F5_FleeingClip()
    {
        if (GetClip("FleeingClip") == null) { Criterion.globalLastKnownError = "Could not find 'Assets/AI/FleeingClip.anim'."; return false; }
        return true;
    }
    static bool CheckRecord(AnimationClip clip, EditorCurveBinding[] bindings, string type, bool value)
    {
        var binding = bindings.FirstOrDefault(b => b.propertyName == "m_Enabled" && b.type.Name == type);
        if (binding == null) return false;
        var wanderingCurve = AnimationUtility.GetEditorCurve(clip, binding);
        if (wanderingCurve == null || wanderingCurve.keys.Count() != 1) return false;
        if (value)
            return wanderingCurve.keys[0].value == 1;
        else
            return wanderingCurve.keys[0].value == 0;
    }
    public bool F5_WanderingClipRecord()
    {
        var clip = GetClip("WanderingClip");
        if (clip == null) return false;

        var bindings = AnimationUtility.GetCurveBindings(clip);
        if (bindings.Count() != 3) return false;

        //wandering = enabled
        //wandering = enabled
        bool correct = CheckRecord(clip, bindings, "Wandering", true) &&
            CheckRecord(clip, bindings, "Flee", false) &&
            CheckRecord(clip, bindings, "Pursuit", false);
        
        if (!correct) { Criterion.globalLastKnownError = "'WanderingClip' should enable Wandering and disable Flee/Pursuit."; return false; }
        return true;
    }
    public bool F5_PursuingClipRecord()
    {
        var clip = GetClip("PursuingClip");
        if (clip == null) return false;

        var bindings = AnimationUtility.GetCurveBindings(clip);
        if (bindings.Count() != 3) return false;

        //pursuit = enabled
        //pursuit = enabled
        bool correct = CheckRecord(clip, bindings, "Wandering", false) &&
            CheckRecord(clip, bindings, "Flee", false) &&
            CheckRecord(clip, bindings, "Pursuit", true);

        if (!correct) { Criterion.globalLastKnownError = "'PursuingClip' should enable Pursuit and disable Flee/Wandering."; return false; }
        return true;
    }
    public bool F5_FleeingClipRecord()
    {
        var clip = GetClip("FleeingClip");
        if (clip == null) return false;

        var bindings = AnimationUtility.GetCurveBindings(clip);
        if (bindings.Count() != 3) return false;

        //flee = enabled
        //flee = enabled
        bool correct = CheckRecord(clip, bindings, "Wandering", false) &&
            CheckRecord(clip, bindings, "Flee", true) &&
            CheckRecord(clip, bindings, "Pursuit", false);

        if (!correct) { Criterion.globalLastKnownError = "'FleeingClip' should enable Flee and disable Wandering/Pursuit."; return false; }
        return true;
    }
    public bool F6_WanderingClipState()
    {
        var state = GetState("Wandering");
        if (state == null) return false;

        var clip = GetClip("WanderingClip");
        if (clip == null) return false;

        if (state.motion != clip) { Criterion.globalLastKnownError = "'Wandering' state Motion should be 'WanderingClip'."; return false; }
        return true;
    }
    public bool F6_PursuingClipState()
    {
        var state = GetState("Pursuing");
        if (state == null) return false;

        var clip = GetClip("PursuingClip");
        if (clip == null) return false;

        if (state.motion != clip) { Criterion.globalLastKnownError = "'Pursuing' state Motion should be 'PursuingClip'."; return false; }
        return true;
    }
    public bool F6_FleeingClipState()
    {
        var state = GetState("Fleeing");
        if (state == null) return false;

        var clip = GetClip("FleeingClip");
        if (clip == null) return false;

        if (state.motion != clip) { Criterion.globalLastKnownError = "'Fleeing' state Motion should be 'FleeingClip'."; return false; }
        return true;
    }

    static Transform GetRadius()
    {
        var player = GameObject.Find("Player");
        if (player == null) return null;

        return player.transform.Find("Radius");
    }
    public bool F8_Radius()
    {
        if (GetRadius() == null) { Criterion.globalLastKnownError = "'Player' is missing a child GameObject named 'Radius'."; return false; }
        return true;
    }
    public bool F8_Radius_Collider()
    {
        var radius = GetRadius();
        if (radius == null) { Criterion.globalLastKnownError = "Radius object not found."; return false; }

        var circle = radius.GetComponent<CircleCollider2D>();
        if (circle == null) { Criterion.globalLastKnownError = "'Radius' object is missing CircleCollider2D."; return false; }

        if (!circle.isTrigger) { Criterion.globalLastKnownError = "'Radius' CircleCollider2D 'Is Trigger' should be checked."; return false; }
        if (circle.radius < 2.5f) { Criterion.globalLastKnownError = "'Radius' CircleCollider2D Radius should be >= 2.5."; return false; }
        return true;
    }
    public bool F8_Radius_TriggerScript()
    {
        var radius = GetRadius();
        if (radius == null) { Criterion.globalLastKnownError = "Radius object not found."; return false; }

        var triggerScript = radius.GetComponent<SetAnimatorBooleanOnTriggerStay>();
        if (triggerScript == null) { Criterion.globalLastKnownError = "'Radius' object is missing 'SetAnimatorBooleanOnTriggerStay' script."; return false; }

        if (triggerScript.booleanName != "PlayerIsNear") { Criterion.globalLastKnownError = "'SetAnimatorBooleanOnTriggerStay' Boolean Name should be 'PlayerIsNear'."; return false; }
        if (!triggerScript.applyToOther) { Criterion.globalLastKnownError = "'SetAnimatorBooleanOnTriggerStay' 'Apply To Other' should be checked."; return false; }
        return true;
    }
    public bool F8_Radius_Layer()
    {
        var radius = GetRadius();
        if (radius == null) return false;

        if (radius.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast")) { Criterion.globalLastKnownError = "'Radius' GameObject Layer should be 'Ignore Raycast'."; return false; }
        return true;
    }
    public bool F9_ScaredOfPlayer()
    {
        var triggerScript = CommonTutorialCallbacks.GameObjectComponent<SetAnimatorBooleanOnInput>(ComplexAlmostHumanLikeAI);
        if (triggerScript == null) { Criterion.globalLastKnownError = $"'ComplexAlmostHumanLikeAI' is missing 'SetAnimatorBooleanOnInput' script."; return false; }

        if (triggerScript.animatorBoolean != "IsScaredOfPlayer") { Criterion.globalLastKnownError = "'SetAnimatorBooleanOnInput' Animator Boolean should be 'IsScaredOfPlayer'."; return false; }
        if (triggerScript.key != KeyCode.Space) { Criterion.globalLastKnownError = "'SetAnimatorBooleanOnInput' Key should be 'Space'."; return false; }
        return true;
    }
    public bool F10_MultipleAI()
    {
        var objs = CommonTutorialCallbacks.GameObjectsStartingWith(ComplexAlmostHumanLikeAI);
        if (objs.Count < 3) { Criterion.globalLastKnownError = $"Create at least 3 '{ComplexAlmostHumanLikeAI}' GameObjects."; return false; }
        if (!CommonTutorialCallbacks.ObjectsInDifferentLocations(objs)) { Criterion.globalLastKnownError = "GameObjects must be in different locations."; return false; }
        return true;
    }

    //g

    public bool G2_MultipleNodes()
    {
        var objs = CommonTutorialCallbacks.GameObjectsStartingWith("Node");
        if (objs.Count < 6) { Criterion.globalLastKnownError = "Create at least 6 'Node' GameObjects (prefabs)."; return false; }
        if (!CommonTutorialCallbacks.ObjectsInDifferentLocations(objs)) { Criterion.globalLastKnownError = "Nodes must be in different locations."; return false; }
        return true;
    }
    public bool G3_Connections()
    {
        var objs = CommonTutorialCallbacks.GameObjectsStartingWith("Node");
        foreach (var node in objs)
        {
            var nodeScript = node.GetComponent<PathNode>();
            if (nodeScript == null) continue; //probably just the node generate or "nodes" parent

            if (nodeScript.connections == null || nodeScript.connections.Count == 0)
            {
                //check to see if any other nodes are connected to us
                bool connectedToUs = false;
                foreach (var node2 in objs)
                {
                    var nodeScript2 = node2.GetComponent<PathNode>();
                    if (nodeScript2 == null) continue; //probably just the node generate or "nodes" parent

                    if (nodeScript2.connections != null && nodeScript2.connections.Contains(nodeScript))
                    {
                        connectedToUs = true;
                        break;
                    }
                }
                if (connectedToUs == false)
                {
                    Criterion.globalLastKnownError = $"Node '{node.name}' is not connected to any other node (incoming or outgoing).";
                    return false;
                }
            }
        }
        return true;
    }
}
#endif