using System.Collections.Generic;
using UnityEngine;
using Jext;
using System.Collections;

public class Jai : MonoBehaviour {

    #region Inspector Variables
    public int criticalLevel = 10;
    public NormalAction[] actions;
    public List<Stat> stats = new List<Stat>();
    /*you can throw anything you want in here, health, hunger, social, even something like a quest meter that sets itself to zero once
     a milestone has been reached in the story.
     */
    #endregion

    #region General Variables
    //these variables are mostly used in calculations as shortcuts
    [HideInInspector] 
    public Action curAction;
    private Stat curStat;
    [HideInInspector]
    public int curValue;
    private float timeLeft, timeRequired;

    public enum Requirement {hasFood, hasBucket, hasFilledBucket, openShop, closedShop }
    public List<Requirement> filledRequirements = new List<Requirement>(); //for instance, hasShovel or hasSandwich
    #endregion

    #region Default Methods
    public virtual void Activate()
    {
        //make copies so that you dont save changes in play mode
        for (int stat = 0; stat < stats.Count; stat++)
            if (!stats[stat].saveChangesInPlayMode)
            {
                stats[stat] = Instantiate(stats[stat]);
                stats[stat].Init(this);
            }
        for (int action = 0; action < actions.Length; action++)
            if (!actions[action].saveChangesInPlayMode)
                actions[action] = Instantiate(actions[action]);
        foreach (Stat stat in stats)
            for (int rootAction = 0; rootAction < stat.boosters.Count; rootAction++)
                if (!stat.boosters[rootAction].saveChangesInPlayMode)
                    stat.boosters[rootAction] = Instantiate(stat.boosters[rootAction]);

        //initialize
        foreach (Action normalAction in actions)
            normalAction.Init(this);
        foreach (Stat stat in stats)
            foreach (RootAction rootAction in stat.boosters)
                rootAction.Init(this, stat);

        //enable stats that rely on ticks, like a hunger meter that slowly depletes
        List<ITickable> tickables = stats.GetTypeFromListAsT<ITickable, Stat>();
        tickables.ForEach(x => StartCoroutine(x.Tick()));
        StartCoroutine(CheckForEvent());
    }

    public virtual void LateActivate()
    {
        StartCoroutine(CheckForEvent());
    }
    #endregion

    [SerializeField]
    private float eventTriggerTime;
    private IEnumerator CheckForEvent()
    {
        while (true)
        {
            NewEvent();
            yield return new WaitForSeconds(eventTriggerTime);
        }
    }

    public void NewEvent()
    {
        //choose which action to take based on the lowest value
        stats = stats.SuperSort(StatSorter);

        //needed for check after this
        curStat = stats.First();
        curValue = curStat.GetValue();

        if (curAction != null)
            if (curValue < criticalLevel || curAction.interruptable) //when, for instance, the AI is dying from hunger
                curAction.Cancel();
            else
                return;
        
        foreach(Stat stat in stats)
        {
            //setting up shortcuts
            curStat = stat;
            curValue = curStat.GetValue();
            timeLeft = curStat.TimeLeftUntilEmpty();

            Methods.Debug("Fix " + curStat.name);
            if (CalculatePath())
                return;
        }
    }

    //function specific variables, since this function will be called a lot I'm not putting these in the function itself (garbage collector)
    private List<CalcAction> open = new List<CalcAction>(), succeeded = new List<CalcAction>();
    private List<Requirement> openRequirements = new List<Requirement>(), curRewards, curRequirements;
    private NormalAction normalAction;
    private CalcAction calcAction;
    private bool fit;
    public bool CalculatePath()
    {
        //reset lists
        succeeded.Clear();
        open.Clear();

        foreach (RootAction rA in curStat.boosters) //rootactions lead directly to the AI's wishes
            if(rA.Executable)
                if (rA.GetReturnValue() + curStat.GetValue() > criticalLevel) {
                    fit = true;
                    curRequirements = rA.GetRequirements();
                    foreach (Requirement requirement in curRequirements)
                        if(!filledRequirements.Contains(requirement))
                        {
                            fit = false;
                            break;
                        }
                    calcAction = new CalcAction(rA, rA.GetEstimatedTimeRequired());
                    if (fit)
                        succeeded.Add(calcAction);
                    else
                        open.Add(calcAction);
                }

        while(open.Count > 0) //from here the AI will check what actions are necessary to be able to execute said rootactions 
        {
            open.SuperSort(CActionSorter);
            openRequirements = open.First().action.GetRequirements(); //for instance, one action (eating) may require "hasFood"

            foreach (NormalAction action in actions) {
                curRewards = action.GetRewards();
                foreach(Requirement reward in openRequirements)
                    if(curRewards.Contains(reward))
                    {
                        //setting variable references
                        timeRequired = action.GetEstimatedTimeRequired() + open.First().duration;

                        if (timeRequired < timeLeft) //if it is able to execute it in time (when the value does not reach zero due to ticks in that time)
                        {
                            fit = true;
                            curRequirements = action.GetRequirements();

                            foreach (Requirement required in curRequirements) //check if AI can execute
                                if(!filledRequirements.Contains(required))
                                {
                                    fit = false;
                                    break;
                                }

                            calcAction = new CalcAction(action, timeRequired);
                            if (!fit)
                                open.Add(calcAction); //when the function is leading to the rootfunction but requires certain requirements
                            else
                                succeeded.Add(calcAction); //when a path is finished
                            break;
                        }
                    }
            }

            open.RemoveAt(0);
        }

        succeeded.RemoveAll(x => !x.action.Executable);

        if (succeeded.Count == 0)
        {
            Methods.Debug("No suitable action has been found");
            return false;
        }
        
        succeeded = succeeded.SuperSort(CActionSorter); //sort on the shortest path
        curAction = succeeded.First().action;
        
        ExecuteNext(curAction);
        return true;
    }

    protected virtual void ExecuteNext(Action action)
    {
        Methods.Debug(curAction.name + " " + action.Executable);
        if (action.Executable)
            action.Execute();
    }

    private struct CalcAction
    {
        public Action action;
        public float duration;

        public CalcAction(Action action, float duration)
        {
            this.action = action;
            this.duration = duration;
        }
    }

    #region Tools
    private float StatSorter(Stat stat)
    {
        return stat.GetValue();
    }

    private float CActionSorter(CalcAction calcAction)
    {
        return calcAction.duration;
    }
    #endregion
}