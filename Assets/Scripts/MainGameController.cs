using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour {
    public static MainGameController Instance;
    public GameObject MaleculaPrefab;
    public List<FieldController> Fields;

    void Awake(){
        Instance = this;
    }

    void Start(){
        Fields.ForEach((obj) => obj.SpawnRandomMalecula()) ;   
    }


    static Dictionary<MaleculaType, HashSet<MaleculaType>> maleculaCompatibility 
    = new Dictionary<MaleculaType, HashSet<MaleculaType>>(){
        {MaleculaType.Dot       , new HashSet<MaleculaType>(){MaleculaType.Perimeter       , MaleculaType.Dot      }},
        {MaleculaType.Horizontal, new HashSet<MaleculaType>(){MaleculaType.Horizontal   , MaleculaType.Vertical }},
        {MaleculaType.Vertical  , new HashSet<MaleculaType>(){MaleculaType.Horizontal   , MaleculaType.Vertical }},
        {MaleculaType.Cross     , new HashSet<MaleculaType>(){MaleculaType.Square                               }},
        {MaleculaType.Square    , new HashSet<MaleculaType>(){MaleculaType.Cross                                }},
        {MaleculaType.Perimeter    , new HashSet<MaleculaType>(){MaleculaType.Dot                                   }}
    };

    public static bool IsMaleculasCompatible(MaleculaType m1, MaleculaType m2){
        return maleculaCompatibility[m1].Contains(m2);
    }


    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
