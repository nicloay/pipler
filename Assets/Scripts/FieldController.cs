using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public enum MovementType{
    Horizontal,
    Vertical
}

public class FieldController : MonoBehaviour {
    MaleculaController _malecula;
    public MaleculaController Malecula
    {
        get
        {
            return _malecula;
        }
        private set
        {
            _malecula = value;
            if (_malecula != null){                
                _malecula.Field = this;
            }
        }
    }
	
    void Update(){
        GivePointAndDestroyIfFull();
        SpawnRandomIfNull();
    }

    public void SpawnRandomIfNull(){
        if (Malecula == null){
            SpawnRandomMalecula();
        }
    }

    void GivePointAndDestroyIfFull(){
        if (Malecula != null && Malecula.m_MaleculaType == MaleculaType.Full){
            GameData.Instance.Point++;
            DestroyMalecula();
            SpawnRandomMalecula();
        }
    }

    public FieldController GetNeighbourField(Vector3 direction){
        RaycastHit hitInfo ;
        if (Physics.Raycast(transform.position, direction, out hitInfo, 100.0f, 1 << gameObject.layer)){
            return hitInfo.collider.GetComponent<FieldController>();
        }
        return null;
    }        

    public void MergeWithNewMaleculaType(MaleculaType newMaleculaType, MovementType movementType){        
        MaleculaType newType = MaleculaType.Dot;
        switch (Malecula.m_MaleculaType){
            case MaleculaType.Perimeter:
                newType = MaleculaType.Full;
                break;
            case MaleculaType.Cross:
                newType = MaleculaType.Perimeter;
                break;
            case MaleculaType.Dot:
                if (newMaleculaType == MaleculaType.Perimeter){
                    newType = MaleculaType.Full;
                } else {
                    if (movementType == MovementType.Horizontal){
                        newType = MaleculaType.Horizontal;
                    } else {
                        newType = MaleculaType.Vertical;
                    }                     
                }
                break;
            case MaleculaType.Horizontal:
                if (newMaleculaType == MaleculaType.Horizontal){
                    newType = MaleculaType.Square;
                } else {
                    newType = MaleculaType.Cross;
                }
                break;
            case MaleculaType.Vertical:
                if (newMaleculaType == MaleculaType.Horizontal){
                    newType = MaleculaType.Cross;
                } else {
                    newType = MaleculaType.Square;
                }
                break;
            case MaleculaType.Square:
                newType = MaleculaType.Perimeter;
                break;
        }
        SpawnMalecula(newType);

    }

    public void SpawnMalecula(MaleculaType maleculaType){
        SpawnMalecula();
        Malecula.m_MaleculaType = maleculaType;
    }

    public void SpawnRandomMalecula(){        
        SpawnMalecula();
        Malecula.SetRandomType();
    }


    public void AdoptNewMaleculaObject(MaleculaController newMalecula){
        Assert.IsNull(Malecula, "malecula must be null if new want to be reassigned");
        if (newMalecula.Field != null){            
            newMalecula.Field.Malecula = null;
        }
        Malecula = newMalecula;
        newMalecula.transform.SetParent(transform, true);
        newMalecula.transform.localPosition = Vector3.zero;
    }

    void SpawnMalecula(){
        if (Malecula != null){
            Destroy(Malecula.gameObject);
            Malecula = null;
        }
        GameObject maleculaGO = GameObject.Instantiate(MainGameController.Instance.MaleculaPrefab);
        maleculaGO.transform.SetParent(transform, true);
        maleculaGO.transform.localPosition = Vector3.zero;
        Malecula = maleculaGO.GetComponent<MaleculaController>();
    }

    public void DestroyMalecula(){
        Malecula.Field = null;
        Object.Destroy(Malecula.gameObject);
        Malecula = null;
    }
}
