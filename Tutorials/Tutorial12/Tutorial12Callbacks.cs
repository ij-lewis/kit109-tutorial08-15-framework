using UnityEngine;
using UnityEditor;
using Unity.Tutorials.Core.Editor;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor.SceneManagement;

/// <summary>
/// Implement your Tutorial callbacks here.
/// </summary>
[CreateAssetMenu(fileName = DefaultFileName, menuName = "Tutorials/" + DefaultFileName + " Instance")]
public class Tutorial12Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial12Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial12Callbacks>(DefaultFileName);
    }


    // Ian's callbacks
    // I hate everything about most of these :)


    public static bool playPressedThisTutorialPage = false;

    public void ResetPlayDetection()
    {
        //Debug.Log("Resetting Play Detector");
        playPressedThisTutorialPage = false;
    }

    public void DetectPlayBeingPressed()
    {
        if (Application.isPlaying)
        {
            //Debug.Log("Detected Play Mode");
            playPressedThisTutorialPage = true;
        }
    }

    public bool HasPlayBeenPressed()
    {
        return playPressedThisTutorialPage;
    }

    public void SaveScene()
    {
        //EditorApplication.SaveScene();
        bool saveOK = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }



    public bool DoesGameObjectContainsScriptByName(string script, string name) //for use when script doesnt exist in base
    {
        var go = GameObject.Find(name);
        if (go == null) return false;

        return go.GetComponent(script) != null;
    }


    //TODO: need same to check for enemyPrefab variable
    //TODO: check that Start() function calls SpawnEnemy? Can we do that?
    //TODO: ensure ComplexAlmostHumanLikeAI prefab is connected to field

    //TODO: should we check access modifier / params / return type ?
    //TODO: do we need one of these for statics
    public bool DoesScriptContainMethod(string gameObjectName, string scriptName, string methodName) //TODO: so don't need the gameObjectName here or on heaps of others -- I was learning :P
    {
        GameObject spawner = GameObject.Find(gameObjectName);

        if (spawner)
        {
            Component mb = spawner.GetComponent(scriptName);

            if (mb)
            {
                var method = mb.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

                if (method != null) return true;
            }
        }

        return false;
    }

    // fieldTypeName has to be horrible, with full package, and also assembly
    //UnityEngine.GameObject
    //System.Single ----- (float)
    //System.Int32 ----- (int)
    public bool DoesScriptContainField(string gameObjectName, string scriptName, string fieldName, string fieldTypeName)
    {
        GameObject spawner = GameObject.Find(gameObjectName);

        if (spawner)
        {
            //Debug.Log("Found: " + gameObjectName);
            Component mb = spawner.GetComponent(scriptName);

            if (mb)
            {
                //Debug.Log("Found: " + mb);
                var field = mb.GetType().GetField(fieldName);

                if (field != null)
                {
                    //Debug.Log("Found: " + field);

                    var fieldType = field.FieldType; // this doesn't work for some reason: .GetType(); (returns MonoType)

                    //Debug.Log(fieldType + " " + Type.GetType(fieldTypeName));

                    if (fieldType.ToString() == fieldTypeName) return true;
                }
            }
        }

        return false;
    }

    // fieldTypeName has to be horrible, with full package, and also assembly
    //UnityEngine.GameObject
    //System.Single ----- (float)
    //System.Int32 ----- (int)
    public static T GetValueOfFieldOnComponentByName<T>(string gameObjectName, string scriptName, string fieldName)
    {
        GameObject spawner = GameObject.Find(gameObjectName);

        if (spawner)
        {
            Component mb = spawner.GetComponent(scriptName);

            if (mb)
            {
                //Debug.Log(string.Join("\n", mb.GetType().GetFields().ToList().Select(f => f.Name)));
                var field = mb.GetType().GetField(fieldName); // this string has to be horrible

                if (field != null)
                {
                    var value = (T)field.GetValue(mb);
                    return value;
                }
            }
        }

        return default;
    }

    public bool DoesScriptNotContainField(string gameObjectName, string scriptName, string fieldName, string fieldTypeName)
    {
        GameObject go = GameObject.Find(gameObjectName);

        if (go)
        {
            //Debug.Log("Found: " + gameObjectName);
            Component mb = go.GetComponent(scriptName);

            if (mb)
            {
                //Debug.Log("Found: " + mb);
                var field = mb.GetType().GetField(fieldName);

                if (field == null) return true;
            }
        }

        return false;
    }

    public bool DoesMethodContainMethodCall(string gameObjectName, string scriptName, string methodName, string methodNameToCall)
    {
        // https://www.nuget.org/packages/Mono.Reflection/2.0.0
        return true; //TODO: parse the code / use Mono.Reflection to actually check this
    }

    public bool DoesArrayFieldMatchLength(string gameObjectName, string scriptName, string fieldName, int expectedLength)
    {
        GameObject spawner = GameObject.Find(gameObjectName);

        if (spawner)
        {
            Component mb = spawner.GetComponent(scriptName);
            //BasicSpawner mb = spawner.GetComponent<BasicSpawner>(); //TODO: OH SHIT< THIS CHANGE IS UNTESTED

            if (mb)
            {
                var field = mb.GetType().GetField(fieldName); // this string has to be horrible

                if (field != null)
                {
                    var array = (Array)field.GetValue(mb);

                    if (array != null)
                    {

                        var value = array.Length;

                        return (value == expectedLength);
                    }
                }
            }
        }

        return false;
    }

    public bool DoesFieldContainValue(string gameObjectName, string scriptName, string fieldName, int expectedValue)
    {
        GameObject spawner = GameObject.Find(gameObjectName);

        if (spawner)
        {
            Component mb = spawner.GetComponent(scriptName);
            //BasicSpawner mb = spawner.GetComponent<BasicSpawner>(); //TODO: OH SHIT< THIS CHANGE IS UNTESTED

            if (mb)
            {
                var field = mb.GetType().GetField(fieldName); // this string has to be horrible

                if (field != null)
                {
                    var value = (int) field.GetValue(mb);

                    return (value == expectedValue);
                }
            }
        }

        return false;
    }

    public bool DoesFieldContainAsset(string gameObjectName, string scriptName, string fieldName, string assetName)
    {
        GameObject spawner = GameObject.Find(gameObjectName);

        if (spawner)
        {
            Component mb = spawner.GetComponent(scriptName);
            //BasicSpawner mb = spawner.GetComponent<BasicSpawner>(); //TODO: OH SHIT< THIS CHANGE IS UNTESTED

            if (mb)
            {
                var field = mb.GetType().GetField(fieldName); // this string has to be horrible

                if (field != null)
                {
                    var value = field.GetValue(mb);

                    string[] assets = AssetDatabase.FindAssets(assetName);

                    if (value != null && assets.Length > 0)
                    {
                        var assetPath = AssetDatabase.GUIDToAssetPath(assets[0]);
                        //Debug.Log("PATH:" + assetPath);
                        var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

                        //Debug.Log(fieldName + " Value: " + value.ToString() + " -- " + asset.ToString());

                        if (((System.Object) value) == asset) // fuck me, why do I need this cast C# :O
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool DoesArrayElementContainAsset(string gameObjectName, string scriptName, string fieldNameAndIndex, string assetName)
    {
        var parts = fieldNameAndIndex.Split(' ');

        //Debug.Log("PARTS: " + parts.Length);

        if (parts.Length != 2) return false;

        string fieldName = parts[0];
        int index = int.Parse(parts[1]);

        //Debug.Log("VALUES: " + fieldName + "[" + index);

        GameObject spawner = GameObject.Find(gameObjectName);

        if (spawner)
        {
            Component mb = spawner.GetComponent(scriptName);
            //BasicSpawner mb = spawner.GetComponent<BasicSpawner>(); //TODO: OH SHIT< THIS CHANGE IS UNTESTED

            if (mb)
            {
                var field = mb.GetType().GetField(fieldName); // this string has to be horrible

                if (field != null)
                {
                    var array = (Array)field.GetValue(mb);

                    if (array != null && index < array.Length)
                    {
                        var value = array.GetValue(index);

                        string[] assets = AssetDatabase.FindAssets(assetName);
                        //Debug.Log(string.Join("\n", assets));

                        if (value != null && assets.Length > 0)
                        {
                            var assetPath = AssetDatabase.GUIDToAssetPath(assets[0]);
                            //Debug.Log("PATH:" + assetPath);
                            var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

                            if (((System.Object)value) == asset) // fuck me, why do I need this cast C# :O
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool DoesFieldContainSceneObject(string gameObjectName, string scriptName, string fieldName, string sceneObjectName)
    {
        GameObject spawner = GameObject.Find(gameObjectName);

        if (spawner)
        {
            Component mb = spawner.GetComponent(scriptName);
            //BasicSpawner mb = spawner.GetComponent<BasicSpawner>();

            if (mb)
            {
                var field = mb.GetType().GetField(fieldName); // this string has to be horrible

                if (field != null)
                {
                    var value = field.GetValue(mb);

                    var sceneObject = GameObject.Find(sceneObjectName);

                    if (value != null && sceneObject)
                    {
                        Component valueAsComponent = (Component)value;

                        //Debug.Log(fieldName + " Value: " + value.ToString() + " -- " + sceneObject.ToString());
                        // Sorry, ran out of time here, need to cast the value to a MonoBehaviour/Component, and get what Gameobject it's on

                        //if (((System.Object)value) == sceneObject) // fuck me, why do I need this cast C# :O
                        if (valueAsComponent && valueAsComponent.gameObject == sceneObject) // fuck me, why do I need this cast C# :O
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }


    //public bool ForceGameWindowToFront()
    //{
    //    EditorApplication.ExecuteMenuItem("Window/General/Game");
    //    //EditorWindow.FocusWindowIfItsOpen(Type.GetType("UnityEditor.GameView"));
    //    Application.isFocused 
    //}


    // End Ian's callbacks





    //b
    public bool B1_OldEnemies()
    {
        return
            CommonTutorialCallbacks.GameObjectsStartingWith("Seek").Count == 0 &&
            CommonTutorialCallbacks.GameObjectsStartingWith("Flee").Count == 0 &&
            CommonTutorialCallbacks.GameObjectsStartingWith("Purs").Count == 0 &&
            CommonTutorialCallbacks.GameObjectsStartingWith("Wand").Count == 0 &&
            CommonTutorialCallbacks.GameObjectsStartingWith("Complex").Count == 0;
    }
    public bool B2_BasicSpawnerOnSpawner()
    {
        return CommonTutorialCallbacks.GameObjectContainsScriptByName("BasicSpawner", "Spawner");
    }

    //step 23
    Vector3 initialPos;
    public void SetBallInitialPos()
    {
        initialPos = GameObject.Find("Ball").transform.position;
    }
    public bool BallMovedDistance()
    {
        return (GameObject.Find("Ball").transform.position - initialPos).magnitude > 1;
    }

    //step 25
    Color initialColor;
    bool initialFlip;
    bool initialFlipY;
    public void SetBallInitialSpriteVals()
    {
        var sr = GameObject.Find("Ball").GetComponent<SpriteRenderer>();
        initialColor = sr.color;
        initialFlip = sr.flipX;
        initialFlipY = sr.flipY;
    }
    public bool SpriteModified()
    {
        var sr = GameObject.Find("Ball").GetComponent<SpriteRenderer>();
        return initialColor.Equals(sr.color) == false && (initialFlip != sr.flipX || initialFlipY != sr.flipY);
    }

    //step 26
    public bool BallCount(int requiredCount)
    {
        var all = GameObject.FindObjectsByType <GameObject>(FindObjectsSortMode.None);
        int ballCount = 0;
        for (int i=0; i<all.Length; i++) {
            if (all[i].name.StartsWith("Ball")) {
                ballCount++;
            }
        }
        return ballCount >= requiredCount;
    }

    //step 27
    public bool AllBallsGreen()
    {
        var all = GameObject.FindObjectsByType <GameObject>(FindObjectsSortMode.None);
        Debug.Log(all.Length);
        for (int i=0; i<all.Length; i++) {
            if (all[i].name.StartsWith("Ball")) {
                var sr = all[i].GetComponent<SpriteRenderer>();
                Debug.Log(sr.color);
                if (sr.color.Equals(Color.green) == false) return false;
            }
        }
        return true;
    }
    //step 29
    public bool AtLeastOneRigidbody()
    {
        var all = GameObject.FindObjectsByType <Rigidbody2D>(FindObjectsSortMode.None);
        return all.Length > 0; 
    }
    //step 36
    public bool AllBallsHaveCircleColliders() 
    {
        var all = GameObject.FindObjectsByType <GameObject>(FindObjectsSortMode.None);
        for (int i=0; i<all.Length; i++) {
            if (all[i].name.StartsWith("Ball")) {
                var c = all[i].GetComponent<CircleCollider2D>();
                if (c == null) return false;
            }
        }
        return true;
    }

    //step 41
    //public bool AtLeastOneWrapAround() 
    //{
    //    var all = GameObject.FindObjectsOfType <GameObject>();
    //    for (int i=0; i<all.Length; i++) {
    //        if (all[i].name.StartsWith("Ball")) {
    //            var c = all[i].GetComponent<WrapAround>();
    //            if (c != null) return true;
    //        }
    //    }
    //    return false;
    //}


    // Implement the logic to automatically complete the criterion here, if wanted/needed.
    public bool AutoComplete()
    {
        var foo = GameObject.Find("Foo");
        if (!foo)
            foo = new GameObject("Foo");
        return foo != null;
    }

    //I
    public bool I1_ReplaceComponent()
    {
        var spawner = GameObject.Find("Spawner");
        if (spawner == null) return false;

        var moveNormal = spawner.GetComponent("MoveBetweenTwoPoints");
        var moveCurve = spawner.GetComponent("MoveBetweenTwoPointsCurve");

        return
            moveNormal == null &&
            moveCurve != null &&
            DoesFieldContainSceneObject("Spawner", "MoveBetweenTwoPointsCurve", "start", "SpawnerStart") &&
            DoesFieldContainSceneObject("Spawner", "MoveBetweenTwoPointsCurve", "end", "SpawnerEnd");

    }
    public bool I2_CurvePoints()
    {
        var curve = GetValueOfFieldOnComponentByName<AnimationCurve>("Spawner", "MoveBetweenTwoPointsCurve", "curve");
        //Debug.Log(curve);
        if (curve == null) return false;

        //Debug.Log(string.Join("|", curve.keys.ToList().Select(k => k.time + "," + k.value + "," + k.inTangent + "," + k.inWeight + "," + k.outTangent + "," + k.outWeight)));
        //0,0,0,0,0,0|1,1,0,0,0,0

        if (curve.keys.Count() != 2) return false;

        return
            curve.keys[0].time.Equals(0) &&
            curve.keys[0].value.Equals(0) &&
            /*curve.keys[0].inTangent.Equals(0) && //cbf checking these, even tho they wil just be zeros
            curve.keys[0].inWeight.Equals(0) &&
            curve.keys[0].outTangent.Equals(0) &&
            curve.keys[0].outWeight.Equals(0) &&*/
            curve.keys[1].time.Equals(1) &&
            curve.keys[1].value.Equals(1);/* &&
            curve.keys[1].inTangent.Equals(0) &&
            curve.keys[1].inWeight.Equals(0) &&
            curve.keys[1].outTangent.Equals(0) &&
            curve.keys[1].outWeight.Equals(0) &&*/
    }
    public bool I6_CurveLoop()
    {
        var curve = GetValueOfFieldOnComponentByName<AnimationCurve>("Spawner", "MoveBetweenTwoPointsCurve", "curve");
        if (curve == null) return false;

        return curve.postWrapMode.Equals(WrapMode.Loop);
    }
    public bool I6_CurvePingPong()
    {
        var curve = GetValueOfFieldOnComponentByName<AnimationCurve>("Spawner", "MoveBetweenTwoPointsCurve", "curve");
        if (curve == null) return false;

        return curve.postWrapMode.Equals(WrapMode.PingPong);
    }
}
