using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class DroneAgent : Agent
{

    public Rigidbody rb;
    public Rigidbody motorfr;
    public Rigidbody motorfl;
    public Rigidbody motorrr;
    public Rigidbody motorrl;

    private Vector3 initPosition;
    private Quaternion initQuaternion;
    private float altitude = 50.0f;

    

    public Transform Target;

    public override void OnEpisodeBegin()
    {
        // 드론 위치 및 방향 초기화
        if (rb.transform.localPosition.y > 100)
        {
            



            // 모터 속도 초기화
            motorfr.velocity = Vector3.zero;
            motorfl.velocity = Vector3.zero;
            motorrr.velocity = Vector3.zero;
            motorrl.velocity = Vector3.zero;
            motorfr.angularVelocity = Vector3.zero;
            motorfl.angularVelocity = Vector3.zero;
            motorrr.angularVelocity = Vector3.zero;
            motorrl.angularVelocity = Vector3.zero;
            rb.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
            // 타겟위치 재설정
            Target.localPosition = new Vector3(Random.value * 40 - 20, 0.5f, Random.value * 40 - 20);
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 타겟과 드론 위치 관찰
        sensor.AddObservation(Target.localPosition + Vector3.up * altitude);
        sensor.AddObservation(rb.transform.localPosition);
        sensor.AddObservation(rb.transform.localRotation);

        // 드론의 속도 관찰
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(rb.velocity.z);
        sensor.AddObservation(rb.angularVelocity.x);
        sensor.AddObservation(rb.angularVelocity.y);
        sensor.AddObservation(rb.angularVelocity.z);

    }

    // 드론 모터 파워
    private float power = 20;

    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 4
        motorfr.AddRelativeForce(Vector3.up * (vectorAction[0] + 0.5f) * power);
        motorfl.AddRelativeForce(Vector3.up * (vectorAction[1] + 0.5f) * power);
        motorrr.AddRelativeForce(Vector3.up * (vectorAction[2] + 0.5f) * power);
        motorrl.AddRelativeForce(Vector3.up * (vectorAction[3] + 0.5f) * power);

        // Rewards
        float distanceToTarget = Vector3.Distance(rb.transform.localPosition, Target.localPosition + Vector3.up * altitude);

        // Reached target
        if (distanceToTarget < 0.3f)
        {
            SetReward(1.0f);
        }

        SetReward(-distanceToTarget / 10 + altitude - Mathf.Abs(altitude - rb.transform.localPosition.y));

        // Fell off platform
        if (distanceToTarget > 150.0f
            || (rb.transform.localRotation.eulerAngles.x > 30 && rb.transform.localRotation.eulerAngles.x < 330)
            || (rb.transform.localRotation.eulerAngles.z > 30 && rb.transform.localRotation.eulerAngles.z < 330)
            )
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }
}

