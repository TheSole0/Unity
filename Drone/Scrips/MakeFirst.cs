using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MakeFirst : Agent
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
       
        
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 1, 0);
        

        Target.localPosition = new Vector3(Random.value * 8.0f - 4.0f, 10f, Random.value * 8.0f - 4.0f );
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
        sensor.AddObservation(rBody.velocity.z);
       

    }

    //��ȭ�н��� ����, ���� �ൿ�� ����
    public float force;
    public float thrust;
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controSignal = Vector3.zero;
        controSignal.x = actionBuffers.ContinuousActions[0];
        controSignal.y = actionBuffers.ContinuousActions[1];
        controSignal.z = actionBuffers.ContinuousActions[2];

        rBody.AddForce(controSignal * force);
        rBody.AddForce(transform.up * thrust);

        // Ÿ������ ���� ����
        float distanceToTarget = Vector3.Distance(this.transform.position, Target.localPosition);

        if (distanceToTarget < 1.0f)
        {
            SetReward(1.0f);
            EndEpisode();


        }
         else if(distanceToTarget < 3.0)
        {
         
          
            SetReward(0.5f);


        }
        else if(distanceToTarget > 150.0f)
        {

            SetReward(-0.1f);
            EndEpisode();
            


        }
        else if (rBody.transform.localPosition.y < 0)
        {
            SetReward(-0.05f);
            EndEpisode();
        }
     









    }
    



}

