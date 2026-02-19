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
    public bool B1SpriteMode()
    {
        var merc = (TextureImporter)AssetImporter.GetAtPath("Assets/Mercenary.png");
        if (merc == null) return false;

        return merc.spriteImportMode == SpriteImportMode.Multiple;
    }
    public bool B1SpriteSlice()
    {
        var merc = (TextureImporter)AssetImporter.GetAtPath("Assets/Mercenary.png");
        if (merc == null) return false;

#pragma warning disable 0618
        return merc.spritesheet.Count() == 8 * 6;
#pragma warning restore 0618
    }
    public bool B1SpritePPU()
    {
        var merc = (TextureImporter)AssetImporter.GetAtPath("Assets/Mercenary.png");
        if (merc == null) return false;

        return merc.spritePixelsPerUnit == 32;
    }
    public bool B1SpriteFilter()
    {
        var merc = (TextureImporter)AssetImporter.GetAtPath("Assets/Mercenary.png");
        if (merc == null) return false;

        return merc.filterMode == FilterMode.Point;
    }
    public bool B2_B3_Anim(string name)
    {
        var a = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Animations/Walking/" + name + ".anim");
        if (a == null) return false;

        //check there are 4 sprites in the animation
        var bindings = AnimationUtility.GetObjectReferenceCurveBindings(a);
        if (bindings.Count() != 1) return false;

        var binding = bindings[0];
        var curve = AnimationUtility.GetObjectReferenceCurve(a, binding);
        if (curve.Count() != 4) return false;

        return true;
    }
    public bool B5_NoMercs()
    {
        return CommonTutorialCallbacks.GameObjectsStartingWith("Merc").Count == 0;
    }
    public bool B5_NoAnimControllers()
    {
        for (var i = 0; i < 6*8; i++)
        {
            var a = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Walking/Mercenary_" + i + ".controller");
            if (a != null) return false;
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
        return GetPlayerController();
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
        return GetWalkingBlendtree() != null;
    }
    public bool C3_BlendTreeMode()
    {
        var tree = GetWalkingBlendtree();
        if (tree == null) return false;

        return tree.blendType == BlendTreeType.SimpleDirectional2D;
    }
    public bool C4_Parameter(string param)
    {
        var player = GetPlayerController();
        if (player == null) return false;

        var ac = player as AnimatorController;
        return ac.parameters.Any(p => p.name.Equals(param) && p.type == AnimatorControllerParameterType.Float);
    }
    public bool C4_BlendParameters()
    {

        var tree = GetWalkingBlendtree();
        if (tree == null) return false;
        
        
        return tree.blendParameter.Equals("XSpeed") && tree.blendParameterY.Equals("YSpeed");
    }
    public bool C5_MotionField()
    {
        var tree = GetWalkingBlendtree();
        if (tree == null) return false;

        return tree.children.Count() > 0;

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

            return false;

    }

    //d
    public bool D1_PlayerSprite()
    {
        return CommonTutorialCallbacks.GameObjectComponent<SpriteRenderer>("Player");
    }
    public bool D1_PlayerRB()
    {
        var rb = CommonTutorialCallbacks.GameObjectComponent<Rigidbody2D>("Player");
        if (rb == null) return false;

        return rb.gravityScale == 0 && rb.constraints.HasFlag(RigidbodyConstraints2D.FreezeRotation);

    }
    public bool D1_PlayerAnimator()
    {
        var a = CommonTutorialCallbacks.GameObjectComponent<Animator>("Player");
        if (a == null) return false;

        var ac = a.runtimeAnimatorController;
        if (ac == null) return false;

        return ac == GetPlayerController();
    }
    public bool D1_Player8Way()
    {
        return CommonTutorialCallbacks.GameObjectComponent<EightWayMovement>("Player");
    }
    public bool D1_OrderInLayer()
    {
        var sr = CommonTutorialCallbacks.GameObjectComponent<SpriteRenderer>("Player");
        if (sr == null) return false;

        return sr.sortingOrder >= 1;
    }
    public bool D1_Tag()
    {
        var player = GameObject.Find("Player");
        if (player == null) return false;

        return player.tag == "Player";
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
        return GetIdleBlendtree() != null;
    }
    public bool D3_Shooting()
    {
        return GetShootingBlendtree() != null;
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
                return false;
            }
        }

        return true;

    }
    public bool D5_AnyStateTransition(string toStateName)
    {

        var rac = GetPlayerController();
        if (rac == null) return false;

        var ac = rac as AnimatorController;
        return ac.layers.First().stateMachine.anyStateTransitions.Any(t => t.destinationState != null && t.destinationState.name.Equals(toStateName));

    }
    public bool D6_ParameterExists(string name)
    {
        var rac = GetPlayerController();
        if (rac == null) return false;

        var ac = rac as AnimatorController;
        return ac.parameters.Any(p => p.name.Equals(name) && p.type == AnimatorControllerParameterType.Bool);
    }

    public bool D6_AnyStateTransition(string toStateName, string conditionVar, bool conditionValue)
    {

        var rac = GetPlayerController();
        if (rac == null) return false;

        var ac = rac as AnimatorController;/*
        ac.layers.First().stateMachine.anyStateTransitions.First(t =>
            t.destinationState != null &&
            t.destinationState.name.Equals(toStateName)).conditions.ToList().ForEach(a => Debug.Log(a.parameter + "_" + a.threshold + "_" + a.mode+"_"+a));*/
        return ac.layers.First().stateMachine.anyStateTransitions.Any(t => 
            t.destinationState != null &&
            t.destinationState.name.Equals(toStateName) &&
            t.conditions.Any(c =>
                c.parameter.Equals(conditionVar) &&
                c.mode.Equals(conditionValue ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot)
            )
        );

    }
    public bool D7_NoExitTime()
    {
        var rac = GetPlayerController();
        if (rac == null) return false;

        var ac = rac as AnimatorController;
        return ac.layers.First().stateMachine.anyStateTransitions.All(t => t.hasExitTime.Equals(false) && t.duration.Equals(0) && t.canTransitionToSelf.Equals(false));
    }
    public bool E1_PlayerShooting()
    {
        return CommonTutorialCallbacks.GameObjectComponent<PlayerShooting>("Player");
    }
}
#endif