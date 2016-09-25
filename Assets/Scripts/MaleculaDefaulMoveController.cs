using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class MaleculaDefaulMoveController : MonoBehaviour {

    public static readonly Vector3 MAX_MOVE_FOR_WRONG_ACTION = new Vector3(0.15f, 0.15f, 0.15f);

    Vector3 mouseDownLocalPosition;
    void OnMouseDown(){
        mouseDownLocalPosition = Vector3.zero;
    }
    MovementType lastMovementType;
    void OnMouseDrag(){
        ClearMovedNeighbours();
        Vector3 newPosition = GetLocalPosition();
        Vector3 diff = newPosition - mouseDownLocalPosition;
        Vector3 movementVector;
        float normalValue;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z)){
            normalValue = Mathf.Abs( diff.x);
            movementVector = new Vector3(diff.x, 0f, 0f);
            lastMovementType = MovementType.Horizontal;
        } else {
            normalValue = Mathf.Abs(diff.z);
            movementVector = new Vector3(0f, 0f, diff.z);
            lastMovementType = MovementType.Vertical;
        }
        if (!IsMovePossible(movementVector)){
            ClampVector(ref movementVector, MAX_MOVE_FOR_WRONG_ACTION);
        } else {
            ClampVector(ref movementVector, Vector3.one);
        }
        DragNeighbours(movementVector);
        transform.localPosition =  movementVector;
    }

    List<MaleculaController> movedNeighbours = new List<MaleculaController>();

    void ClearMovedNeighbours(){
        movedNeighbours.ForEach((obj) => obj.transform.localPosition = Vector3.zero);
        movedNeighbours.Clear();
    }

    void DragNeighbours(Vector3 movementVector){
        movedNeighbours.Clear();
        FieldController field= GetComponentInParent<FieldController>();
        do{
            FieldController neighbour = field.GetNeighbourField(-movementVector);
            if (neighbour == null){
                break;
            } else {
                neighbour.Malecula.transform.localPosition = movementVector;
                movedNeighbours.Add(neighbour.Malecula);
            }
            field = neighbour;
        } while(field != null);
    }

    void ClampVector(ref Vector3 vector, Vector3 maxVector){
        vector.x = Mathf.Sign(vector.x) * Mathf.Min(Mathf.Abs(vector.x), maxVector.x);
        vector.y = Mathf.Sign(vector.y) * Mathf.Min(Mathf.Abs(vector.y), maxVector.y);
        vector.z = Mathf.Sign(vector.z) * Mathf.Min(Mathf.Abs(vector.z), maxVector.z);
    }

    bool IsMovePossible(Vector3 direction){
        FieldController fc = GetComponentInParent<FieldController>();
        FieldController neighbour = fc.GetNeighbourField(direction);
        if (neighbour == null){
            return false;
        }
        MaleculaType neighbourType = neighbour.Malecula.m_MaleculaType;
        bool isCompatible = MainGameController.IsMaleculasCompatible(GetComponent<MaleculaController>().m_MaleculaType, neighbourType);

        return isCompatible;
    }

    void OnMouseUp(){
        if (transform.localPosition.magnitude < 0.5f){
            transform.localPosition = Vector3.zero;
            ClearMovedNeighbours();
        } else {
            GetComponent<MaleculaController>().MergeWithNewField(lastMovementType);
            AssignMaleculasToNewPositions();
        }
    }


    void AssignMaleculasToNewPositions(){
        movedNeighbours.ForEach((obj) => obj.AssigneMaleculaToTheFieldLocatedAt());
        movedNeighbours.Clear();
    }

    Vector3 GetLocalPosition(){
        Plane Plane = new Plane(Vector3.up, transform.TransformPoint( Vector3.zero));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        Plane.Raycast(ray, out distance);
        Vector3 globalPoint = ray.GetPoint(distance);

        return transform.parent.transform.InverseTransformPoint(globalPoint);
    }
}
