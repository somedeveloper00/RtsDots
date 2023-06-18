using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public class SpawnObjects : EditorWindow {
    [MenuItem( "Window/Spawn Objects" )]
    static void ShowWindow() {
        var window = GetWindow<SpawnObjects>();
        window.titleContent = new GUIContent( "Spawn Objects" );
        window.Show();
    }

    [SerializeField] GameObject parent;
    [SerializeField] GameObject reference;
    [Tooltip("Use {0} for reference name and {1} for index")]
    [SerializeField] string nameFormat = "{0} {1}";
    [SerializeField] int count = 10;
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 area;
    BoxBoundsHandle _boxHandle = new BoxBoundsHandle();


    void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    void OnDestroy() => SceneView.duringSceneGui -= OnSceneGUI;
    

    void OnSceneGUI(SceneView obj) {
        using (var check = new EditorGUI.ChangeCheckScope()) {
            position = Handles.PositionHandle( position, Quaternion.identity );
            if (check.changed) Repaint();
        }
        using (var check = new EditorGUI.ChangeCheckScope()) {
            _boxHandle.center = position;
            _boxHandle.size = area;
            _boxHandle.DrawHandle();

            if (check.changed) {
                position = _boxHandle.center;
                area = _boxHandle.size;
                Repaint();
            }
        }
    }
    
    void OnGUI() {
        reference = (GameObject)EditorGUILayout.ObjectField( "To Instantiate", reference, typeof( GameObject ), true );
        nameFormat = EditorGUILayout.TextField( "Name Format", nameFormat );
        EditorGUILayout.LabelField( "would look like", GetName( nameFormat, 3 ) );
        parent = (GameObject)EditorGUILayout.ObjectField( "Parent", parent, typeof( GameObject ), true );
        count = EditorGUILayout.IntField( "Count", count );
        position = EditorGUILayout.Vector3Field( "Position", position );
        area = EditorGUILayout.Vector3Field( "Area", area );
        
        if ( GUILayout.Button( "Spawn" ) ) {
            var spawned = new Object[count];
            for (int i = 0; i < count; i++) {
                var obj = SafeInstantiate( reference );
                if (parent) obj.transform.SetParent( parent.transform );
                obj.name = GetName( nameFormat, i );
                var pos = position + new Vector3( Random.Range( -area.x, area.x ), Random.Range( -area.y, area.y ), Random.Range( -area.z, area.z ) ) / 2f;
                obj.transform.position = pos;
                spawned[i] = obj;
                Undo.RegisterCreatedObjectUndo( obj, "Spawn Objects" );
            }
            Selection.objects = spawned;
        }
        var scene = SceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty( scene );
    }

    string GetName(string format, int index) =>
        !reference ? format : format.Replace( "{0}", reference.name ).Replace( "{1}", index.ToString() );

    static GameObject SafeInstantiate(GameObject gameObject) {
        if (PrefabUtility.IsPartOfPrefabAsset( gameObject )) {
            return (GameObject)PrefabUtility.InstantiatePrefab( gameObject );
        }
        return Instantiate( gameObject );
    }
}