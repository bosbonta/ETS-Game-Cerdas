using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Tester : Agent
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin(){
        transform.localPosition = new Vector3(Random.Range(-2f,2f), 0, Random.Range(-2f,2f));
        targetTransform.localPosition = new Vector3(Random.Range(-2f,2f), 0, Random.Range(2f,2f));
    }
    public override void CollectObservations(VectorSensor sensor){
        //observation for agent position
        sensor.AddObservation(transform.localPosition);

         //observation for targent position
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 1f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }


    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter(Collider other){
        if(other.TryGetComponent<Builder>(out Builder builder)){
         SetReward(+1f);
         floorMeshRenderer.material = winMaterial;
        EndEpisode();
        }
        if(other.TryGetComponent<Wal>(out Wal wal)){
         SetReward(-1f);
         floorMeshRenderer.material = loseMaterial;
        EndEpisode();
        }
  
    }
}
