using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourchHolding : MonoBehaviour
{
/*
    [SerializeField] LayerMask hitLayers;
    [SerializeField] Transform holdingHand;
    [SerializeField] Transform lightSource;
    [SerializeField] float adjustSpeed = 12;
    [Range(0,1)] [SerializeField] float lightSourceYPos = 0.5f; 
    [Range(0,1)] [SerializeField] float wallDis = .2f;
    [Range(0,1)] [SerializeField] float max_posWeight = 0.4f;
    float posWeight;
    [Range(0,1)] [SerializeField] float max_layerWeight = 0.6f;
    [Range(0,1)] [SerializeField] float lamp_forwardMultiplier = 1.5f;

    [SerializeField] float rayLength = 1.3f; 
    [SerializeField] Vector3 handOffset;
    [SerializeField] Transform forwardRef;

    [Header("Requirements")]
    [SerializeField] GameObject handTourch;

    [Header("Requirements")]
    [SerializeField] AudioClip burnAudio;
    [SerializeField] AudioSource audioSource;

    Animator anim;
    Vector3 finalHandPos;
    bool hasTourch = false;
    bool tourchOnb = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        posWeight = max_posWeight;
        anim.SetLayerWeight(1, max_layerWeight);
        TourchOff;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyeCode.G) && hasTourch){
            if(tourchOnb) TourchOff(); else tourchOn();
        }
        float cur_layerWeight = anim.GetLayerWeight(1);
        if(tourchOnb){
            if(cur_layerWeight != max_layerWeight) anim.SetLayerWeight(1, Mathf.Lerp(cur_layerWeight, max_layerWeight, Time.deltaTime *4));
            if(posWeight != max_posWeight) posWeight = Mathf.Lerp(posWeight, max_posWeight, Time.deltaTime *4);
        }
        else{
            if(cur_layerWeight != 0) anim.SetLayerWeight(1, Mathf.Lerp(cur_layerWeight, 0, Time.deltaTime *4));
            if(posWeight != 0) posWeight = Mathf.Lerp(posWeight, 0, Time.deltaTime *4);
        }
    }

    void tourchOn(){
        audioSource.PlayOneShot(burnAudio);
        handTourch.SetActive(true);
        tourchOn = true;
    }

    void TourchOff(){
        audioSource.PlayOneShot(burnAudio);
        handTourch.SetActive(false);
        tourchOn = false;
    }
    */

/*
    void OnAnimatorIK(){
        if(!hasTourch || !tourchOn) return;
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHAnd, posWeight);
        CheckHandCollision();
        anim.SetIKPosition(AvatarIKGoal.leftHand, finalHandPos);
    }

    void CheckHandCollision(){
        Vector3 desirdeHandPos = forwardRef.position + (Vector3.up * handOffset.y) + (forwardRef.forward * hand)
    }
*/
}
