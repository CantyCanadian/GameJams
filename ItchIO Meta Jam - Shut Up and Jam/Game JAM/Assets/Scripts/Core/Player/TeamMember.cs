using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMember
{
    public enum PositivePersonality
    {
        Optimist,
        Dieting,
        Genius,
        KeenEye,
        SmallSleeper,
        Focused,
        Lightweight,
        Null
    }

    public enum NegativePersonality
    {
        Pessimist,
        Glutton,
        Dreamy,
        Careless,
        Sleeper,
        Lazy,
        Tolerant,
        Null
    }

    public string Name;
    public bool GenderIsMale;
    public string DescriptionKey;

    public int Level;
    public int CurrentExp;

    public int Programming;
    public int Debugging;
    public int Creativity;
    public int Art;
    public int Audio;
    public int Bugging;

    public int Potential;

    public PositivePersonality[] PersonalityPositive;
    public NegativePersonality[] PersonalityNegative;

    public TeamMember()
    {
        PersonalityPositive = new PositivePersonality[] { PositivePersonality.Null, PositivePersonality.Null, PositivePersonality.Null };
        PersonalityNegative = new NegativePersonality[] { NegativePersonality.Null, NegativePersonality.Null, NegativePersonality.Null };
    }

    public string[] GetData() //For Saving Purposes
    {
        List<string> result = new List<string>();

        result.Add(Name);
        result.Add(GenderIsMale ? "True" : "False");
        result.Add(DescriptionKey);

        result.Add(Level.ToString());
        result.Add(CurrentExp.ToString());

        result.Add(Programming.ToString());
        result.Add(Debugging.ToString());
        result.Add(Creativity.ToString());
        result.Add(Art.ToString());
        result.Add(Audio.ToString());
        result.Add(Bugging.ToString());

        result.Add(Potential.ToString());

        result.Add(PersonalityPositive[0].ToString());
        result.Add(PersonalityPositive[1].ToString());
        result.Add(PersonalityPositive[2].ToString());

        result.Add(PersonalityNegative[0].ToString());
        result.Add(PersonalityNegative[1].ToString());
        result.Add(PersonalityNegative[2].ToString());

        return result.ToArray();
    }

    public int SetData(string[] data, int index) //For Loading Purposes
    {
        Name = data[index];
        GenderIsMale = data[index + 1] == "True";
        DescriptionKey = data[index + 2];

        Level = int.Parse(data[index + 3]);
        CurrentExp = int.Parse(data[index + 4]);

        Programming = int.Parse(data[index + 5]);
        Debugging = int.Parse(data[index + 6]);
        Creativity = int.Parse(data[index + 7]);
        Art = int.Parse(data[index + 8]);
        Audio = int.Parse(data[index + 9]);
        Bugging = int.Parse(data[index + 10]);

        Potential = int.Parse(data[index + 11]);

        PersonalityPositive[0] = (PositivePersonality)System.Enum.Parse(typeof(PositivePersonality), data[index + 12]);
        PersonalityPositive[1] = (PositivePersonality)System.Enum.Parse(typeof(PositivePersonality), data[index + 13]);
        PersonalityPositive[2] = (PositivePersonality)System.Enum.Parse(typeof(PositivePersonality), data[index + 14]);

        PersonalityNegative[0] = (NegativePersonality)System.Enum.Parse(typeof(NegativePersonality), data[index + 15]);
        PersonalityNegative[1] = (NegativePersonality)System.Enum.Parse(typeof(NegativePersonality), data[index + 16]);
        PersonalityNegative[2] = (NegativePersonality)System.Enum.Parse(typeof(NegativePersonality), data[index + 17]);

        return index + 18;
    }

    public void SetData() //For New Game Purposes
    {
        GenderIsMale = Random.Range(0, 1) == 0;
        Name = GenderIsMale ? GenerateMaleName() : GenerateFemaleName();
        DescriptionKey = GameManager.Instance.Data.GetCharacterLoc("CHARACTER_DESCRIPTION" + Random.Range(1, int.Parse(GameManager.Instance.Data.GetData("CHARACTERGEN_DESCRIPTION_COUNT")) + 1));

        Level = 1;
        CurrentExp = 0;

        Programming = 1;

        Programming = 1;
        Debugging = 1;
        Creativity = 1;
        Art = 1;
        Audio = 1;
        Bugging = 10;

        Potential = 3;

        PersonalityPositive[0] = (PositivePersonality)Random.Range(0, (int)PositivePersonality.Null);
        PersonalityPositive[1] = PositivePersonality.Null;
        PersonalityPositive[2] = PositivePersonality.Null;

        PersonalityNegative[0] = (NegativePersonality)RandomUtil.RangeExcept(0, (int)NegativePersonality.Null, (int)PersonalityPositive[0]);
        PersonalityNegative[1] = NegativePersonality.Null;
        PersonalityNegative[2] = NegativePersonality.Null;
    }

    public string GetPersonalityDescriptionKey(PositivePersonality pp)
    {
        switch(pp)
        {
            case PositivePersonality.Dieting:
                return "CHARACTER_PERSONALITY_DIETINGDESC";

            case PositivePersonality.Focused:
                return "CHARACTER_PERSONALITY_FOCUSEDDESC";

            case PositivePersonality.Genius:
                return "CHARACTER_PERSONALITY_GENIUSDESC";

            case PositivePersonality.KeenEye:
                return "CHARACTER_PERSONALITY_KEENEYEDDESC";

            case PositivePersonality.Lightweight:
                return "CHARACTER_PERSONALITY_LIGHTWEIGHTDESC";

            case PositivePersonality.Optimist:
                return "CHARACTER_PERSONALITY_OPTIMISTDESC";

            case PositivePersonality.SmallSleeper:
                return "CHARACTER_PERSONALITY_SMALLSLEEPDESC";

            default:
                return "NULL";
        }
    }

    public string GetPersonalityDescriptionKey(NegativePersonality np)
    {
        switch (np)
        {
            case NegativePersonality.Glutton:
                return "CHARACTER_PERSONALITY_GLUTTONDESC";

            case NegativePersonality.Lazy:
                return "CHARACTER_PERSONALITY_LAZYDESC";

            case NegativePersonality.Dreamy:
                return "CHARACTER_PERSONALITY_DREAMYDESC";

            case NegativePersonality.Careless:
                return "CHARACTER_PERSONALITY_CARELESSDESC";

            case NegativePersonality.Tolerant:
                return "CHARACTER_PERSONALITY_TOLERANTDESC";

            case NegativePersonality.Pessimist:
                return "CHARACTER_PERSONALITY_PESSIMISTDESC";

            case NegativePersonality.Sleeper:
                return "CHARACTER_PERSONALITY_SLEEPERDESC";

            default:
                return "NULL";
        }
    }

    public string GenerateMaleName()
    {
        DataManager dm = GameManager.Instance.Data;

        string name = "";

        int firstNameCount = int.Parse(dm.GetData("CHARACTERGEN_FIRSTNAMEMALE_COUNT"));
        int lastNameCount = int.Parse(dm.GetData("CHARACTERGEN_LASTNAME_COUNT"));

        name += dm.GetCharacterLoc("CHARGEN_FIRSTNAME_MALE" + Random.Range(1, firstNameCount + 1));
        name += " ";
        name += dm.GetCharacterLoc("CHARGEN_LASTNAME" + Random.Range(1, lastNameCount + 1));

        return name;
    }

    public string GenerateFemaleName()
    {
        DataManager dm = GameManager.Instance.Data;

        string name = "";

        int firstNameCount = int.Parse(dm.GetData("CHARACTERGEN_FIRSTNAMEFEMALE_COUNT"));
        int lastNameCount = int.Parse(dm.GetData("CHARACTERGEN_LASTNAME_COUNT"));

        name += dm.GetCharacterLoc("CHARGEN_FIRSTNAME_FEMALE" + Random.Range(1, firstNameCount + 1));
        name += " ";
        name += dm.GetCharacterLoc("CHARGEN_LASTNAME" + Random.Range(1, lastNameCount + 1));

        return name;
    }

    public int CalculateExp(int level)
    {
        int extra = 0;
        for (int i = 0; i < level; i++)
        {
            extra += i;
        }

        return 100 + (extra * 10);
    }
}
