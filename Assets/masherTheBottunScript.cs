using System.Linq;
using System;
using UnityEngine;
using KModkit;
using System.Collections;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

public class masherTheBottunScript : MonoBehaviour {

    public KMBombModule Module;
    public KMBombInfo Info;
    public KMAudio Audio;
    public KMSelectable btnSelectable;
    public Transform moduleTransform, numberTransform;
    public TextMesh screenText, btnText;
    public MeshRenderer btnRenderer;
    public Color32[] numberColors, buttonBGColors;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private bool solved = false;

    private int[] positions = { 0,0,0,0 };
    private int numberDisplayed = 0;
    private int sectionUsed = 0;
    private int lastStage = 69420;

    private int target = 0;
    private int bitch = 0;

    private int previousNumberColor = 2;

    private int currentCell = 0;
    private int previousBtnColor = 69420;
    private static readonly DayOfWeek todayDayOfWeek = DateTime.Today.DayOfWeek;

    private static readonly int[] wordFormLengths = { 4, 3, 3, 5, 4, 4, 3, 5, 5, 4, 3, 6, 6, 8, 8, 7, 7, 9, 8, 8 };
    
    private bool doingTheMorseThing = false;
    private bool doingTheCycleThing = false;
    private bool doingTheSpinThing = false;
    private bool doingTheOtherSlightlyDifferentSpinThing = false;
    private bool shutTheFuckUp = false;

    private bool clockwise = true;
    private int[] spinDirections = { 0, 0, 0 };

    private bool lastStageWasWrong = false;
    
    private int[] previousSections = { 99, 99, 99, 99 };
    private int stageNo = 0;

    private int[] bgColorTargets;
    private int[] spinTargets;

    // Use this for initialization
    void Start () {
        _moduleId = _moduleIdCounter++;
        Module.OnActivate += Activate;

        Init();
	}

    void Activate()
    {
        btnSelectable.OnInteract += delegate ()
        {
            btnSelectable.AddInteractionPunch();
            if (!solved)
                Press();
            return false;
        };
    }

    void Init()
    {
        numberDisplayed = Random.Range(60, 80);
        screenText.text = numberDisplayed.ToString();
        Debug.LogFormat("[Masher The Bottun #{0}] The starting number was {1}.", _moduleId, numberDisplayed);

        for (int i = 0; i < 4; i++)
        {
            positions[i] = Random.Range(1, numberDisplayed);
            while (positions.ToList().Where(x => x == positions[i]).Count() > 1)
                positions[i] = Random.Range(0, numberDisplayed);
        }

        Debug.LogFormat("[Masher The Bottun #{0}] The four stages are, in order of generation, {1}, {2}, {3}, and {4}.", _moduleId, positions[0], positions[1], positions[2], positions[3]);
    }

