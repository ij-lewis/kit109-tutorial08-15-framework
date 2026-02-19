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
        return CommonTutorialCallbacks.GameObjectContainsScript<Seek>("Seeker");
    }
    public bool B2_SeekTarget()
    {
        var seekScript = CommonTutorialCallbacks.GameObjectComponent<Seek>("Seeker");
        if (seekScript == null) return false;

        var player = GameObject.Find("Player");
        if (player == null) return false;

        return seekScript.target == player.transform;
    }

    //c
    public bool C2_FleeScript()
    {
        var fleer = GameObject.Find("Fleer");
        if (fleer == null) return false;

        //have to use string get component here because flee doesnt exist in base proj
        var fleeScript = fleer.GetComponent("Flee");
        return fleeScript;
    }
    public bool C4_FleeInherit()
    {
        var fleer = CommonTutorialCallbacks.GetPrefab("AI/Fleer");
        if (fleer == null) return false;

        var fleeScript = fleer.GetComponent("Flee");
        if (fleeScript == null) return false;

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var fleeType = fleeScript.GetType();
        return fleeType.BaseType == typeof(Mover);
    }
    public bool C6_FleeTarget()
    {
        var player = GameObject.Find("Player");
        if (player == null) return false;

        var fleer = GameObject.Find("Fleer");
        if (fleer == null) return false;

        var fleeScript = fleer.GetComponent("Flee");
        if (fleeScript == null) return false;

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var fleeType = fleeScript.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = fleeType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) return false;

        return targetField.FieldType == typeof(Transform) &&
                (object)targetField.GetValue(fleeScript) == player.transform;
    }

    //d
    public bool D2_PursuitScript()
    {
        var pursuer = GameObject.Find("Pursuer");
        if (pursuer == null) return false;

        //have to use string get component here because flee doesnt exist in base proj
        var pursuitScript = pursuer.GetComponent("Pursuit");
        return pursuitScript;
    }
    public bool D3_PursuitVars()
    {
        var pursuer = GameObject.Find("Pursuer");
        if (pursuer == null) return false;

        var pursuitScript = pursuer.GetComponent("Pursuit");
        if (pursuitScript == null) return false;

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var pursuitType = pursuitScript.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = pursuitType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) return false;

        var fixedUpdateFunc = pursuitType.GetMethod("FixedUpdate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (fixedUpdateFunc == null) return false;

        return (targetField.FieldType == typeof(Transform) || targetField.FieldType == typeof(Rigidbody2D) || targetField.FieldType == typeof(Mover)) &&
            fixedUpdateFunc.ReturnType == typeof(void);
    }
    public bool D5_PursuitTargetVars()
    {
        var playerRB = CommonTutorialCallbacks.GameObjectComponent<Rigidbody2D>("Player");
        if (playerRB == null) return false;

        var pursuer = GameObject.Find("Pursuer");
        if (pursuer == null) return false;

        var pursuitScript = pursuer.GetComponent("Pursuit");
        if (pursuitScript == null) return false;

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var pursuitType = pursuitScript.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = pursuitType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) return false;

        return targetField.FieldType == typeof(Rigidbody2D) &&
                (object)targetField.GetValue(pursuitScript) == playerRB;
    }
    public bool D6_PredictionTime()
    {
        var pursuer = GameObject.Find("Pursuer");
        if (pursuer == null) return false;

        var pursuitScript = pursuer.GetComponent("Pursuit");
        if (pursuitScript == null) return false;

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var pursuitType = pursuitScript.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var predictionField = pursuitType.GetField("predictionTime", BindingFlags.Instance | BindingFlags.Public);
        if (predictionField == null) return false;

        return predictionField.FieldType == typeof(float);
    }

    //e
    public bool E1_WandererScript()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<Wandering>("Wanderer");
    }

    //f
    const string ComplexAlmostHumanLikeAI = "ComplexAlmostHumanLikeAI";
    public bool F1_Pursuit()
    {
        var playerRB = CommonTutorialCallbacks.GameObjectComponent<Rigidbody2D>("Player");
        if (playerRB == null) return false;

        var script = CommonTutorialCallbacks.GameObjectComponentByName("Pursuit", ComplexAlmostHumanLikeAI);
        if (script == null) return false;

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var scriptType = script.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = scriptType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) return false;

        return targetField.FieldType == typeof(Rigidbody2D) &&
                (object)targetField.GetValue(script) == playerRB;
    }
    public bool F1_Flee()
    {
        var player = GameObject.Find("Player");
        if (player == null) return false;

        var script = CommonTutorialCallbacks.GameObjectComponentByName("Flee", ComplexAlmostHumanLikeAI);
        if (script == null) return false;

        //have to muck around with reflection here, because flee doesnt exist in base proj
        var scriptType = script.GetType();
        //Debug.Log(string.Join(", ", fleeType.GetFields(BindingFlags.Instance | BindingFlags.Public).ToList().Select<FieldInfo, string>(f => f.Name)));
        var targetField = scriptType.GetField("target", BindingFlags.Instance | BindingFlags.Public);
        if (targetField == null) return false;

        return targetField.FieldType == typeof(Transform) &&
                (object)targetField.GetValue(script) == player.transform;
    }
    public bool F1_Wander()
    {
        return CommonTutorialCallbacks.GameObjectContainsScriptByName("Wandering", ComplexAlmostHumanLikeAI);
    }
    public bool F1_AllThreeDisabled()
    {

        return
            F1_Pursuit() &&
            F1_Flee() &&
            F1_Wander() &&
            CommonTutorialCallbacks.GameObjectComponentByName("Pursuit", ComplexAlmostHumanLikeAI).enabled == false &&
            CommonTutorialCallbacks.GameObjectComponentByName("Flee", ComplexAlmostHumanLikeAI).enabled == false &&
            CommonTutorialCallbacks.GameObjectComponentByName("Wandering", ComplexAlmostHumanLikeAI).enabled == false;
    }
    public bool F1_Animator()
    {
        return CommonTutorialCallbacks.GameObjectComponent<Animator>(ComplexAlmostHumanLikeAI) != null;
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
        return GetAnimatorController();
    }
    public bool F2_AnimatorLinked()
    {
        var a = CommonTutorialCallbacks.GameObjectComponent<Animator>(ComplexAlmostHumanLikeAI);
        if (a == null) return false;

        var ac = a.runtimeAnimatorController;
        if (ac == null) return false;

        return ac == GetAnimatorController();
    }
    public bool F2_States()
    {
        return
            GetState("Wandering") &&
            GetState("Fleeing") &&
            GetState("Pursuing") &&
            GetState("Stop and Cry");
    }
    public bool F2_Transitions()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        var wanderingTransitions =
            wandering.transitions.Any(t => t.destinationState == pursuing) &&
            wandering.transitions.Any(t => t.destinationState == fleeing);
        var fleeingTransitions = 
            fleeing.transitions.Any(t => t.destinationState == stopAndCry);
        var pursuingTransitions =
            pursuing.transitions.Any(t => t.destinationState == stopAndCry);
        var stopAndCryTransitions =
            stopAndCry.transitions.Any(t => t.destinationState == wandering);

        return
            wanderingTransitions &&
            fleeingTransitions &&
            pursuingTransitions &&
            stopAndCryTransitions;
    }
    public bool F2_ExitTime()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        return
            wandering.transitions.All(t => t.hasExitTime == false && t.duration == 0) &&
            fleeing.transitions.All(t => t.hasExitTime == false && t.duration == 0) &&
            pursuing.transitions.All(t => t.hasExitTime == false && t.duration == 0);
    }
    public bool F2_ExitTimeStopAndCry()
    {
        var stopAndCry = GetState("Stop and Cry");
        if (stopAndCry == null) return false;

        return
            stopAndCry.transitions.Any(t => t.duration == 1);
    }

    public bool F3_PlayerIsNear()
    {
        var controller = GetAnimatorController();
        if (controller == null) return false;

        var ac = controller as AnimatorController;
        return ac.parameters.Any(p => p.name.Equals("PlayerIsNear") && p.type == AnimatorControllerParameterType.Bool);
    }
    public bool F3_IsScaredOfPlayer()
    {
        var controller = GetAnimatorController();
        if (controller == null) return false;

        var ac = controller as AnimatorController;
        return ac.parameters.Any(p => p.name.Equals("IsScaredOfPlayer") && p.type == AnimatorControllerParameterType.Bool);
    }
    public bool F3_WanderingToPursuing()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;
        /*
        Debug.Log(string.Join("\n", wandering.transitions.ToList().Select(t => t.destinationState.name + " -- " + 
                string.Join("|", t.conditions.ToList().Select(c => c.parameter + "-" + c.mode + "-" + c.threshold)) + " \n" +
                t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.If) + "\n"+
                t.conditions.Any(c => c.parameter == "IsScaredOfPlayer" && c.mode == AnimatorConditionMode.IfNot)
        )));*/
        return wandering.transitions.Any(t => t.destinationState == pursuing &&
                    t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.If) &&
                    t.conditions.Any(c => c.parameter == "IsScaredOfPlayer" && c.mode == AnimatorConditionMode.IfNot));
    }
    public bool F3_WanderingToFleeing()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        return wandering.transitions.Any(t => t.destinationState == fleeing &&
                    t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.If) &&
                    t.conditions.Any(c => c.parameter == "IsScaredOfPlayer" && c.mode == AnimatorConditionMode.If));
    }
    public bool F3_PursuingToStopAndCry()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        return pursuing.transitions.Any(t => t.destinationState == stopAndCry &&
                    t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.IfNot));
    }
    public bool F3_FleeingToStopAndCry()
    {
        var wandering = GetState("Wandering");
        var fleeing = GetState("Fleeing");
        var pursuing = GetState("Pursuing");
        var stopAndCry = GetState("Stop and Cry");

        if (wandering == null || fleeing == null || pursuing == null || stopAndCry == null) return false;

        return fleeing.transitions.Any(t => t.destinationState == stopAndCry &&
                    t.conditions.Any(c => c.parameter == "PlayerIsNear" && c.mode == AnimatorConditionMode.IfNot));
    }

    static AnimationClip GetClip(string name)
    {
        return AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/AI/" + name + ".anim");
    }
    public bool F5_WanderingClip()
    {
        return GetClip("WanderingClip") != null;
    }
    public bool F5_PursuingClip()
    {
        return GetClip("PursuingClip") != null;
    }
    public bool F5_FleeingClip()
    {
        return GetClip("FleeingClip") != null;
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
        return
            CheckRecord(clip, bindings, "Wandering", true) &&
            CheckRecord(clip, bindings, "Flee", false) &&
            CheckRecord(clip, bindings, "Pursuit", false);
    }
    public bool F5_PursuingClipRecord()
    {
        var clip = GetClip("PursuingClip");
        if (clip == null) return false;

        var bindings = AnimationUtility.GetCurveBindings(clip);
        if (bindings.Count() != 3) return false;

        //pursuit = enabled
        return
            CheckRecord(clip, bindings, "Wandering", false) &&
            CheckRecord(clip, bindings, "Flee", false) &&
            CheckRecord(clip, bindings, "Pursuit", true);
    }
    public bool F5_FleeingClipRecord()
    {
        var clip = GetClip("FleeingClip");
        if (clip == null) return false;

        var bindings = AnimationUtility.GetCurveBindings(clip);
        if (bindings.Count() != 3) return false;

        //flee = enabled
        return
            CheckRecord(clip, bindings, "Wandering", false) &&
            CheckRecord(clip, bindings, "Flee", true) &&
            CheckRecord(clip, bindings, "Pursuit", false);
    }
    public bool F6_WanderingClipState()
    {
        var state = GetState("Wandering");
        if (state == null) return false;

        var clip = GetClip("WanderingClip");
        if (clip == null) return false;

        return state.motion == clip;
    }
    public bool F6_PursuingClipState()
    {
        var state = GetState("Pursuing");
        if (state == null) return false;

        var clip = GetClip("PursuingClip");
        if (clip == null) return false;

        return state.motion == clip;
    }
    public bool F6_FleeingClipState()
    {
        var state = GetState("Fleeing");
        if (state == null) return false;

        var clip = GetClip("FleeingClip");
        if (clip == null) return false;

        return state.motion == clip;
    }

    static Transform GetRadius()
    {
        var player = GameObject.Find("Player");
        if (player == null) return null;

        return player.transform.Find("Radius");
    }
    public bool F8_Radius()
    {
        return GetRadius() != null;
    }
    public bool F8_Radius_Collider()
    {
        var radius = GetRadius();
        if (radius == null) return false;

        var circle = radius.GetComponent<CircleCollider2D>();
        if (circle == null) return false;

        return circle.isTrigger && circle.radius >= 2.5f;
    }
    public bool F8_Radius_TriggerScript()
    {
        var radius = GetRadius();
        if (radius == null) return false;

        var triggerScript = radius.GetComponent<SetAnimatorBooleanOnTriggerStay>();
        if (triggerScript == null) return false;

        return 
            triggerScript.booleanName == "PlayerIsNear" &&
            triggerScript.applyToOther;
    }
    public bool F8_Radius_Layer()
    {
        var radius = GetRadius();
        if (radius == null) return false;

        return radius.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast");
    }
    public bool F9_ScaredOfPlayer()
    {
        var triggerScript = CommonTutorialCallbacks.GameObjectComponent<SetAnimatorBooleanOnInput>(ComplexAlmostHumanLikeAI);
        if (triggerScript == null) return false;

        return triggerScript.animatorBoolean == "IsScaredOfPlayer" &&
            triggerScript.key == KeyCode.Space;
    }
    public bool F10_MultipleAI()
    {
        var objs = CommonTutorialCallbacks.GameObjectsStartingWith(ComplexAlmostHumanLikeAI);
        return objs.Count >= 3 && CommonTutorialCallbacks.ObjectsInDifferentLocations(objs);
    }

    //g

    public bool G2_MultipleNodes()
    {
        var objs = CommonTutorialCallbacks.GameObjectsStartingWith("Node");
        return objs.Count >= 6 && CommonTutorialCallbacks.ObjectsInDifferentLocations(objs);
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
                    return false;
                }
            }
        }
        return true;
    }
}
#endif