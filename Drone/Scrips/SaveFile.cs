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


    //���Ǽҵ� ���۽� ������ �ʱ�ȭ
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
    //��ȭ�н� ���α׷����� ���������� ����
    public override void CollectObservations(VectorSensor sensor)
    {
        //Ÿ�ٰ� ������Ʈ�� �������� ����
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        //���� ������Ʈ�� �̵����� ����
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.y);

    }

    //��ȭ�н��� ����, ���� �ൿ�� ����
    public float force = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //�н��� ������ �������� �̵�

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