    void Press()
    {
        // Check if button was pressed correctly, based on the previous stage.

        if (lastStage == 0)
        {
            if ((int)Info.GetTime() % 10 != target)
            {
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at " + Info.GetFormattedTime() + "! You should've pressed when the last digit of the timer was {1}.", _moduleId, target);
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }

        if (lastStage == 1)
        {
            if (!bgColorTargets.Contains((int)Info.GetTime() % 60))
            {
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at {1}! That was incorrect, strike!.", _moduleId, Info.GetFormattedTime());
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }

        if (lastStage == 2)
        {
            if ((int)Info.GetTime() % 10 != bitch)
            {
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at " + Info.GetFormattedTime() + "! You should've pressed when the last digit of the timer was {1}.", _moduleId, bitch);
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }

        if (lastStage == 3)
        {
            doingTheMorseThing = false;
            if ((int)Info.GetTime() % 10 != target)
            {
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at " + Info.GetFormattedTime() + "! You should've pressed when the last digit of the timer was {1}.", _moduleId, bitch);
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }

        if (lastStage == 4)
        {
            doingTheCycleThing = false;
            if ((int)Info.GetTime() % 10 != target)
            {
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at " + Info.GetFormattedTime() + "! You should've pressed when the last digit of the timer was {1}.", _moduleId, bitch);
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }

        if (lastStage == 5)
        {
            doingTheSpinThing = false;
            StartCoroutine(SlowSpin(clockwise));
            if ((int)Info.GetTime() % 10 != target)
            {
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at " + Info.GetFormattedTime() + "! You should've pressed when the last digit of the timer was {1}.", _moduleId, bitch);
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }
        
        if (lastStage == 6)
        {
            doingTheOtherSlightlyDifferentSpinThing = false;
            if (!spinTargets.Contains((int)Info.GetTime() % 60))
            {
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at {1}! Strike!", _moduleId, Info.GetFormattedTime());
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }

        if (lastStage == 7)
        {
            if ((int)Info.GetTime() % 10 != target)
            {
                shutTheFuckUp = true;
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at " + Info.GetFormattedTime() + "! You should've pressed when the last digit of the timer was {1}.", _moduleId, target);
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }

        if (lastStage == 8)
        {
            if ((int)Info.GetTime() % 10 != target)
            {
                shutTheFuckUp = true;
                Debug.LogFormat("[Masher The Bottun #{0}] You pressed the button at " + Info.GetFormattedTime() + "! You should've pressed when the last digit of the timer was {1}.", _moduleId, target);
                Module.HandleStrike();
                lastStageWasWrong = true;
            }
        }

        if (!positions.Contains(numberDisplayed - 1))
            StartCoroutine(CheckTime(numberDisplayed));

        // Decrease number.

        numberDisplayed--;
        screenText.text = numberDisplayed.ToString();

        // Create new stage.

        if (numberDisplayed == 0)
        {
            Debug.LogFormat("[Masher The Bottun #{0}] Bottun is thoroughly mashed... solving module.", _moduleId);
            Module.HandlePass();
            solved = true;
        }

        if (positions.Contains(numberDisplayed))
        {
            target = 0;
            bitch = 0;
            sectionUsed = Random.Range(0, 9);
            while (previousSections.Contains(sectionUsed))
                sectionUsed = Random.Range(0, 9);
            lastStage = sectionUsed;
            previousSections[stageNo] = sectionUsed;
            stageNo++;

            Debug.LogFormat("[Masher The Bottun #{0}] Stage {1} used Section {2}.", _moduleId, numberDisplayed, sectionUsed + 2);

            if (sectionUsed == 0)
            {
                var color = Random.Range(0, 7);
                while (color == previousNumberColor)
                    color = Random.Range(0, 7);
                previousNumberColor = color;
                string[] colorNames = { "red", "orange", "yellow", "green", "cyan", "blue", "purple" };
                screenText.color = numberColors[color];
                Debug.LogFormat("[Masher The Bottun #{0}] The number is {1}.", _moduleId, colorNames[color]);

                char[] table = { 'F', 'N', 'L', 'A', 'M', 'A', 'C', '0', '0', 'L', '1', '3', '0', 'B', 'J', 'E', 'K', 'T', 'S', 'C', '0', 'U', 'N', 'T', 'R', 'I', 'E', 'S' };
                char[] otherTable = { 'M', 'S', 'H', '1', '2', 'B', 'T', 'N', 'E', 'P', 'I', 'C', 'H', 'A', '6', '9', 'B', 'T', 'N', '5', '7', 'W', 'H', 'Y', 'T', '0', 'A', '4' };
                char[] surpriseTheresAnotherTable = { 'B', 'C', 'D', '9', 'E', 'F', '3', 'G', 'A', '6', 'H', 'J', 'I', '2', 'K', 'L', '7', 'M', 'X', 'P', 'Q', 'R', 'S', '2', 'N', 'U', '8', 'V' };

                if (Info.GetSerialNumber().Contains(table[4 * color]) || Info.GetSerialNumber().Contains(otherTable[4 * color]) || Info.GetSerialNumber().Contains(surpriseTheresAnotherTable[4 * color]))
                    target += 8;

                if (Info.GetSerialNumber().Contains(table[1 + 4 * color]) || Info.GetSerialNumber().Contains(otherTable[1 + 4 * color]) || Info.GetSerialNumber().Contains(surpriseTheresAnotherTable[1 + 4 * color]))
                    target += 4;

                if (Info.GetSerialNumber().Contains(table[2 + 4 * color]) || Info.GetSerialNumber().Contains(otherTable[2 + 4 * color]) || Info.GetSerialNumber().Contains(surpriseTheresAnotherTable[2 + 4 * color]))
                    target += 2;

                if (Info.GetSerialNumber().Contains(table[3 + 4 * color]) || Info.GetSerialNumber().Contains(otherTable[3 + 4 * color]) || Info.GetSerialNumber().Contains(surpriseTheresAnotherTable[3 + 4 * color]))
                    target += 1;

                target %= 10;
                Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the last digit of the timer is {1}.", _moduleId, target.ToString());
            }

            if (sectionUsed == 1)
            {
                var color = Random.Range(0, 7);
                while (color == previousBtnColor)
                    color = Random.Range(0, 7);
                previousBtnColor = color;
                currentCell = color;
                string[] colorNames = { "magenta", "cyan", "gray", "blue", "red", "yellow", "green" };
                btnRenderer.material.color = buttonBGColors[color];
                Debug.LogFormat("[Masher The Bottun #{0}] The button background is {1}.", _moduleId, colorNames[color]);

                //This snippet of code was originally in the Press() method. Epic, why.
                bool[] visitedCells = { false, false, false, false, false, false, false };
                bool[] statements = {
                    Info.GetIndicators().Contains("BOB") || Info.GetIndicators().Contains("FRK"),
                    Info.GetBatteryCount(Battery.D) > 1,
                    Info.GetModuleNames().Count() <= 31,
                    Info.GetPorts().Contains("RJ45"),
                    Info.GetStrikes() > 0,
                    todayDayOfWeek == DayOfWeek.Monday || todayDayOfWeek == DayOfWeek.Wednesday || todayDayOfWeek == DayOfWeek.Friday,
                    Info.GetSolvedModuleNames().Count() <= Info.GetSolvedModuleNames().Count() / 2}; 
                int[] locations = { 3, 2, 2, 0, 6, 4, 5, 1, 0, 5, 1, 6, 4, 3 }; // first number in each pair = yes, second number = no

                while (!visitedCells[currentCell])
                {
                    visitedCells[currentCell] = true;
                    if (statements[currentCell])
                        currentCell = locations[currentCell * 2];
                    else
                        currentCell = locations[currentCell * 2 + 1];
                }
                switch (currentCell)
                {
                    case 0: bgColorTargets = new int[] { 5, 15, 25, 35, 45, 55};  Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the last seconds digit is a 5.", _moduleId); break;
                    case 1: bgColorTargets = new int[] { 19, 28, 37, 46, 55 };    Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the seconds digits add up to 10.", _moduleId); break;
                    case 2: bgColorTargets = new int[] { 7, 17, 27, 37, 47, 57 }; Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the last seconds digit is a 7.", _moduleId); break;
                    case 3: bgColorTargets = new int[] { 0, 10, 20, 30, 40, 50 }; Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the last seconds digit is a 0.", _moduleId); break;
                    case 4: bgColorTargets = new int[] { 7, 16, 25, 34, 43, 52 }; Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the seconds digits add up to 7.", _moduleId); break;
                    case 5: bgColorTargets = new int[] { 0, 11, 22, 33, 44, 55 }; Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the seconds digits are the same.", _moduleId); break;
                    case 6: bgColorTargets = new int[] { 3, 14, 25, 36, 47, 58 }; Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the difference between the seconds digits is 3.", _moduleId); break;
                }
            }

            if (sectionUsed == 2)
            {
                var wordShown = btnText.text;
                char[] theEntireAlphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                while (wordShown == btnText.text)
                {
                    wordShown = "";
                    for (int i = 0; i < Random.Range(3, 6); i++)
                        wordShown += theEntireAlphabet[Random.Range(0, 26)];
                }
                btnText.text = wordShown;
                Debug.LogFormat("[Masher The Bottun #{0}] The button now says {1}.", _moduleId, wordShown);

                var xValue = 1;
                var totalXs = 0;
                var target = numberDisplayed;
                Debug.LogFormat("[Masher The Bottun #{0}] The number starts at {1}", _moduleId, numberDisplayed);

                foreach (char letter in wordShown)
                {
                    totalXs += xValue;

                    if (letter == 'A' || letter == 'N')
                    {
                        target += xValue;
                        xValue = Info.GetIndicators().Count();
                    }
                    if (letter == 'B' || letter == 'O')
                    {
                        target *= xValue + 1;
                        xValue = Info.GetSerialNumberNumbers().Last();
                    }
                    if (letter == 'C' || letter == 'P')
                        target -= wordFormLengths[xValue];
                    if (letter == 'D' || letter == 'Q')
                    {
                        target -= 2 * xValue;
                        xValue = Info.GetBatteryCount() * Info.GetBatteryHolderCount();
                    }
                    if (letter == 'E' || letter == 'R')
                    {
                        target += (int)(xValue * .5);
                        xValue = wordShown.Length;
                    }
                    if (letter == 'F' || letter == 'S')
                    {
                        target /= xValue + 1;
                        xValue = Info.GetPortCount("RJ45") + Info.GetPortCount("DVI");
                    }
                    if (letter == 'G' || letter == 'T')
                    {
                        target %= xValue + 5;
                        xValue = Info.GetStrikes() + 2;
                    }
                    if (letter == 'H' || letter == 'U')
                    {
                        target += xValue * 2;
                        xValue = Info.GetModuleNames().Count();
                    }
                    if (letter == 'I' || letter == 'V')
                    {
                        if (xValue % 2 == 0)
                            target += 5;
                        xValue = 0;
                        foreach (string module in Info.GetSolvableModuleNames())
                            if (module.ToLowerInvariant().Contains("button"))
                                xValue += 1;
                    }
                    if (letter == 'J' || letter == 'W')
                    {
                        target = (target + xValue) / 2;
                        if (todayDayOfWeek == DayOfWeek.Monday || todayDayOfWeek == DayOfWeek.Friday || todayDayOfWeek == DayOfWeek.Sunday)
                            xValue = 6;
                        else if (todayDayOfWeek == DayOfWeek.Tuesday)
                            xValue = 7;
                        else if (todayDayOfWeek == DayOfWeek.Thursday || todayDayOfWeek == DayOfWeek.Saturday)
                            xValue = 8;
                        else
                            xValue = 9;
                    }
                    if (letter == 'K' || letter == 'X')
                    {
                        target += wordFormLengths[xValue];
                        xValue = 13;
                    }
                    if (letter == 'L' || letter == 'Y')
                    {
                        if (xValue < 5 || xValue > 15)
                            target += 7;
                        else
                            target += 3;
                        xValue = totalXs;
                    }
                    if (letter == 'M' || letter == 'Z')
                    {
                        target -= xValue;
                        xValue = numberDisplayed % 10 + numberDisplayed / 10 % 6;
                    }

                    xValue %= 20;

                    if (letter != ' ')
                        Debug.LogFormat("[Masher The Bottun #{0}] The number is now {1}. [X] is now {2}.", _moduleId, target, xValue);
                }

                if (target < 0)
                    target *= -1;
                target %= 10;
                Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the last digit of the timer is {1}.", _moduleId, target);
                bitch = target;
                // somewhere between the above message and the start of Press() target gets changed to zero, and i've been trying to figure out why for an hour, so i just did this and it works. whatever
            }

            if (sectionUsed == 3)
            {
                var manual = Random.Range(0, 8);
                int[] targets = { 6, 9, 4, 2, 0, 1, 3, 7 };
                target = targets[manual];
                doingTheMorseThing = true;
                StartCoroutine(MorseFlash(manual));
            }

            if (sectionUsed == 4)
            {
                int[] numbers = { Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100) };
                Debug.LogFormat("[Masher The Bottun #{0}] The numbers are {1}, {2}, and {3}.", _moduleId, numbers[0], numbers[1], numbers[2]);
                int[] solutions = { 0, 0, 0 };
                doingTheCycleThing = true;
                StartCoroutine(NumberCycle(numbers));

                for (int i = 0; i < 3; i++)
                {
                    if (numbers[i] >= numbers[(i + 1) % 3] + 50)
                        solutions[i] = numbers[i] + numbers[(i + 1) % 3];
                    else if (numbers[(i + 1) % 3] >= numbers[i] + 50)
                    {
                        solutions[i] = int.Parse(numbers[i].ToString() + numbers[(i + 1) % 3].ToString()) % 9;
                        if (solutions[i] == 0)
                            solutions[i] = 9;
                    }
                    else if (numbers[i] >= numbers[(i + 1) % 3] + 25)
                        solutions[i] = numbers[i] * numbers[(i + 1) % 3];
                    else if (numbers[(i + 1) % 3] >= numbers[i] + 25)
                        solutions[i] = int.Parse((numbers[i] % 10).ToString() + (numbers[(i + 1) % 3] % 10).ToString());
                    else if (numbers[i] >= numbers[(i + 1) % 3])
                        solutions[i] = Math.Abs(numbers[i] - numbers[(i + 1) % 3]);
                    else if (numbers[i] <= numbers[(i + 1) % 3])
                        solutions[i] = (numbers[i] + numbers[(i + 1) % 3]) / 2;
                    else
                        solutions[i] = 0;
                    solutions[i] %= 100;
                    Debug.LogFormat("[Masher The Bottun #{0}] {1} and {2} evaluates to {3}.", _moduleId, numbers[i], numbers[(i + 1) % 3], solutions[i]);
                }
                target = int.Parse(solutions[0].ToString() + solutions[1].ToString() + solutions[2].ToString()) % 9;
                if (target == 0 && !(solutions[0] == 0 && solutions[1] == 0 && solutions[2] == 0))
                    target = 9;
                Debug.LogFormat("[Masher The Bottun #{0}] You should press the button when the last digit of the timer is {1}.", _moduleId, target);
            }

            if (sectionUsed == 5)
            {
                int[] table = { 3, 4, 9, 6, 7, 8, 5, 2 };
                int currentCell = 0;
                bool[] visitedCells = { false, false, false, false, false, false, false, false };
                if (Random.Range(0, 2) == 0) { clockwise = false; }
                doingTheSpinThing = true;
                StartCoroutine(NumberSpin(clockwise));

                if (numberDisplayed % 10 < 2)
                    currentCell = Info.GetSerialNumberNumbers().Skip(1).First() % table.Length;
                else
                    currentCell = Array.IndexOf(table, numberDisplayed % 10);

                if (clockwise) // this is completely counter-intuitive but i'm too goddamn lazy to fix it
                    Debug.LogFormat("[Masher The Bottun #{0}] The number is spinning clockwise.", _moduleId);
                else
                    Debug.LogFormat("[Masher The Bottun #{0}] The number is spinning counter-clockwise.", _moduleId);

                var offset = 0;
                if (numberDisplayed > 9)
                    offset = numberDisplayed / 10 % 6;
                else
                    offset = numberDisplayed;

                for (int i = 0; i < 7; i++)
                {
                    Debug.LogFormat("[Masher The Bottun #{0}] You visit the {1} cell.", _moduleId, table[currentCell]);
                    visitedCells[currentCell] = true;
                    if (clockwise)
                    {
                        currentCell += offset;
                        currentCell %= 8;
                        while (visitedCells[currentCell])
                        {
                            currentCell++;
                            currentCell %= 8;
                        }
                    }
                    else
                    {
                        currentCell -= offset;
                        if (currentCell < 0)
                            currentCell += 8;
                        while (visitedCells[currentCell])
                        {
                            currentCell--;
                            if (currentCell < 0)
                                currentCell += 8;
                        }
                    }
                }

                target = table[Array.IndexOf(visitedCells, false)];
                Debug.LogFormat("[Masher The Bottun #{0}] The last cell was the {1} cell. Press the button when the last digit of the timer is {1}.", _moduleId, table[currentCell]);
            }

            if (sectionUsed == 6)
            {
                doingTheOtherSlightlyDifferentSpinThing = true;
                string[] directionNames = { "90 clockwise", "180 clockwise", "270 clockwise", "90 counter-clockwise", "180 counter-clockwise", "270 counter-clockwise" };
                for (int i = 0; i < 3; i++)
                {
                    spinDirections[i] = Random.Range(0, 6);
                    Debug.LogFormat("[Masher The Bottun #{0}] Direction {1} is {2}.", _moduleId, i + 1, directionNames[spinDirections[i]]);
                }
                int[] primeNumbers = { 0, 2, 3, 5, 7 };
                int[] fourLetters = { 0, 4, 5, 9 };
                Predicate<int>[][] conditions = new Predicate<int>[][]
                {
                    new Predicate<int>[]{ x=> primeNumbers.Contains(x % 10), x=> x % 10 < 5, x=> !fourLetters.Contains(x % 10), x=> x % 10 > 4, x=> fourLetters.Contains(x % 10), x=> !primeNumbers.Contains(x % 10) },
                    new Predicate<int>[]{ x=> x % 2 == 0, x=> x % 10 % 3 != 0, x=> x % 10 >= 3 && x % 10 <= 7, x=> x % 10 % 3 == 0, x=> x % 10 < 3 || x % 10 > 7, x=> x % 2 == 1},
                    new Predicate<int>[]{ x=> x / 10 % 2 == 0, x => x / 10 % 2 == 1, x=> x / 10 % 2 == 0, x => x / 10 % 2 == 1, x=> x / 10 % 2 == 0, x => x / 10 % 2 == 1 }
                };
                Predicate<int>[] chosen3 = Enumerable.Range(0, 3).Select(x => conditions[x][spinDirections[x]]).ToArray();
                spinTargets = Enumerable.Range(0, 60).Where(num => chosen3.All(pred => pred(num))).ToArray();
                Debug.LogFormat("[Masher The Bottun #{0}] Correct timer digits to submit are {1}.", _moduleId, spinTargets.Join(", "));
                    StartCoroutine(ModuleSpin(spinDirections));
            }

            if (sectionUsed == 7)
            {
                string[] words = new string[3];
                string[] phrases = {
                    "you", "bought", "game",
                    "that", "is", "no", "work",
                    "just", "got", "autocorrect", "english", "enabled",
                    "people", "call", "troll", "it", "really", "dismotivates",
                    "explain", "myself", "what",
                    "did", "nobody", "upload", "example", "modification", "before",
                    "add", "twitch", "play",
                    "theyre", "all", "challenges",
                    "make", "easy", "modules", "and", "then", "harder",
                    "objects", "made" };
                int[] starts = { 0, 3, 7, 12, 18, 21, 27, 30, 33, 39 };
                int[] lengths = { 3, 4, 5, 6, 3, 6, 3, 3, 6, 2 };

                int doubleUsed = Random.Range(0, 10);
                int singleUsed = Random.Range(0, 10);
                while (singleUsed == doubleUsed)
                    singleUsed = Random.Range(0, 10);
                int index = Random.Range(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                        words[index] = phrases[starts[singleUsed] + Random.Range(0, lengths[singleUsed])];
                    else
                    {
                        words[index] = phrases[starts[doubleUsed] + Random.Range(0, lengths[doubleUsed])];
                        while (words.ToList().Where(x => x == words[index]).Count() > 1)
                            words[index] = phrases[starts[doubleUsed] + Random.Range(0, lengths[doubleUsed])];
                    }
                    index++;
                    index %= 3;
                }

                StartCoroutine(SayStuff(words));
                target = (singleUsed + 1) % 10;
                Debug.LogFormat("{0}", target);

                Debug.LogFormat("[Masher The Bottun #{0}] The words are {1}, {2}, and {3}.", _moduleId, words[0], words[1], words[2]);
                Debug.LogFormat("[Masher The Bottun #{0}] Two of the words are from quote #{1}, and one is from quote #{2}.", _moduleId, (doubleUsed + 1) % 10, (singleUsed + 1) % 10);
            }

            if (sectionUsed == 8)
            {
                string[] soundEffects = { "Honk", "Phone ringing", "Ding", "Megalovania", "Explosion", "Bruh", "Boing", "Discord call", "Bonk" };
                int sound = Random.Range(0, 9);
                Audio.PlaySoundAtTransform(soundEffects[sound], Module.transform);
                int theOtherNumber = 0;
                int[] table = { 4, 7, 2, 6, 0, 8, 3, 5, 1 };
                if (numberDisplayed % 10 == 9)
                    theOtherNumber = Array.IndexOf(table, (table[sound] + 4) % 9);
                else
                    theOtherNumber = Array.IndexOf(table, numberDisplayed % 10);

                Debug.LogFormat("[Masher The Bottun #{0}] The module played the '{1}' sound.", _moduleId, soundEffects[sound]);
                Debug.LogFormat("[Masher The Bottun #{0}] The number is {1}.", _moduleId, table[theOtherNumber]);

                if (sound == theOtherNumber)
                {
                    target = theOtherNumber;
                    Debug.LogFormat("The sound and the number are in the same cell.");
                }
                else if (sound / 3 == theOtherNumber / 3)
                {
                    Debug.LogFormat("The sound and the number are in the same row.");
                    if ((sound / 3 * 3 + 1) % 9 != theOtherNumber)
                        target = sound / 3 * 3 + 1;
                    else
                        target = sound / 3 * 3 + 2;
                }
                else if (sound % 3 == theOtherNumber % 3)
                {
                    Debug.LogFormat("The sound and the number are in the same column.");
                    if ((sound + 3) % 9 != theOtherNumber)
                        target = (sound + 3) % 9;
                    else
                        target = (sound + 6) % 9;
                }
                else
                {
                    Debug.LogFormat("The sound and the number are not in the same row or column.");
                    for (int i = 0; i < 9; i++)
                        if (sound % 3 != i % 3 && theOtherNumber % 3 != i % 3 && sound / 3 != i / 3 && theOtherNumber / 3 != i / 3)
                            target = i;
                }

                target = table[target];
            }
        }

        else
            lastStage = 69420;
    }

    IEnumerator MorseFlash(int manual)
    {
        string[] morse = { ".-", "-...", "-.-.", "-..", ".", "..-.", "--.", "....", "..", ".---", "-.-", ".-..", "--", "-.", "---", ".--.", "--.-", ".-.", "...", "-", "..-", "...-", ".--", "-..-", "-.--", "--..", ".----", "..---", "...--", "....-", " " };
        char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', ' ' };
        string[] addTimeForwards = { "add time module", "time module add", "module add the", "add the time", "the time love", "time love isnt", "love isnt alwys", "isnt alwys on", "alwys on thyme", "on thyme press", "thyme press the", "press the add", "add time bottun", "time bottun to", "bottun to add", "to add time", "add time press", "time press solve", "press solve to", "to solve fun" };
        string[] addTimeBackward = { "module time add", "add module time", "the add module", "time the add", "love time the", "isnt love time", "alwys isnt love", "on alwys isnt", "thyme on alwys", "press thyme on", "the press thyme", "add the press", "bottun time add", "to bottun time", "add to bottun", "time add to", "press time add", "solve press time", "to solve press", "fun solve to" };
        string[] flashingForwards = { "press the bottun", "the bottun that", "that is flashing", "is flashing if", "flashing if you", "if you pres", "you pres s", "pres s wrong", "s wrong bottun", "wrong bottun you", "bottun you will", "you will blow", "will blow up", "blow up xd", "up xd press", "xd press bottun", "press bottun that", "that is flash", "is flash red", "flash red and", "red and white", "and white no", "white no press", "no press other", "press other button", "other button fun", "button fun yay", "fun yay happy" }; ;
        string[] flashingBackward = { "bottun the press", "that bottun the", "flashing is that", "if flashing is", "you if flashing", "pres you if", "s pres you", "wrong s pres", "bottun wrong s", "you bottun wrong", "will you bottun", "blow will you", "up blow will", "xd up blow", "press xd up", "bottun press xd", "that bottun press", "flash is that", "red flash is", "and red flash", "white and red", "no white and", "press no white", "other press no", "button other press", "fun button other", "yay fun button", "happy yay fun" }; ;
        string[] rightOrder = { "button in the", "in the rite", "the rite order", "rite order you", "order you dont", "dont know how", "know how count", "how count 1234", "count 1234 lol", "1234 lol xd", "lol xd press", "xd press button", "button in order", "in order 1234", "order 1234 have", "1234 have fun" };
        string[] wrongOrder = { "the in button", "rite the in", "order rite the", "you order rite", "dont you order", "how know dont", "count how know", "1234 count how", "lol 1234 count", "xd lol 1234", "press xd lol", "button press xd", "order in button", "1234 order in", "have 1234 order", "fun have 1234" };
        // i'm so fucking funny
        string[] dontPressForwards = { "dont pres the", "pres the red", "the red button", "red button you", "button you press", "you press red", "press red button", "red button haha", "button haha press", "haha press the", "press the button", "the button with", "button with the", "with the green", "the green light", "green light dont", "light dont press", "dont press button", "press button with", "button with red", "with red light", "red light easy", "light easy haven", "easy haven fun" };
        string[] dontPressBackward = { "the pres dont", "red the pres", "button red the", "you button red", "press you button", "red press you", "button red press", "haha button red", "press haha button", "the press haha", "button the press", "with button the", "the with button", "green the with", "light green the", "dont light green", "press dont light", "button press dont", "with button press", "red with button", "light red with", "easy light red", "haven easy light", "fun haven easy" };
        // there's probably a better way of doing this. let me know if there is.
        
        string[] listOfPhrases = { };

        if (manual == 0)
            listOfPhrases = addTimeForwards;
        if (manual == 1)
            listOfPhrases = addTimeBackward;
        if (manual == 2)
            listOfPhrases = flashingForwards;
        if (manual == 3)
            listOfPhrases = flashingBackward;
        if (manual == 4)
            listOfPhrases = rightOrder;
        if (manual == 5)
            listOfPhrases = wrongOrder;
        if (manual == 6)
            listOfPhrases = dontPressForwards;
        if (manual == 7)
            listOfPhrases = dontPressBackward;
        var phraseNum = Random.Range(0, listOfPhrases.Length);
        Debug.LogFormat("[Masher The Bottun #{0}] The transmitted message: {1}.", _moduleId, listOfPhrases[phraseNum]);
        Debug.LogFormat("[Masher The Bottun #{0}] This means you should press the button when the last digit of the timer is {1}.", _moduleId, target);
        Color32 currentColor = screenText.color;

        while (doingTheMorseThing)
        {
            screenText.color = new Color32(0, 0, 0, 0);
            for (int i = 0; i < 20; i++)
            {
                yield return new WaitForSeconds(.1f);
                if (!doingTheMorseThing)
                    break;
            }

            foreach (char letter in listOfPhrases[phraseNum])
            {
                if (!doingTheMorseThing)
                    break;
                screenText.color = new Color32(0, 0, 0, 0);
                foreach (char flash in morse[Array.IndexOf(alphabet, letter)])
                {
                    if (!doingTheMorseThing)
                        break;
                    if (flash == '.')
                    {
                        yield return new WaitForSeconds(.3f);
                        screenText.color = currentColor;
                        if (!doingTheMorseThing)
                            break;
                        yield return new WaitForSeconds(.15f);
                        screenText.color = new Color32(0, 0, 0, 0);
                    }
                    else if (flash == '-')
                    {
                        yield return new WaitForSeconds(.3f);
                        screenText.color = currentColor;
                        if (!doingTheMorseThing)
                            break;
                        yield return new WaitForSeconds(.8f);
                        screenText.color = new Color32(0, 0, 0, 0);
                    }
                    else
                    {
                        yield return new WaitForSeconds(.3f);
                    }
                }
                yield return new WaitForSeconds(.75f);
                if (!doingTheMorseThing)
                    break;
            }
        }

        screenText.color = currentColor;
    }
    
    IEnumerator NumberCycle(int[] numbers)
    {
        var index = 0;
        while (doingTheCycleThing)
        {
            screenText.text = numbers[index].ToString();
            index++;
            index %= 3;
            yield return new WaitForSeconds(.2f);
            screenText.text = "";
            yield return new WaitForSeconds(.05f);
        }
        screenText.text = numberDisplayed.ToString();
    }

    IEnumerator NumberSpin(bool clockwise)
    {
        while (doingTheSpinThing)
        {
            if (clockwise)
            {
                numberTransform.transform.Rotate(Vector3.back, 5);
                yield return new WaitForSeconds(.01f);
            }
            else
            {
                numberTransform.transform.Rotate(Vector3.back, -5);
                yield return new WaitForSeconds(.01f);
            }
        }
    }

    IEnumerator SlowSpin(bool clockwise)
    {
        var speed = 5f;
        for (int i = 0; i < 100; i++)
        {
            if (clockwise)
            {
                numberTransform.transform.Rotate(Vector3.back, speed);
                yield return new WaitForSeconds(.01f);
            }
            else
            {
                numberTransform.transform.Rotate(Vector3.back, -speed);
                yield return new WaitForSeconds(.01f);
            }
            speed *= .9f;
        }
    }

    IEnumerator ModuleSpin(int[] directions)
    {
        while (doingTheOtherSlightlyDifferentSpinThing)
        {
            foreach (int direction in directions)
            {
                if (!doingTheOtherSlightlyDifferentSpinThing)
                    break;
                if (direction < 3)
                {
                    for (int i = 0; i < (direction % 3 + 1) * 45; i++)
                    {
                        moduleTransform.transform.Rotate(Vector3.up, 2);
                        yield return new WaitForSeconds(.01f);
                    }
                }
                else
                {
                    for (int i = 0; i < (direction % 3 + 1) * 45; i++)
                    {
                        moduleTransform.transform.Rotate(Vector3.up, -2);
                        yield return new WaitForSeconds(.01f);
                    }
                }

                yield return new WaitForSeconds(.5f);
            }

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SayStuff(string[] words)
    {
        for (int i = 0; i < 3; i++)
        {
            if (shutTheFuckUp)
                break;
            Audio.PlaySoundAtTransform(words[i], Module.transform);
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator CheckTime(int currentDisplayed)
    {
        yield return new WaitForSeconds(2);

        if (currentDisplayed - 1 == numberDisplayed && !solved)
        {
            if (lastStageWasWrong)
                lastStageWasWrong = false;
            else
            {
                Module.HandleStrike();
                Debug.LogFormat("[Masher The Bottun #{0}] You didn't press in time! Strike.", _moduleId);
            }
        }
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use <!{0} masher> to masher the btmuno until something happens. Use <!{0} masher at 5> to masher the bnutianot when the last digit of the bomb's timer is that number. Use two digits to specify the last 2 digits.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.Trim().ToUpperInvariant();
        if (command == "MASHER")
        {
            yield return null;
            do
            {
                btnSelectable.OnInteract();
                yield return new WaitForSeconds(0.1f);
            } while (!positions.Contains(numberDisplayed));
        }
        else if (Regex.IsMatch(command, @"^MASHER\s+(AT\s+)?[0-9]$"))
        {
            yield return null;
            int submit = command.Last() - '0';
            while ((int)Info.GetTime() % 10 == submit)
                yield return null;
            while ((int)Info.GetTime() % 10 != submit)
                yield return "trycancel you stopped the buoont pressing?!!?!? wtf how!?!!?!";
            do
            {
                btnSelectable.OnInteract();
                yield return new WaitForSeconds(0.1f);
            } while (!positions.Contains(numberDisplayed));
        }
        else if (Regex.IsMatch(command, @"^MASHER\s+(AT\s+)?[0-5][0-9]$"))
        {
            int submit = 10 * (command[command.Length - 2] - '0') + (command.Last() - '0');
            while ((int)Info.GetTime() % 60 == submit)
                yield return null;
            while ((int)Info.GetTime() % 60 != submit)
                yield return "trycancel you stopped the b pressing?!!?!? wtf how!?!!?!";
            do
            {
                btnSelectable.OnInteract();
                yield return new WaitForSeconds(0.1f);
            } while (!positions.Contains(numberDisplayed));
        }            
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (!solved)
        {
            while (!positions.Contains(numberDisplayed))
            {
                btnSelectable.OnInteract();
                yield return new WaitForSeconds(0.1f);
                if (solved) //Needs to break here or else the module will autosolve and then just wait on 0 forever.
                    yield break; 
            }
            switch (sectionUsed)
            {
                case 1:
                    while (!bgColorTargets.Contains((int)Info.GetTime() % 60))
                        yield return true;
                    break;
                case 6:
                    while (!spinTargets.Contains((int)Info.GetTime() % 60))
                        yield return true;
                    break;
                case 2:
                    while ((int)Info.GetTime() % 10 != bitch)
                        yield return true;
                    break;
                default:
                    while ((int)Info.GetTime() % 10 != target)
                        yield return true;
                    break;
            }
            btnSelectable.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }
}