using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.WitAi;


public class WitActivation : MonoBehaviour
{
   [SerializeField] private Wit wit;

   private void OnValidate()
   {
       if (!wit) wit = GetComponent<Wit>();
   }


void Start(){
}

   void Update()
   {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            Debug.Log("Started Listening");
            wit.Activate();
        }
       if (OVRInput.GetDown(OVRInput.Button.One)){
            Debug.Log("Stopped Listening");
            wit.Activate();
        }
      
   }

   void LogResponse() {
       Debug.Log("Got Response");
   }
}