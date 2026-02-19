#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.Tutorials.Core.Editor;
using System.Linq;
using UnityEditor.Animations;

/// <summary>
/// Implement your Tutorial callbacks here.
/// </summary>
[CreateAssetMenu(fileName = DefaultFileName, menuName = "Tutorials/" + DefaultFileName + " Instance")]
public class Tutorial08Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial08Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial08Callbacks>(DefaultFileName);
    }

    //b
    public bool B1_SpriteMode()
    {
        var merc = (TextureImporter)AssetImporter.GetAtPath("Assets/Mercenary.png");
        if (merc == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Mercenary.png'."; return false; }

        if (merc.spriteImportMode != SpriteImportMode.Multiple) { Criterion.globalLastKnownError = "Sprite Mode is not set to 'Multiple'."; return false; }
        return true;
    }
    public bool B1SpriteSlice()
    {
        var merc = (TextureImporter)AssetImporter.GetAtPath("Assets/Mercenary.png");
        if (merc == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Mercenary.png'."; return false; }

#pragma warning disable 0618
        if (merc.spritesheet.Count() != 8 * 6) { Criterion.globalLastKnownError = "Sprite sheet is not sliced correctly. Expected 48 sprites."; return false; }
        return true;
#pragma warning restore 0618
    }
    public bool B1SpritePPU()
    {
        var merc = (TextureImporter)AssetImporter.GetAtPath("Assets/Mercenary.png");
        if (merc == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Mercenary.png'."; return false; }

        if (merc.spritePixelsPerUnit != 32) { Criterion.globalLastKnownError = "Pixels Per Unit is not 32."; return false; }
        return true;
    }
    public bool B1SpriteFilter()
    {
        var merc = (TextureImporter)AssetImporter.GetAtPath("Assets/Mercenary.png");
        if (merc == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Mercenary.png'."; return false; }

        if (merc.filterMode != FilterMode.Point) { Criterion.globalLastKnownError = "Filter Mode is not set to 'Point (no filter)'."; return false; }
        return true;
    }
    public bool B2_B3_Anim(string name)
    {
        var a = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Animations/Walking/" + name + ".anim");
        if (a == null) { Criterion.globalLastKnownError = $"Could not find animation 'Assets/Animations/Walking/{name}.anim'."; return false; }

        //check there are 4 sprites in the animation
        var bindings = AnimationUtility.GetObjectReferenceCurveBindings(a);
        if (bindings.Count() != 1) { Criterion.globalLastKnownError = $"Animation '{name}' should affect exactly one property (Sprite). Found {bindings.Count()}."; return false; }

        var binding = bindings[0];
        var curve = AnimationUtility.GetObjectReferenceCurve(a, binding);
        if (curve.Count() != 4) { Criterion.globalLastKnownError = $"Animation '{name}' should have exactly 4 keyframes. Found {curve.Count()}."; return false; }

        return true;
    }
    public bool B5_NoMercs()
    {
        if (CommonTutorialCallbacks.GameObjectsStartingWith("Merc").Count != 0) { Criterion.globalLastKnownError = "Found 'Mercenary' GameObjects in the scene. Please delete them."; return false; }
        return true;
    }
    public bool B5_NoAnimControllers()
    {
        for (var i = 0; i < 6*8; i++)
        {
            var a = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Walking/Mercenary_" + i + ".controller");
            if (a != null) { Criterion.globalLastKnownError = $"Found Animator Controller '{a.name}'. Please delete it."; return false; }
        }
        return true;
    }

    //c
    static RuntimeAnimatorController GetPlayerController()
    {
        return AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/PlayerController.controller");
    }

    public bool C1_AnimController()
    {
        var pc = GetPlayerController();
        if (pc == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Animations/PlayerController.controller'."; return false; }
        return true;
    }
    static BlendTree GetBlendtree(string name)
    {
        var player = GetPlayerController();
        if (player == null) return null;

        var ac = player as AnimatorController;
        bool any = ac.layers.First().stateMachine.states.Any(s => s.state.name.Equals(name));
        if (any == false) return null;

        var state = ac.layers.First().stateMachine.states.FirstOrDefault(s => s.state.name.Equals(name));
        return state.state.motion as BlendTree;
    }
    public static BlendTree GetWalkingBlendtree()
    {
        return GetBlendtree("Walking");
    }
    public bool C2_Walking()
    {
        if (GetWalkingBlendtree() == null) { Criterion.globalLastKnownError = "PlayerController does not have a 'Walking' state with a Blend Tree."; return false; }
        return true;
    }
    public bool C3_BlendTreeMode()
    {
        var tree = GetWalkingBlendtree();
        if (tree == null) { Criterion.globalLastKnownError = "Could not find 'Walking' Blend Tree."; return false; }

        if (tree.blendType != BlendTreeType.SimpleDirectional2D) { Criterion.globalLastKnownError = "Blend Tree Type is not 'Simple Directional 2D'."; return false; }
        return true;
    }
    public bool C4_Parameter(string param)
    {
        var player = GetPlayerController();
        if (player == null) { Criterion.globalLastKnownError = "PlayerController not found."; return false; }

        var ac = player as AnimatorController;
        if (!ac.parameters.Any(p => p.name.Equals(param) && p.type == AnimatorControllerParameterType.Float)) { Criterion.globalLastKnownError = $"PlayerController is missing Float parameter '{param}'."; return false; }
        return true;
    }
    public bool C4_BlendParameters()
    {

        var tree = GetWalkingBlendtree();
        if (tree == null) { Criterion.globalLastKnownError = "Could not find 'Walking' Blend Tree."; return false; }
        
        if (!tree.blendParameter.Equals("XSpeed") || !tree.blendParameterY.Equals("YSpeed")) { Criterion.globalLastKnownError = "Blend Tree parameters should be 'XSpeed' and 'YSpeed'."; return false; }
        return true;
    }
    public bool C5_MotionField()
    {
        var tree = GetWalkingBlendtree();
        if (tree == null) { Criterion.globalLastKnownError = "Could not find 'Walking' Blend Tree."; return false; }

        if (tree.children.Count() == 0) { Criterion.globalLastKnownError = "Blend Tree has no motions added."; return false; }
        return true;

    }
    public bool C5_C6_MotionField(string name, float x, float y)
    {
        var tree = GetWalkingBlendtree();
        if (tree == null) return false;

        for (var i = 0; i < tree.children.Count(); i++)
        {
            var child = tree.children[i];
            if (child.motion != null && child.motion.name.Equals(name))
            {
                return child.position.x.Equals(x) && child.position.y.Equals(y);
            }
        }

        Criterion.globalLastKnownError = $"Could not find motion '{name}' at position ({x}, {y}) in 'Walking' Blend Tree.";
            return false;

    }

    //d
    public bool D1_PlayerSprite()
    {
        if (!CommonTutorialCallbacks.GameObjectComponent<SpriteRenderer>("Player")) { Criterion.globalLastKnownError = "Player GameObject is missing a SpriteRenderer."; return false; }
        return true;
    }
    public bool D1_PlayerRB()
    {
        var rb = CommonTutorialCallbacks.GameObjectComponent<Rigidbody2D>("Player");
        if (rb == null) { Criterion.globalLastKnownError = "Player GameObject is missing a Rigidbody2D."; return false; }

        if (rb.gravityScale != 0) { Criterion.globalLastKnownError = "Player Rigidbody2D Gravity Scale should be 0."; return false; }
        if (!rb.constraints.HasFlag(RigidbodyConstraints2D.FreezeRotation)) { Criterion.globalLastKnownError = "Player Rigidbody2D should have 'Freeze Rotation' checked."; return false; }
        return true;

    }
    public bool D1_PlayerAnimator()
    {
        var a = CommonTutorialCallbacks.GameObjectComponent<Animator>("Player");
        if (a == null) { Criterion.globalLastKnownError = "Player GameObject is missing an Animator."; return false; }

        var ac = a.runtimeAnimatorController;
        if (ac == null) { Criterion.globalLastKnownError = "Player Animator is missing a Controller."; return false; }

        if (ac != GetPlayerController()) { Criterion.globalLastKnownError = "Player Animator Controller is not set to 'PlayerController'."; return false; }
        return true;
    }
    public bool D1_Player8Way()
    {
        if (!CommonTutorialCallbacks.GameObjectComponent<EightWayMovement>("Player")) { Criterion.globalLastKnownError = "Player GameObject is missing 'EightWayMovement' script."; return false; }
        return true;
    }
    public bool D1_OrderInLayer()
    {
        var sr = CommonTutorialCallbacks.GameObjectComponent<SpriteRenderer>("Player");
        if (sr == null) { Criterion.globalLastKnownError = "Player GameObject is missing a SpriteRenderer."; return false; }

        if (sr.sortingOrder < 1) { Criterion.globalLastKnownError = "Player SpriteRenderer Sorting Order must be at least 1."; return false; }
        return true;
    }
    public bool D1_Tag()
    {
        var player = GameObject.Find("Player");
        if (player == null) { Criterion.globalLastKnownError = "Could not find GameObject named 'Player'."; return false; }

        if (player.tag != "Player") { Criterion.globalLastKnownError = "Player GameObject tag is not set to 'Player'."; return false; }
        return true;
    }
    public static BlendTree GetIdleBlendtree()
    {
        return GetBlendtree("Idle");
    }
    public static BlendTree GetShootingBlendtree()
    {
        return GetBlendtree("Shooting");
    }
    public bool D3_Idle()
    {
        if (GetIdleBlendtree() == null) { Criterion.globalLastKnownError = "PlayerController does not have an 'Idle' state with a Blend Tree."; return false; }
        return true;
    }
    public bool D3_Shooting()
    {
        if (GetShootingBlendtree() == null) { Criterion.globalLastKnownError = "PlayerController does not have a 'Shooting' state with a Blend Tree."; return false; }
        return true;
    }
    public bool D4_MotionField_Idle()
    {
        var tree = GetIdleBlendtree();
        if (tree == null) return false;

        for (var i = 0; i < tree.children.Count(); i++)
        {
            var child = tree.children[i];
            if (child.motion == null || child.motion.name.Contains("_Idle") == false)
            {
                Criterion.globalLastKnownError = "Idle Blend Tree should only contain motions with '_Idle' in their name.";
                return false;
            }
        }

        return true;

    }
    public bool D4_MotionField_Shooting()
    {
        var tree = GetShootingBlendtree();
        if (tree == null) return false;

        for (var i = 0; i < tree.children.Count(); i++)
        {
            var child = tree.children[i];
            if (child.motion == null || child.motion.name.Contains("_Shoot") == false)
            {
                Criterion.globalLastKnownError = "Shooting Blend Tree should only contain motions with '_Shoot' in their name.";
                return false;
            }
        }

        return true;

    }
    public bool D5_AnyStateTransition(string toStateName)
    {

        var rac = GetPlayerController();
        if (rac == null) { Criterion.globalLastKnownError = "PlayerController not found."; return false; }

        var ac = rac as AnimatorController;
        if (!ac.layers.First().stateMachine.anyStateTransitions.Any(t => t.destinationState != null && t.destinationState.name.Equals(toStateName))) { Criterion.globalLastKnownError = $"Missing 'Any State' transition to '{toStateName}'."; return false; }
        return true;

    }
    public bool D6_ParameterExists(string name)
    {
        var rac = GetPlayerController();
        if (rac == null) { Criterion.globalLastKnownError = "PlayerController not found."; return false; }

        var ac = rac as AnimatorController;
        if (!ac.parameters.Any(p => p.name.Equals(name) && p.type == AnimatorControllerParameterType.Bool)) { Criterion.globalLastKnownError = $"PlayerController is missing Bool parameter '{name}'."; return false; }
        return true;
    }

    public bool D6_AnyStateTransition(string toStateName, string conditionVar, bool conditionValue)
    {

        var rac = GetPlayerController();
        if (rac == null) { Criterion.globalLastKnownError = "PlayerController not found."; return false; }

        var ac = rac as AnimatorController;/*
        ac.layers.First().stateMachine.anyStateTransitions.First(t =>
            t.destinationState != null &&
            t.destinationState.name.Equals(toStateName)).conditions.ToList().ForEach(a => Debug.Log(a.parameter + "_" + a.threshold + "_" + a.mode+"_"+a));*/
        
        var hasTransition = ac.layers.First().stateMachine.anyStateTransitions.Any(t => 
            t.destinationState != null &&
            t.destinationState.name.Equals(toStateName) &&
            t.conditions.Any(c =>
                c.parameter.Equals(conditionVar) &&
                c.mode.Equals(conditionValue ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot)
            )
        );

        if (!hasTransition) { Criterion.globalLastKnownError = $"Missing transition from 'Any State' to '{toStateName}' with condition '{conditionVar}' is {conditionValue}."; return false; }
        return true;

    }
    public bool D7_NoExitTime()
    {
        var rac = GetPlayerController();
        if (rac == null) { Criterion.globalLastKnownError = "PlayerController not found."; return false; }

        var ac = rac as AnimatorController;
        if (!ac.layers.First().stateMachine.anyStateTransitions.All(t => t.hasExitTime.Equals(false) && t.duration.Equals(0) && t.canTransitionToSelf.Equals(false)))
        {
             Criterion.globalLastKnownError = "One or more 'Any State' transitions have Exit Time, Duration > 0, or Can Transition To Self checked. Please uncheck them / set duration to 0.";
             return false;
        }
        return true;
    }
    public bool E1_PlayerShooting()
    {
        if (!CommonTutorialCallbacks.GameObjectComponent<PlayerShooting>("Player")) { Criterion.globalLastKnownError = "Player GameObject is missing 'PlayerShooting' script."; return false; }
        return true;
    }
}
#endif