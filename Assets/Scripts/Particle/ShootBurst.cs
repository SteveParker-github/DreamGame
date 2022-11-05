using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBurst : MonoBehaviour
 {
      void Update()
     {
         if (Input.GetMouseButtonDown(0))   //play shoot effect
         {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            ps.Play();
         }

         if (Input.GetMouseButton(1))   //play absorb effect
         {
            ParticleSystem ps = GameObject.Find("AbsorbParticle").GetComponent<ParticleSystem>();
            ps.Play();
         }
     }
 }
 