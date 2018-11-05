using System;

namespace ConsoleHelper
{
    public static class ConsoleHelper
    {
        #region GetInput definition
        public delegate bool InputStringCondition(string input);

        public static string GetInput(string label, string failResponse, params InputStringCondition[] conditions)
        {
            string output;
            bool doContinue;
            do
            {
                Console.Write(label);
                output = Console.ReadLine();

                doContinue = false;
                foreach (var condition in conditions)
                {
                    doContinue = doContinue || !condition(output);
                }

                if (doContinue)
                {
                    Console.WriteLine(failResponse);
                }
            } while (doContinue);
            return output;
        }

        public static string GetInput(params InputStringCondition[] conditions)
        {
            return GetInput("", "", conditions);
        }

        public static string GetInput(string label)
        {
            return GetInput(label, "");
        }

        public static string GetInput(string failResponse, params InputStringCondition[] conditions)
        {
            return GetInput("", failResponse, conditions);
        }

        public static string GetNonEmptyInput(string label = "", string failReponse = "")
        {
            return GetInput(label, failReponse, x => !string.IsNullOrEmpty(x));
        }
        #endregion

        #region GetMaskedInput definition
        private const char ZERO_WIDTH_SPACE = '\x200B';

        public static string GetMaskedInput(string label, string failResponse, char mask, params InputStringCondition[] conditions)
        {
            string output;
            bool doContinue;
            do
            {
                Console.Write(label);

                // Mostly equivalent to Console.ReadLine(), except notably the mask character is printed instead of the typed character
                ConsoleKeyInfo inputKey;
                output = "";
                do
                {
                    inputKey = Console.ReadKey(true);
                    if (CharIsNotAControl(inputKey.KeyChar))
                    {
                        output += inputKey.KeyChar.ToString();
                        // The console improperly displays ZeroWidthSpaces as ?
                        if (mask != ZERO_WIDTH_SPACE) Console.Write(mask);
                    }
                } while (inputKey.Key != ConsoleKey.Enter);
                Console.WriteLine();

                doContinue = false;
                foreach (var condition in conditions)
                {
                    doContinue = doContinue || !condition(output);
                }

                if (doContinue)
                {
                    Console.WriteLine(failResponse);
                }
            } while (doContinue);
            return output;
        }

        public static string GetMaskedInput(string label, string failResponse, params InputStringCondition[] conditions)
        {
            return GetMaskedInput(label, failResponse, ZERO_WIDTH_SPACE, conditions);
        }

        public static string GetMaskedInput(params InputStringCondition[] conditions)
        {
            return GetMaskedInput("", "", ZERO_WIDTH_SPACE, conditions);
        }

        public static string GetMaskedInput(string label, char mask = ZERO_WIDTH_SPACE)
        {
            return GetMaskedInput(label, "", mask);
        }

        public static string GetMaskedInput(string failResponse, params InputStringCondition[] conditions)
        {
            return GetMaskedInput("", failResponse, ZERO_WIDTH_SPACE, conditions);
        }

        public static string GetNonEmptyMaskedInput(string label = "", string failReponse = "", char mask = ZERO_WIDTH_SPACE)
        {
            return GetMaskedInput(label, failReponse, mask, x => !string.IsNullOrEmpty(x));
        }

        private static bool CharIsNotAControl(char c)
        {
            return !(c <= '\x001F' || (c >= '\x0080' && c <= '\x00A0'));
        }
        #endregion

        #region GetMenuSelection definition
        public static int GetMenuSelectionValue(params string[] options)
        {
            Console.CursorVisible = false;

            int selectionIndex = 0;

            do
            {
                Console.Write(GenerateMenuString(options, selectionIndex));
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    selectionIndex = Math.Clamp(selectionIndex - 1, 0, options.Length - 1);
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    selectionIndex = Math.Clamp(selectionIndex + 1, 0, options.Length - 1);
                }
                else if (key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    Console.WriteLine();
                    return selectionIndex;
                }
                Console.CursorLeft = 0;
            } while (true);
        }

        private static string GenerateMenuString(string[] options, int selectionIndex)
        {
            string output = "";
            for (int i = 0; i < options.Length; i++)
            {
                output +=
                    (i == selectionIndex     ? "[" : "") +
                    options[i] +
                    (i == selectionIndex     ? "]" : "") +
                    (i <  options.Length - 1 ? " " : "");
            }
            return output;
        }

        public static string GetMenuSelectionString(params string[] options)
        {
            return options[GetMenuSelectionValue(options)];
        }
        #endregion

        #region WaitForKeypress definition
        public static void WaitForKeypress(string label = "")
        {
            Console.Write(label);
            Console.ReadKey();
        }
        #endregion
    }
}
