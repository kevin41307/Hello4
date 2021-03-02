using System.Collections.Generic;
using UnityEngine;

public class AIComboSystem 
{
    List<string> shortRange = new List<string>();
    List<string> midRange = new List<string>();
    List<string> longRange = new List<string>();
    InteractiveWithPlayer interactiveWithPlayer;
    List<string[]> twoCombos = new List<string[]>();
    string[] twoCombo1 = new string[] { "Run", "Attack_Dash" };
    string[] twoCombo2 = new string[] { "Run", "Attack_Jump" };   
    string[] twoCombo3 = new string[] { "Run", "Piercing" };
    string[] twoCombo4 = new string[] { "Run", "Dodge" };
    string[] twoCombo5 = new string[] { "Run", "Attack12" };
    string[] twoCombo6 = new string[] { "Run", "Attack26" };
    string[] twoCombo7 = new string[] { "Run", "Attack1" };
    string[] twoCombo8 = new string[] { "Run", "Attack2" };


    List<string[]> threeCombos = new List<string[]>();
    string[] threeCombo1 = new string[] { "Run", "Attack2", "Attack12" };
    string[] threeCombo2 = new string[] { "Run", "Piercing", "Attack26" };
    string[] threeCombo3 = new string[] { "Run", "Dodge", "Attack_Jump" };
    string[] threeCombo4 = new string[] { "Run", "Dodge", "Attack_Dash" };
    string[] threeCombo5 = new string[] { "Attack_Dash", "Walk", "Attack26" };
    string[] threeCombo6 = new string[] { "Walk", "Piercing", "Attack2" };
    string[] threeCombo7 = new string[] { "Run", "Attack12", "Attack2" };
    string[] threeCombo8 = new string[] { "Dodge", "Walk", "Piercing" };
    string[] threeCombo9 = new string[] { "Run", "Piercing", "Piercing" };
    string[] threeCombo10 = new string[] { "Run", "Attack26", "Dodge" };
    string[] threeCombo11 = new string[] { "Run", "Attack12", "Attack26" };
    string[] threeCombo12 = new string[] { "Run", "Attack1", "Attack2" };
    string[] threeCombo13 = new string[] { "Piercing", "Dodge", "Dodge" };
    string[] threeCombo14 = new string[] { "Run", "Piercing", "Attack2" };
    string[] threeCombo15 = new string[] { "Walk", "Attack2", "Attack1" };
    string[] threeCombo16 = new string[] { "Walk", "Attack1", "Dodge" };


    List<string[]> longAtackCombos = new List<string[]>();


    //move cmds: Walk, Run, Dodge
    //attack cmds: Piercing Attack1  Attack2 Attack12 Attack26
    //Attack_Dash Attack_Jump

    public AIComboSystem(InteractiveWithPlayer _interactiveWithPlayer)
    {
        interactiveWithPlayer = _interactiveWithPlayer;
        Init();
    }

    void Init()
    {

        shortRange.Add("Attack1");
        shortRange.Add("Kick");
        shortRange.Add("Attack2");
        shortRange.Add("Attack12");
        shortRange.Add("Dodge");

        midRange.Add("Attack1");
        midRange.Add("Piercing");
        midRange.Add("Attack2");
        midRange.Add("Attack12");
        midRange.Add("Attack26");
        midRange.Add("Dodge");

        longRange.Add("Attack_Dash");
        longRange.Add("Attack_Jump");

        //twoCombos.Add(twoCombo1);
        //twoCombos.Add(twoCombo2);
        twoCombos.Add(twoCombo3);
        twoCombos.Add(twoCombo4);
        twoCombos.Add(twoCombo5);
        twoCombos.Add(twoCombo6);
        twoCombos.Add(twoCombo7);
        twoCombos.Add(twoCombo8);

        threeCombos.Add(threeCombo1);
        threeCombos.Add(threeCombo2);
        threeCombos.Add(threeCombo3);
        threeCombos.Add(threeCombo4);
        threeCombos.Add(threeCombo5);
        threeCombos.Add(threeCombo6);
        threeCombos.Add(threeCombo7);
        threeCombos.Add(threeCombo8);
        threeCombos.Add(threeCombo9);
        threeCombos.Add(threeCombo10);
        threeCombos.Add(threeCombo11);
        threeCombos.Add(threeCombo12);
        threeCombos.Add(threeCombo13);
        threeCombos.Add(threeCombo14);
        threeCombos.Add(threeCombo15);
        threeCombos.Add(threeCombo16);


        longAtackCombos.Add(twoCombo1);
        longAtackCombos.Add(twoCombo2);

    }

    public string GetSkillName()
    {
        string skillName = "";
        int state = interactiveWithPlayer.GetCurrenDistanceState();

        if (state == 0)
        {
            int i = Random.Range(0, shortRange.Count);
            skillName = shortRange[i];

        }
        if (state == 1) 
        {
            int i = Random.Range(0, midRange.Count);
            skillName = midRange[i];

        }
        if (state > 1)
        {
            int i = Random.Range(0, longRange.Count);
            skillName = longRange[i];
        }
        return skillName;
    }

    public string[] GetSkillNames(int count)
    {
        string[] skillNames = new string[count] ;

        if( count == 1 )
            skillNames[0] = GetSkillName();
        else if(count == 2)
        {
            int i = Random.Range(0, twoCombos.Count);
            skillNames = twoCombos[i];
        }
        else if (count == 3)
        {
            int i = Random.Range(0, threeCombos.Count);
            skillNames = threeCombos[i];
        }


        return skillNames;
    }

    public string[] GetSkillNames(int count, float range)
    {
        string[] skillNames = new string[count];

        if(range > interactiveWithPlayer.attackRange + 0.1f )
        {
            int i = Random.Range(0, longAtackCombos.Count);
            skillNames = longAtackCombos[i];
        }
        else
        {
            skillNames = GetSkillNames(count);
        }

        return skillNames;
    }
}
