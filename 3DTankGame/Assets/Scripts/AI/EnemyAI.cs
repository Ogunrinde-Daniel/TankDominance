using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Parameters 
{
    [Range(0, 10)] public float proximity;
    [Range(0, 10)] public float currentHealth;          
    [Range(0, 10)] public float currentlyAttackingMe;   //0 or 10
    [Range(0, 10)] public float currentlyUnderAttack;   //0 or 10

    public float proximityScale = 1;
    public float currentHealthScale = 1;
    public float currentlyAttackingMeScale = 1;
    public float currentlyUnderAttackScale = 1;


    public float getMaxSumOfValues()
    {
        float maxSum = (10 * proximityScale)
            + (10 * currentHealthScale)
            + (10 * currentlyAttackingMeScale)
            + (10 * currentlyUnderAttackScale);
        return maxSum;
    }
    public float getSumOfValues()
    {
        float sum = (proximity * proximityScale) 
            + (currentHealth * currentHealthScale) 
            + (currentlyAttackingMe * currentlyAttackingMeScale)
            + (currentlyUnderAttack * currentlyUnderAttackScale);

        return sum;
    }

}


public class EnemyAI : MonoBehaviour
{
    private AgentMovement agentMovement;
    private Movement movement;
    private Tank tank;

    [SerializeField] private List<Tank> opponentList;
    private List<Parameters> opponentParameterList;
    
    public LineRenderer lineR;
    public LayerMask opponentLayer;

    private int enemyRange = 17;             //how close to be before attacking, a shorter range gun wil require a shorter range
    private int whoToAttack = 0;            //index of who to attack
    private int previousWhoToAttack = -1;   //index of who the tank attacked in the last frame

    //the previous position of the tank in the last frame-used for rotation
    private Vector3 previousPos;

    [SerializeField] private WheelRotation lFront;
    [SerializeField] private WheelRotation rFront;    
    [SerializeField] private WheelRotation lback;
    [SerializeField] private WheelRotation rback;

    private void Start()
    {
        agentMovement = GetComponent<AgentMovement>();
        movement = GetComponent<Movement>();
        tank = GetComponent<Tank>();

        movement.lFront = lFront;
        movement.rFront = rFront;
        movement.lback = lback;
        movement.rback = rback;

        opponentParameterList = new List<Parameters>();
        for (int i = 0; i < opponentList.Count; i++)
        {
            opponentParameterList.Add(new Parameters());
        }
        previousPos = transform.position;
    }

    private void Update()
    {
        //remove objects that are destroyed / dead from the list
        TrimObjects();          
        if (opponentList.Count <= 0) return;

        //calculate who to attack next
        whoToAttack = CalculateTarget();

        //move towards whoToAttack
        bool stoppedMoving = MoveTowardsTargets();
        //rotate turret towards target
        tank.turret.GetComponent<TankTurret>().target = opponentList[whoToAttack].turret.transform;
        //if rotation is complete and we can see target
        if (tank.turret.GetComponent<TankTurret>().targetReached && IsSeeingTarget())
        {
            tank.Shoot();
        }

        movement.UpdateWheels(stoppedMoving);

        //set the previous who to to attack
        previousWhoToAttack = whoToAttack;
        previousPos = transform.position;
    }

    /*
     this method calculates the parameters for each enemy
     then uses a weighted random function to decide who to attack
     */
    private int CalculateTarget()
    {
        int targetIndex = 0;
        float highestValue = 0; //highest value for the sum of parameters
        for (int i = 0; i < opponentList.Count; i++)
        {
            //calculate parameters
            opponentParameterList[i].proximity = CalculateProxity(i);
            opponentParameterList[i].currentHealth = CalculateCurrentHealth(i);
            opponentParameterList[i].currentlyAttackingMe = CalculateCurrentlyAttackingMe(i);
            //opponentParameterList[i].currentlyUnderAttack = CalculateCurrentlyUnderAttack(i);
            
            //calculate the highest value
            if (opponentParameterList[i].getSumOfValues() > highestValue)
            {
                highestValue = opponentParameterList[i].getSumOfValues();
                targetIndex = i;
            }
        }
        return targetIndex;
    }

