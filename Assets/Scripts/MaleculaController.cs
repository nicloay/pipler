using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public enum MaleculaType{
    Dot,
    Horizontal,
    Vertical,
    Cross,
    Square,
    Perimeter,
    Full
}


[ExecuteInEditMode]
public class MaleculaController : MonoBehaviour {
    public MaleculaType m_MaleculaType;
    public List<GameObject> Objects;
    public LayerMask        FieldMask;
    public FieldController  Field {get; set;}

    static bool[] DOT        = new bool[]
        { false, false, false
        , false, true , false
        , false, false, false};

    static bool[] HORIZONTAL = new bool[]   
        { false, false, false
        , true , false, true
        , false, false, false};

    static bool[] VERTICAL   = new bool[]  
        { false, true , false
        , false, false, false
        , false, true , false};

    static bool[] CROSS      = new bool[]  
        { false, true , false
        , true , false, true
        , false, true , false};

    static bool[] SQUARE     = new bool[]  
        { true , false, true
        , false, false, false
        , true , false, true};

    static bool[] BORDER     = new bool[]  
        { true , true , true
        , true , false, true
        , true , true , true};

    static bool[] FULL     = new bool[]  
        { true , true , true
        , true , true , true
        , true , true , true};
    

    static Dictionary<MaleculaType, bool[]> arrayByType = new Dictionary<MaleculaType, bool[]>(){
        {MaleculaType.Dot       , DOT},
        {MaleculaType.Horizontal, HORIZONTAL},
        {MaleculaType.Vertical  , VERTICAL},
        {MaleculaType.Cross     , CROSS},
        {MaleculaType.Square    , SQUARE},
        {MaleculaType.Perimeter    , BORDER},
        {MaleculaType.Full      , FULL}
    };

    SpawnerController[] spawners;

	// Use this for initialization
	void Start () {
        SyncObjects();
        spawners = GetComponentsInChildren<SpawnerController>();
	}
		
	void Update () {        
        SyncObjects();
	}

    MaleculaType previousMaleculaType;

    void SyncObjects() {
        bool[] array = arrayByType[m_MaleculaType];
        for (int i = 0; i < array.Length; i++)
        {
            Objects[i].SetActive(array[i]);
        }


        previousMaleculaType = m_MaleculaType;
    }

    public void MergeWithNewField(MovementType lastMovementType){
        FindNewFieldController().MergeWithNewMaleculaType(m_MaleculaType, lastMovementType);
        Field.DestroyMalecula();
    }

    FieldController FindNewFieldController(){
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position - Vector3.up * 50, Vector3.up, out hitInfo, float.PositiveInfinity, FieldMask)){
            return hitInfo.collider.GetComponent<FieldController>();
        }
        return null;
    }

    public void AssigneMaleculaToTheFieldLocatedAt(){
        FieldController newField = FindNewFieldController();
        newField.AdoptNewMaleculaObject(this);
    }


    [ContextMenu("SetRandomType")]
    public void SetRandomType(){
        m_MaleculaType = (MaleculaType) Random.Range(0, 5);
    }



}
