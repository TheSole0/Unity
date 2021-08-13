using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SaveFile : Agent
{
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }


    //에피소드 시작시 포지션 초기화
    public Transform Target;
    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        Target.localPosition = new Vector3(Random.value * 8 - 4, 12f, Random.value * 8 - 4);
    }
    //강화학습 프로그램에게 관측정보를 전달
    public override void CollectObservations(VectorSensor sensor)
    {
        //타겟과 에이전트의 포지션을 전달
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        //현재 에이전트의 이동량을 전달
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.y);

    }

    //강화학습을 위한, 통한 행동을 결정
    public float force = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //학습된 정보를 바탕으로 이동

        Vector3 controSignal = Vector3.zero;
        controSignal.x = actionBuffers.ContinuousActions[0];
        controSignal.z = actionBuffers.ContinuousActions[1];

        rBody.AddForce(controSignal * force);



        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }

    }
}
