#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.Tutorials.Core.Editor;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine.Tilemaps;

/// <summary>
/// Implement your Tutorial callbacks here.
/// </summary>
[CreateAssetMenu(fileName = DefaultFileName, menuName = "Tutorials/" + DefaultFileName + " Instance")]
public class Tutorial09Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial09Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial09Callbacks>(DefaultFileName);
    }
    
    //a
    public bool A2_CheckPlayerEightWay()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<EightWayMovement>("Player")) { Criterion.globalLastKnownError = "Player GameObject is missing 'EightWayMovement' script."; return false; }
        return true;
    }
    public bool A2_CheckPlayerAnimator()
    {
        if (!CommonTutorialCallbacks.GameObjectContainsScript<Animator>("Player")) { Criterion.globalLastKnownError = "Player GameObject is missing an Animator."; return false; }
        if (!Tutorial08Callbacks.GetIdleBlendtree() || !Tutorial08Callbacks.GetShootingBlendtree() || !Tutorial08Callbacks.GetWalkingBlendtree())
        {
             Criterion.globalLastKnownError = "Missing one or more BlendTrees from Tutorial 08 (Idle, Shooting, or Walking).";
             return false;
        }
        return true;
    }

    //b
    public bool B1SpriteMode()
    {
        if (walls == null || lights == null || decor == null) { Criterion.globalLastKnownError = "One or more textures (WallsMap, LightsMap, DecorMap) could not be found in 'Assets/TilemapSprites/'."; return false; }
        
        if (walls.spriteImportMode != SpriteImportMode.Multiple || lights.spriteImportMode != SpriteImportMode.Multiple || decor.spriteImportMode != SpriteImportMode.Multiple)
        {
             Criterion.globalLastKnownError = "Sprite Mode is not set to 'Multiple' for one or more textures (WallsMap, LightsMap, DecorMap).";
             return false;
        }
        return true;
    }
    public bool B1SpriteSlice()
    {
        if (walls == null || lights == null || decor == null) { Criterion.globalLastKnownError = "One or more textures could not be found."; return false; }

        //Debug.Log(walls.spritesheet.Count() + " ," + lights.spritesheet.Count() + " ," + decor.spritesheet.Count());
#pragma warning disable 0618
        if (walls.spritesheet.Count() == 0) { Criterion.globalLastKnownError = "WallsMap sprite sheet is empty. Please slice it."; return false; }
        if (lights.spritesheet.Count() != 2) { Criterion.globalLastKnownError = $"LightsMap should have 2 sprites. Found {lights.spritesheet.Count()}."; return false; }
        if (decor.spritesheet.Count() != 15) { Criterion.globalLastKnownError = $"DecorMap should have 15 sprites. Found {decor.spritesheet.Count()}."; return false; }
        return true;
#pragma warning restore 0618
    }
    public bool B1SpriteFilter()
    {
        if (walls == null || lights == null || decor == null) { Criterion.globalLastKnownError = "One or more textures could not be found."; return false; }

        if (walls.filterMode != FilterMode.Point || lights.filterMode != FilterMode.Point || decor.filterMode != FilterMode.Point)
        {
             Criterion.globalLastKnownError = "Filter Mode is not set to 'Point (no filter)' for one or more textures.";
             return false;
        }
        return true;
    }
    public bool B2SpriteCount()
    {
        if (walls == null) { Criterion.globalLastKnownError = "Could not find 'Assets/TilemapSprites/WallsMap.png'."; return false; }

#pragma warning disable 0618
        if (walls.spritesheet.Count() != 9) { Criterion.globalLastKnownError = $"WallsMap should be sliced into 9 sprites. Found {walls.spritesheet.Count()}."; return false; }
        return true;
#pragma warning restore 0618
    }
    public bool B2SpriteSlice()
    {
        var walls = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/WallsMap.png");
        if (walls == null) return false;

        //Debug.Log(walls.spritesheet.Count() + " ," + lights.spritesheet.Count() + " ," + decor.spritesheet.Count());
        /*for (var i = 0; i < walls.spritesheet.Length; i++)
        {
            var t = walls.spritesheet[i];
            Debug.Log(i+" | "+t.rect.yMax + " | " + t.rect.xMax + " | " + t.rect.yMin + " | " + t.rect.xMin);
        }*/
#pragma warning disable 0618
        var correctCount = walls.spritesheet.Count() == 9;
        var correctRects = walls.spritesheet.Any(t => t.rect.yMax.Equals(64) && t.rect.xMax.Equals(64) && t.rect.yMin.Equals(48) && t.rect.xMin.Equals(48));

        if (!correctCount) { Criterion.globalLastKnownError = $"WallsMap sprite count is incorrect. Expected 9, found {walls.spritesheet.Count()}."; return false; }
        if (!correctRects) { Criterion.globalLastKnownError = "WallsMap slicing seems incorrect. Check if any sprite has rect (48,48,64,64)."; return false; }
        return true;
#pragma warning restore 0618
    }
    public bool B4TilesWall()
    {
        //there should be 9 but lets not assume their numbers
        var count = 0;
        for (var i = 0; i <= 16; i++)
        {
            if (AssetDatabase.LoadAssetAtPath<Tile>("Assets/Tiles/WallsMap_" + i + ".asset") != null)
            {
                count++;
            }
        }
        if (count != 9) { Criterion.globalLastKnownError = $"Could not find all 9 Wall Tiles in 'Assets/Tiles'. Found {count}."; return false; }
        return true;
    }
    public bool B5TilesDecor()
    {
        //there should be 9 but lets not assume their numbers
        var count = 0;
        for (var i = 0; i <= 16; i++)
        {
            if (AssetDatabase.LoadAssetAtPath<Tile>("Assets/Tiles/DecorMap_" + i + ".asset") != null)
            {
                count++;
            }
        }
        if (count != 15) { Criterion.globalLastKnownError = $"Could not find all 15 Decor Tiles in 'Assets/Tiles'. Found {count}."; return false; }
        return true;
    }
    public bool B5TilesLight()
    {
        //there should be 9 but lets not assume their numbers
        var count = 0;
        for (var i = 0; i <= 16; i++)
        {
            if (AssetDatabase.LoadAssetAtPath<Tile>("Assets/Tiles/LightsMap_" + i + ".asset") != null)
            {
                count++;
            }
        }
        if (count != 2) { Criterion.globalLastKnownError = $"Could not find all 2 Light Tiles in 'Assets/Tiles'. Found {count}."; return false; }
        return true;
    }

    //d
    public bool D1TilemapCollider2D()
    {
        if (!CommonTutorialCallbacks.GameObjectComponent<TilemapCollider2D>("Background")) { Criterion.globalLastKnownError = "'Background' GameObject is missing a TilemapCollider2D."; return false; }
        return true;
    }
    public bool D2SelectedTile()
    {
        if (UnityEditor.Tilemaps.GridSelection.active == false) { Criterion.globalLastKnownError = "No tile selected in the palette."; return false; }

        var pos = UnityEditor.Tilemaps.GridSelection.position;
        // Check target exists to avoid null reference? Original code assumes it.
        if (UnityEditor.Tilemaps.GridSelection.target == null) return false;

        var selected = UnityEditor.Tilemaps.GridSelection.target.GetComponent<Tilemap>().GetTile(pos.min);
        if (selected == null) { Criterion.globalLastKnownError = "Selected cell is empty."; return false; }
        
        if (!selected.name.Equals("WallsMap_10")) { Criterion.globalLastKnownError = $"Selected tile is '{selected.name}', but should be 'WallsMap_10'."; return false; }
        return true;
    }
    public bool D3ColliderType()
    {
        var tile = AssetDatabase.LoadAssetAtPath<Tile>("Assets/Tiles/WallsMap_10.asset");
        if (tile == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Tiles/WallsMap_10.asset'."; return false; }

        if (!tile.colliderType.Equals(Tile.ColliderType.None)) { Criterion.globalLastKnownError = "Tile Collider Type is not set to 'None'."; return false; }
        return true;
    }
    public bool D4PlayerCollider()
    {
        if (CommonTutorialCallbacks.GameObjectComponent<Collider2D>("Player") == null) { Criterion.globalLastKnownError = "Player GameObject is missing a Collider2D."; return false; }
        return true;
    }

    //really common for people to have a "slider background" object, so lets get it a different way...
    T GetComponentSpecific<T>(string goName) where T : Component
    {
        var compcollider = FindFirstObjectByType<T>();
        if (compcollider == null) return default(T);

        if (compcollider.gameObject.name.Equals(goName) == false) return default(T);

        return compcollider;
    }
    public bool D5CompositeCollider()
    {
        //return CommonTutorialCallbacks.GameObjectComponent<CompositeCollider2D>("Background") != null;
        if (GetComponentSpecific<CompositeCollider2D>("Background") == null) { Criterion.globalLastKnownError = "'Background' GameObject is missing a CompositeCollider2D."; return false; }
        return true;
    }
    public bool D5UsedByComposite()
    {

        var collider = GetComponentSpecific<TilemapCollider2D>("Background");
        if (collider == null) { Criterion.globalLastKnownError = "'Background' GameObject is missing a TilemapCollider2D."; return false; }

#pragma warning disable 0618
        if (!collider.usedByComposite) { Criterion.globalLastKnownError = "TilemapCollider2D 'Used By Composite' checkmark is not enabled."; return false; }
        return true;
#pragma warning restore 0618
    }
    public bool D6RB()
    {
        var rb = GetComponentSpecific<Rigidbody2D>("Background");
        if (rb == null) { Criterion.globalLastKnownError = "'Background' GameObject is missing a Rigidbody2D."; return false; }

        if (!rb.bodyType.Equals(RigidbodyType2D.Static)) { Criterion.globalLastKnownError = "Rigidbody2D Body Type is not set to 'Static'."; return false; }
        return true;
    }
    public bool D7TilemapCollider2D()
    {
        if (!CommonTutorialCallbacks.GameObjectComponent<TilemapCollider2D>("Decorations")) { Criterion.globalLastKnownError = "'Decorations' GameObject is missing a TilemapCollider2D."; return false; }
        return true;
        //return GetCompositeColliderComponent<TilemapCollider2D>("Decorations");
    }

    //f
    public bool F1LightsTile()
    {
        if (AssetDatabase.LoadAssetAtPath<AnimatedTile>("Assets/Tiles/Lights.asset") == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Tiles/Lights.asset'."; return false; }
        return true;
    }
    public bool F1LightsTileCount()
    {
        var tile = AssetDatabase.LoadAssetAtPath<AnimatedTile>("Assets/Tiles/Lights.asset");
        if (tile == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Tiles/Lights.asset'."; return false; }

        if (tile.m_AnimatedSprites.Length != 2) { Criterion.globalLastKnownError = $"AnimatedTile should have 2 sprites. Found {tile.m_AnimatedSprites.Length}."; return false; }
        return true;
    }
    public bool F1LightsTileSprites()
    {
        var tile = AssetDatabase.LoadAssetAtPath<AnimatedTile>("Assets/Tiles/Lights.asset");
        if (tile == null) { Criterion.globalLastKnownError = "Could not find 'Assets/Tiles/Lights.asset'."; return false; }

        if (tile.m_AnimatedSprites.Length != 2) { Criterion.globalLastKnownError = "AnimatedTile sprite count incorrect."; return false; }
        
        bool sprite0Correct = tile.m_AnimatedSprites[0] != null && tile.m_AnimatedSprites[0].name.Equals("LightsMap_0");
        bool sprite1Correct = tile.m_AnimatedSprites[1] != null && tile.m_AnimatedSprites[1].name.Equals("LightsMap_1");

        if (!sprite0Correct || !sprite1Correct) { Criterion.globalLastKnownError = "AnimatedTile sprites should be 'LightsMap_0' and 'LightsMap_1'."; return false; }
        return true;
    }
}
#endif