using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldController))]
public class SpawnerController : MonoBehaviour {
    FieldController fc;

    void Start(){
        fc = GetComponent<FieldController>();
    }
	
    void Update(){
        SpawnRandomIfNull();
        HideMaleculaIfNotMoved();
    }

    void HideMaleculaIfNotMoved()
    {
        bool isMaleculaVisible = fc.Malecula.transform.localPosition.magnitude > MaleculaDefaulMoveController.MAX_MOVE_FOR_WRONG_ACTION.magnitude;
        fc.Malecula.gameObject.SetActive(isMaleculaVisible);
    }

    public void SpawnRandomIfNull(){
        if (fc.Malecula == null){
            fc.SpawnRandomMalecula();
        }
    }
}
