using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ToDoList
{
    public interface IListIO

    {
        void SaveTaskList(List<Task> list);

        void LoadTaskList(ToDoList list);

    }

    public class ListIO : IListIO
    {

        public void SaveTaskList(List<Task> list)
        {
            SaveFileDialog theDialog = new SaveFileDialog();
            theDialog.Title = "Save to-do list";
            theDialog.Filter = "TXT files|*.txt";
            theDialog.InitialDirectory = @".";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //MessageBox.Show(theDialog.FileName.ToString());
                    using (TextWriter TW = new StreamWriter(theDialog.FileName))
                    {
                        foreach (Task task in list)
                        {
                            TW.WriteLine(task.IsDone + "," + task.Description);
                        }
                    }
                    Process.Start(theDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not write file.  Error: " + ex.Message);
                }
            }
        }

        public void LoadTaskList(ToDoList list)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open existing to-do list?";
            theDialog.Filter = "TXT files|*.txt";
            theDialog.InitialDirectory = @".";
            if (theDialog.ShowDialog() == DialogResult.OK)
                try 
                {
                    string filename = theDialog.FileName;
                    System.Console.WriteLine("Reading file {0}", filename);
                    StreamReader file = new StreamReader(filename);
                    int counter = 0;
                    string line;
                    string status;
                    bool taskIsDone;
                    string description;

                    while ((line = file.ReadLine()) != null)
                    {
                        System.Console.WriteLine(line);
                        string[] taskElements = line.Split(',');
                        status = taskElements[0];
                        description = taskElements[1];
                        if (status == "False")
                            taskIsDone = false;
                        else
                            taskIsDone = true;

                        if (taskElements.Length == 2)
                            list.AddTaskToList(description, taskIsDone);
                        else
                            MessageBox.Show("There is an error in the ToDoList.txt file.  It is not in the proper format.");

                        counter++;
                    }

                    file.Close();
                    System.Console.WriteLine("There were {0} lines.", counter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not read file.  Error: " + ex.Message);
                }
        }
    }
}
