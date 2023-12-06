using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace OctopusController
{
  
    public class MyScorpionController
    {
        class TailTargetStuff
        {
            public MyTentacleController tail;
            public Transform endEffector, target;
            readonly Vector3 rotateAxisJoint0 = Vector3.forward, rotateAxisOtherJoints = Vector3.right;
            readonly float distanceThreshold = 0.01f, correctionVelocity = 5;
            readonly int iterations = 20;
            public void Update()
            {
                for (int j = 0; j < iterations; j++)
                    if (Vector3.Distance(endEffector.position, target.position) > distanceThreshold)
                    {
                        for (int i = 0; i < tail.Bones.Length; i++)
                        {
                            Vector3 rotateAxis;
                            if (i == 0)
                                rotateAxis = rotateAxisJoint0;
                            else
                                rotateAxis = rotateAxisOtherJoints;

                            float slope = CalculateSlope(tail.Bones[i], rotateAxis);
                            tail.Bones[i].Rotate(rotateAxis * -slope * correctionVelocity);
                        }
                    }

            }

            float CalculateSlope(Transform bone, Vector3 rotateAxis)
            {
                float deltaTheta = 0.01f;

                float distance1 = Vector3.Distance(endEffector.position, target.position);
                bone.Rotate(rotateAxis * deltaTheta);

                float distance2 = Vector3.Distance(endEffector.position, target.position);
                bone.Rotate(rotateAxis * -deltaTheta);

                return (distance2 - distance1) / deltaTheta;
            }
        }

        TailTargetStuff _tailStuff;

        bool tailLoop = false;
        float thresholdToActivateTail = 2.5f;

        //TAIL
        Transform tailTarget;
        Transform tailEndEffector;
        MyTentacleController _tail;
        float animationRange;

        //LEGS
        Transform[] legTargets;
        Transform[] legFutureBases;
        MyTentacleController[] _legs = new MyTentacleController[6]; 

        
        #region public
        public void InitLegs(Transform[] LegRoots,Transform[] LegFutureBases, Transform[] LegTargets)
        {
            _legs = new MyTentacleController[LegRoots.Length];
            //Legs init
            for(int i = 0; i < LegRoots.Length; i++)
            {
                _legs[i] = new MyTentacleController();
                _legs[i].LoadTentacleJoints(LegRoots[i], TentacleMode.LEG);
                //TODO: initialize anything needed for the FABRIK implementation
            }

        }

        public void InitTail(Transform TailBase)
        {
            _tail = new MyTentacleController();
            _tail.LoadTentacleJoints(TailBase, TentacleMode.TAIL);
            //TODO: Initialize anything needed for the Gradient Descent implementation
            tailEndEffector = _tail._endEffectorSphere;
            tailTarget = GameObject.Find("Ball").transform;
        }

        //TODO: Check when to start the animation towards target and implement Gradient Descent method to move the joints.
        public void NotifyTailTarget(Transform target)
        {
            _tailStuff = new TailTargetStuff();
            _tailStuff.tail = _tail;
            _tailStuff.target = tailTarget;
            _tailStuff.endEffector = tailEndEffector;
        }

        //TODO: Notifies the start of the walking animation
        public void NotifyStartWalk()
        {

        }

        //TODO: create the apropiate animations and update the IK from the legs and tail

        public void UpdateIK()
        {
            tailLoop = Vector3.Distance(_tailStuff.tail.Bones[0].GetComponentInParent<Transform>().position, _tailStuff.target.position) <= thresholdToActivateTail;

            if (tailLoop)
            {
                updateTail();
            }
        }
        #endregion


        #region private
        //TODO: Implement the leg base animations and logic
        private void updateLegPos()
        {
            //check for the distance to the futureBase, then if it's too far away start moving the leg towards the future base position
            //
        }
        //TODO: implement Gradient Descent method to move tail if necessary
        private void updateTail()
        {
            _tailStuff.Update();
        }
        //TODO: implement fabrik method to move legs 
        private void updateLegs()
        {

        }
        #endregion
    }
}
