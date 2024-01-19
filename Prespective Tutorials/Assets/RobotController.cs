using System.Collections;
using System.Collections.Generic;
using u040.prespective.referenceobjects.kinetics.motor.servomotor;
using u040.prespective.referenceobjects.materialhandling.gripper;
using UnityEngine;

public class RobotController : MonoBehaviour
{


    public DrivenServoMotor Piece1;
    public DrivenServoMotor Piece2;
    public DrivenServoMotor Piece3;
    public DrivenServoMotor Piece4;
    public DrivenServoMotor Base;
    public GripperBase Gripper;


    public bool ToDropoff = false;
    public bool ToPickup = false;

    public void GoToPickup()
    {
        if (ToPickup && IsValidSetup())
        {
            ToPickup = false;
            Piece1.TargetAngle = 45.30f;
            Piece2.TargetAngle = 62.63f;
            Piece3.TargetAngle = 87.90f;
            Piece4.TargetAngle = 29.45f;
            Base.TargetAngle = 45.45f;
        }
    }

    public void GoToDropoff()
    {
        if (ToDropoff && IsValidSetup())
        {
            ToDropoff = false;
            Piece1.TargetAngle = -45.30f;
            Piece2.TargetAngle = -62.63f;
            Piece3.TargetAngle = -87.90f;
            Piece4.TargetAngle = -29.45f;
            Base.TargetAngle = -45.45f;
        }
    }

    public void OpenGripper()
    {
        if (Gripper)
        {
            Gripper.TargetClosePercentage = 0f;
        }
    }

    public void CloseGripper()
    {
        if (Gripper)
        {
            Gripper.TargetClosePercentage = 1f;
        }
    }

    public bool IsValidSetup()
    {
        //Validate Piece1
        if (Piece1 == null ||
            Piece1 == Piece2 ||
            Piece1 == Piece3 ||
            Piece1 == Piece4 ||
            Piece1 == Base ) 
        {
            return false;
        }

        //Validate Piece2
        if (Piece2 == null ||
            Piece2 == Piece3 ||
            Piece2 == Piece4 ||
            Piece2 == Base )
        {
            return false;
        }

        //Validate Piece3
        if (Piece3 == null ||
            Piece3 == Piece4 ||
            Piece3 == Base )
        {
            return false;
        }

        //Validate Piece4
        if (Piece4 == null ||
            Piece4 == Base )
        {
            return false;
        }

        //Validate Base
        if (Base == null)
        {
            return false;
        }

        //Validate Gripper
        if (Gripper == null)
        {
            return false;
        }

        return true;
    }


}