    /*
     this method move towards the target,
     it returns true after it has reached it's destination or stopped
     */
    private bool MoveTowardsTargets()
    {
        //check if we are in proper range of the enemy
        bool inEnemyRange = Vector3.Distance(transform.position, opponentList[whoToAttack].transform.position) < enemyRange;
        
        var whoToAttackPos = opponentList[whoToAttack].transform.position;

        //only move if we are changing target or not in enemy range 
        if (whoToAttack != previousWhoToAttack)
        {
            agentMovement.SetTarget(whoToAttackPos);
        }
                
        //stop movement and return true signifigying the agent has reached his destination
        if (IsSeeingTarget() && inEnemyRange){
            agentMovement.SetTarget(transform.position);
            //agentMovement.StopAgentMovement(true);
            return true;
        }else{//reset agent movement if not
            //agentMovement.StopAgentMovement(false);
            agentMovement.SetTarget(whoToAttackPos);
            return false;
        }
    }

    /*
     this method returns true if the tank can see the target's without any obstacles
     */
    private bool IsSeeingTarget()
    {
        // set the start point and direction of the raycast
        Vector3 startPoint = transform.position;
        //Vector2 direction = transform.right;
        Vector3 direction = opponentList[whoToAttack].transform.position - transform.position;


        // set the length of the raycast
        float raycastLength = 100f;

        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, raycastLength, opponentLayer))
        {

            if (hit.collider.gameObject == opponentList[whoToAttack].gameObject)
            {
                return true;
            }
        }
    
        return false;
    }

    /*
     this method generates a random number based on weights
     exp: 
         if I insert an array of 4 with weights {1,5,2,1} and I run the funtion 10 times, 
         technically, the first element will appear once, the sceond elment will appear 5 
         times and so on......
     v2 this version allows so the weights does not have to sum up to 1
     */
    public int generateWeightedRandomNumber(float[] weights)
    {
        //calculate the total weights sent in
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }
        //generate a random value between (0 and 1) and multiply by the total weight
        float randomValue = Random.value * totalWeight;
        for (int i = 0; i < weights.Length; i++)
        {
            //if the current weight is less than the total weight, return index
            //this works because the higher your weight, the more likely it is 
            //that you're greater than the generater number
            if (randomValue < weights[i])
            {
                return i;
            }
            randomValue -= weights[i];
        }

        return weights.Length - 1;
    }


    #region calculator Methods
    /*
     This function return 10 if the index passed in is currently under attack,
     returns 0 otherwise
     */
    private float CalculateCurrentlyUnderAttack(int index)
    {
        if (index == whoToAttack)
            return 10;
        return 0;
    }


    /*
     This function return 10 if the index passed in is currently attacking the AI,
     returns 0 otherwise
     */
    private float CalculateCurrentlyAttackingMe(int index)
    {
        float justAttacked = 0;
        var justAttackedList = GetComponent<Tank>().justAttackedMe.tankList;
        for (int i = 0; i < justAttackedList.Count; i++)
        {
            if (opponentList[index] == justAttackedList[i])
            {
                justAttacked = 10;
            }
        }
        return justAttacked;
    }

    /*
     This function return 10 if the index passed in has 0 health,
     returns 0 if the index has full health
     --the lower the index health, the more vulnerable it is(the higher the no)
     */
    private float CalculateCurrentHealth(int index)
    {
        //calculate a value between 0 and 1 of the health
        float health = 
            (opponentList[index].maxHealth - opponentList[index].currentHealth) / opponentList[index].maxHealth;
        //interpolate the value on a scale of 1 to 10
        float healthScale = Mathf.Lerp(1f, 10f, health);
        return healthScale;
    }

    private float CalculateProxity(int index)
    {
        //calculate distance from ai to the current enemy
        float distance = Vector2.Distance(transform.position, opponentList[index].transform.position);
        //calculate a proximityScale between o and 1 (/4 is to add weight)
        float proximityScale = 1f / (distance / 4);
        //interpolate the value on a scale of 1 to 10
        proximityScale = Mathf.Lerp(1f, 10f, proximityScale);

        return proximityScale;
    }

    private void TrimObjects()
    {
        /*trim the object that don't currently exist*/
        for (int i = opponentList.Count - 1; i >= 0; i--)
        {
            if (opponentList[i] == null || opponentList[i].gameObject.activeSelf == false)
            {
                opponentList.RemoveAt(i);
                opponentParameterList.RemoveAt(i);
            }
        }
    }
#endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }

}
