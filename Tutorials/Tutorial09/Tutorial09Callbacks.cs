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
        return CommonTutorialCallbacks.GameObjectContainsScript<EightWayMovement>("Player");
    }
    public bool A2_CheckPlayerAnimator()
    {
        return CommonTutorialCallbacks.GameObjectContainsScript<Animator>("Player") &&
            Tutorial08Callbacks.GetIdleBlendtree() &&
            Tutorial08Callbacks.GetShootingBlendtree() &&
            Tutorial08Callbacks.GetWalkingBlendtree();
    }

    //b
    public bool B1SpriteMode()
    {
        var walls = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/WallsMap.png");
        if (walls == null) return false;
        var lights = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/LightsMap.png");
        if (lights == null) return false;
        var decor = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/DecorMap.png");
        if (decor == null) return false;

        return walls.spriteImportMode == SpriteImportMode.Multiple &&
            lights.spriteImportMode == SpriteImportMode.Multiple && 
            decor.spriteImportMode == SpriteImportMode.Multiple;
    }
    public bool B1SpriteSlice()
    {
        var walls = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/WallsMap.png");
        if (walls == null) return false;
        var lights = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/LightsMap.png");
        if (lights == null) return false;
        var decor = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/DecorMap.png");
        if (decor == null) return false;

        //Debug.Log(walls.spritesheet.Count() + " ," + lights.spritesheet.Count() + " ," + decor.spritesheet.Count());
#pragma warning disable 0618
        return walls.spritesheet.Count() > 0&&// was 16, but later they remove some down to 9
            lights.spritesheet.Count() == 2 &&
            decor.spritesheet.Count() == 15;
#pragma warning restore 0618
    }
    public bool B1SpriteFilter()
    {
        var walls = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/WallsMap.png");
        if (walls == null) return false;
        var lights = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/LightsMap.png");
        if (lights == null) return false;
        var decor = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/DecorMap.png");
        if (decor == null) return false;

        return walls.filterMode == FilterMode.Point &&
            lights.filterMode == FilterMode.Point &&
            decor.filterMode == FilterMode.Point;
    }
    public bool B2SpriteCount()
    {
        var walls = (TextureImporter)AssetImporter.GetAtPath("Assets/TilemapSprites/WallsMap.png");
        if (walls == null) return false;

#pragma warning disable 0618
        return walls.spritesheet.Count() == 9;
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
        return walls.spritesheet.Count() == 9 && 
            walls.spritesheet.Any(t => t.rect.yMax.Equals(64) && t.rect.xMax.Equals(64) && t.rect.yMin.Equals(48) && t.rect.xMin.Equals(48));
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
        return count == 9;
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
        return count == 15;
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
        return count == 2;
    }

    //d
    public bool D1TilemapCollider2D()
    {
        return CommonTutorialCallbacks.GameObjectComponent<TilemapCollider2D>("Background");
    }
    public bool D2SelectedTile()
    {
        if (UnityEditor.Tilemaps.GridSelection.active == false) return false;

        var pos = UnityEditor.Tilemaps.GridSelection.position;
        var selected = UnityEditor.Tilemaps.GridSelection.target.GetComponent<Tilemap>().GetTile(pos.min);
        if (selected == null) return false;
        
        return selected.name.Equals("WallsMap_10");
    }
    public bool D3ColliderType()
    {
        var tile = AssetDatabase.LoadAssetAtPath<Tile>("Assets/Tiles/WallsMap_10.asset");
        if (tile == null) return false;

        return tile.colliderType.Equals(Tile.ColliderType.None);
    }
    public bool D4PlayerCollider()
    {
        return CommonTutorialCallbacks.GameObjectComponent<Collider2D>("Player") != null;
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
        return GetComponentSpecific<CompositeCollider2D>("Background") != null;
    }
    public bool D5UsedByComposite()
    {

        var collider = GetComponentSpecific<TilemapCollider2D>("Background");//CommonTutorialCallbacks.GameObjectComponent<TilemapCollider2D>("Background");
        if (collider == null) return false;

#pragma warning disable 0618
        return collider.usedByComposite;
#pragma warning restore 0618
    }
    public bool D6RB()
    {
        var rb = GetComponentSpecific<Rigidbody2D>("Background");//CommonTutorialCallbacks.GameObjectComponent<Rigidbody2D>("Background");
        if (rb == null) return false;

        return rb.bodyType.Equals(RigidbodyType2D.Static);
    }
    public bool D7TilemapCollider2D()
    {
        return CommonTutorialCallbacks.GameObjectComponent<TilemapCollider2D>("Decorations");
        //return GetCompositeColliderComponent<TilemapCollider2D>("Decorations");
    }

    //f
    public bool F1LightsTile()
    {
        return AssetDatabase.LoadAssetAtPath<AnimatedTile>("Assets/Tiles/Lights.asset") != null;
    }
    public bool F1LightsTileCount()
    {
        var tile = AssetDatabase.LoadAssetAtPath<AnimatedTile>("Assets/Tiles/Lights.asset");
        if (tile == null) return false;

        return tile.m_AnimatedSprites.Length == 2;
    }
    public bool F1LightsTileSprites()
    {
        var tile = AssetDatabase.LoadAssetAtPath<AnimatedTile>("Assets/Tiles/Lights.asset");
        if (tile == null) return false;

        return tile.m_AnimatedSprites.Length == 2 &&
            tile.m_AnimatedSprites[0] != null && tile.m_AnimatedSprites[0].name.Equals("LightsMap_0") &&
            tile.m_AnimatedSprites[1] != null && tile.m_AnimatedSprites[1].name.Equals("LightsMap_1");
    }
}
#endif