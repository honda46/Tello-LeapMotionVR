using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System;

public class LeapPositionGet : MonoBehaviour
{
    public float hand;

    public double ThetaUP;
    public double ThetaLR;

    private Controller controller;
    private Finger[] fingers;
    private Hand[] hands;

    void Start()
    {
        controller = new Controller();
        hands = new Hand[2];
        fingers = new Finger[5];

        hand = 1000;

    }

    void Update()
    {
        float test = 4;
        Frame frame = controller.Frame();
        
        float left_index_d = frame.Hands[0].Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL).Center.x;
        float right_index_m = frame.Hands[1].Fingers[1].Bone(Bone.BoneType.TYPE_METACARPAL).Center.x;

        float p = frame.Hands[0].Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL).Center.x;
        float q = frame.Hands[0].Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL).Center.y;
        float r = frame.Hands[0].Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL).Center.z;

        float x = frame.Hands[0].Fingers[1].Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint.x;
        float y = frame.Hands[0].Fingers[1].Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint.y;
        float z = frame.Hands[0].Fingers[1].Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint.z;

        double Lud = Math.Sqrt((p - x) * (p - x) + (q - y) * (q - y) + (r - z) * (r - z));
        double Dud = Math.Sqrt((x - p) * (x - p) + (z - r) * (z - r));
        double Llr = Dud;
        double Dlr = (z - r);
        double thetaUP = Math.Acos(Dud / Lud);
        double thetaLR = Math.Acos(Dlr / Llr);
        if (q < y) thetaUP *= -1;
        if (p - x < 0) thetaLR *= -1;
        //test = left_index_d - right_index_m;
        if (test < 0)
        {
            test *= -1;
        }
        hand = test;
        thetaUP = thetaUP * 180 / 3.14159265359;
        thetaLR = thetaLR * 180 / 3.14159265359;
        ThetaUP = thetaUP;
        ThetaLR = thetaLR;
        //Debug.Log(thetaUP);
        Debug.Log(thetaLR);
    }
}