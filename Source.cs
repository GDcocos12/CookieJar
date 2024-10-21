using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookieJar
{
    public partial class MainWindow : Window
    {
        private Stack<int> stack;

        public MainWindow()
        {
            InitializeComponent();
            stack = new Stack<int>();
            UpdateStackDisplay();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCommand(RemoveComments(CodeTextBox.Text));
            UpdateStackDisplay();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            stack.Clear();
            ResultTextBox.Text = string.Empty;
            DebugTextBox.Text = string.Empty;
            UpdateStackDisplay();
        }

        private Dictionary<string, int> labels = new Dictionary<string, int>();

        private string RemoveComments(string code)
        {
            string[] lines = code.Split(new[] { '\n' }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                bool inComment = false;
                int commentStartIndex = -1;

                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '/' && j + 1 < lines[i].Length && lines[i][j + 1] == '*')
                    {
                        inComment = true;
                        commentStartIndex = j;
                        j++;
                    }
                    else if (lines[i][j] == '*' && j + 1 < lines[i].Length && lines[i][j + 1] == '/')
                    {
                        if (inComment && commentStartIndex >= 0)
                        {
                            lines[i] = lines[i].Remove(commentStartIndex, j - commentStartIndex + 2);
                            j = commentStartIndex - 1;
                            inComment = false;
                        }
                    }
                }
            }

            return string.Join("\n", lines);
        }

        private void ExecuteCommand(string command)
        {
            string[] commands = command.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < commands.Length; i++)
            {
                string trimmedCommand = commands[i].Trim();
                if (trimmedCommand.EndsWith(":"))
                {
                    string labelName = trimmedCommand.Substring(0, trimmedCommand.Length - 1).Trim();
                    labels[labelName] = i;
                }
            }

            for (int currentCommandIndex = 0; currentCommandIndex < commands.Length; currentCommandIndex++)
            {
                string trimmedCommand = commands[currentCommandIndex].Trim();
                DebugTextBox.Text += (trimmedCommand + '\n');

                if (trimmedCommand.EndsWith(":"))
                {
                    continue;
                }

                if (trimmedCommand == "FINISH")
                {
                    ResultTextBox.Text += "\nProgram finished.";
                    break;
                }

                if (trimmedCommand.StartsWith("PUSH "))
                {
                    string valueString = trimmedCommand.Substring(5).Trim();
                    if (int.TryParse(valueString, out int number))
                    {
                        stack.Push(number);
                        AddStackValue(number);
                    }
                }
                else if (trimmedCommand == "POP")
                {
                    if (stack.Count > 0)
                    {
                        int poppedValue = stack.Pop();
                        UpdateStackDisplay(); 
                    }
                    else
                    {
                        ResultTextBox.Text += "\nStack is empty";
                    }
                }
                else if (trimmedCommand == "ADD")
                {
                    if (stack.Count >= 2)
                    {
                        int a = stack.Pop();
                        int b = stack.Pop();
                        stack.Push(a + b);
                    }
                    else
                    {
                        ResultTextBox.Text += "\nNot enough values on the stack";
                    }
                }
                else if (trimmedCommand == "SUB")
                {
                    if (stack.Count >= 2)
                    {
                        int a = stack.Pop();
                        int b = stack.Pop();
                        stack.Push(b - a);
                    }
                    else
                    {
                        ResultTextBox.Text += "\nNot enough values on the stack";
                    }
                }
                else if (trimmedCommand == "MUL")
                {
                    if (stack.Count >= 2)
                    {
                        int a = stack.Pop();
                        int b = stack.Pop();
                        stack.Push(a * b);
                    }
                    else
                    {
                        ResultTextBox.Text += "\nNot enough values on the stack";
                    }
                }
                else if (trimmedCommand == "DIV")
                {
                    if (stack.Count >= 2)
                    {
                        int a = stack.Pop();
                        int b = stack.Pop();
                        if (a != 0)
                        {
                            stack.Push(b / a);
                        }
                        else
                        {
                            ResultTextBox.Text += "\nCannot divide by zero";
                            stack.Push(b);
                            stack.Push(a);
                        }
                    }
                    else
                    {
                        ResultTextBox.Text += "\nNot enough values on the stack";
                    }
                }
                else if (trimmedCommand == "REM")
                {
                    if (stack.Count >= 2)
                    {
                        int a = stack.Pop();
                        int b = stack.Pop();
                        stack.Push(b % a);
                    }
                    else
                    {
                        ResultTextBox.Text += "\nNot enough values on the stack";
                    }
                }
                else if (trimmedCommand == "CPY")
                {
                    if (stack.Count > 0)
                    {
                        int topValue = stack.Peek();
                        stack.Push(topValue);
                    }
                    else
                    {
                        ResultTextBox.Text += "\nStack is empty";
                    }
                }
                else if (trimmedCommand.StartsWith("PRINT "))
                {
                    string textToPrint = trimmedCommand.Substring(5);
                    ResultTextBox.Text += $"\nOutput: {textToPrint}";
                }
                else if (trimmedCommand == "TOP")
                {
                    if (stack.Count > 0)
                    {
                        int topValue = stack.Peek();
                        ResultTextBox.Text += $"\nTop value: {topValue}";
                    }
                    else
                    {
                        ResultTextBox.Text += "\nStack is empty";
                    }
                }
                else if (trimmedCommand.StartsWith("JUMPIFNULL "))
                {
                    string labelName = trimmedCommand.Substring(11).Trim();

                    if (stack.Count > 0 && stack.Peek() == 0)
                    {
                        if (labels.TryGetValue(labelName, out int targetIndex))
                            currentCommandIndex = targetIndex;
                        else
                            ResultTextBox.Text += $"\nLabel '{labelName}' not found.";

                        continue;
                    }
                }
                else if (trimmedCommand.StartsWith("JUMPIFNOTNULL "))
                {
                    string labelName = trimmedCommand.Substring(14).Trim();

                    if (stack.Count > 0 && stack.Peek() != 0)
                    {
                        if (labels.TryGetValue(labelName, out int targetIndex))
                            currentCommandIndex = targetIndex;
                        else
                            ResultTextBox.Text += $"\nLabel '{labelName}' not found.";

                        continue;
                    }
                }

            }

            labels.Clear();
        }

        private void UpdateStackDisplay()
        {
            StackPanelValues.Children.Clear();
            foreach (var value in stack)
            {
                AddStackValue(value);
            }
        }

        private void AddStackValue(int value)
        {
            var border = new Border
            {
                Background = (StackPanelValues.Children.Count % 2 == 0) ? Brushes.LightGray : Brushes.DarkGray,
                //Margin = new Thickness(5),
                Padding = new Thickness(5),
                Child = new TextBlock
                {
                    Text = value.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center
                }
            };

            StackPanelValues.Children.Add(border);
        }
    }
}
