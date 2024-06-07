using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadBornReport : MonoBehaviour
{
    [SerializeField] Image Img_Body;


    public void SetBodyColor(GameObject body)
    {
        gameObject.transform.localScale = Vector3.zero;
        Material material = Instantiate(Img_Body.material);
        material.SetColor("_PlayerColor", body.GetComponent<MeshRenderer>().material.color);
        Img_Body.material = material;
        GetComponent<DOTweenAnimation>().DORestart();
    }
}
